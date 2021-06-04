using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.WebServices.EmulatorWebService.Models;
using MediatR;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Queries
{
    public record GetExploreEmulateResultQuery(ExploreEmulateSetup Setup) : IRequest<ExploreEmulateResult>;

    public class GetExploreEmulateResultHandler
        : IRequestHandler<GetExploreEmulateResultQuery, ExploreEmulateResult>
    {
        private readonly IMediator _mediator;

        public GetExploreEmulateResultHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ExploreEmulateResult> Handle(GetExploreEmulateResultQuery request, CancellationToken ct)
        {
            var setup = request.Setup;
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(setup.Location), ct);
            var emulateResult = new ExploreEmulateResult {ExploreResults = new Dictionary<long, ExploreResult>()};

            for (var e = 0; e < setup.EmulateCount; e++)
            {
                var result = new ExploreResult {GatheringsReceived = new Dictionary<string, long>()};

                foreach (var gathering in gatherings) result.GatheringsReceived.Add(gathering.Name, 0);

                for (var i = 0; i < setup.ExploreCount; i++)
                {
                    long itemsCount = 0;
                    foreach (var gathering in gatherings)
                    {
                        var chance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                                gathering.Id, GatheringProperty.GatheringChance), ct))
                            .MasteryMaxValue((long) Math.Floor(setup.GatheringMastery));
                        var doubleChance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                                gathering.Id, GatheringProperty.GatheringDoubleChance), ct))
                            .MasteryMaxValue((long) Math.Floor(setup.GatheringMastery));
                        var amount = (await _mediator.Send(new GetGatheringPropertiesQuery(
                                gathering.Id, GatheringProperty.GatheringAmount), ct))
                            .MasteryMaxValue((long) Math.Floor(setup.GatheringMastery));
                        var successAmount = await _mediator.Send(new GetSuccessAmountQuery(
                            chance, doubleChance, amount), ct);

                        if (successAmount < 1) continue;

                        itemsCount += successAmount;
                        result.CurrencyReceived += gathering.Price * successAmount;
                        result.GatheringsReceived[gathering.Name] += successAmount;
                    }

                    if (itemsCount > 0)
                    {
                        result.SuccessCount++;
                        result.TotalItemsCount += itemsCount;

                        if (setup.MasteryStackable)
                        {
                            var masteryReceived = await _mediator.Send(new GetMasteryXpQuery(
                                    MasteryXpProperty.Gathering, (long) Math.Floor(setup.GatheringMastery), itemsCount),
                                ct);
                            setup.GatheringMastery += masteryReceived;
                            result.MasteryReceived += masteryReceived;
                        }
                    }
                    else
                    {
                        result.FailCount++;
                    }
                }

                result.SuccessPercent = (decimal) result.SuccessCount / setup.ExploreCount * 100;

                emulateResult.ExploreResults.Add(e, result);
            }

            emulateResult.AverageSuccessPercent = emulateResult.ExploreResults.Average(x => x.Value.SuccessPercent);
            emulateResult.AverageMasteryReceived = emulateResult.ExploreResults.Average(x => x.Value.MasteryReceived);
            emulateResult.AverageCurrencyReceived = emulateResult.ExploreResults.Average(x => x.Value.CurrencyReceived);

            return emulateResult;
        }
    }
}
