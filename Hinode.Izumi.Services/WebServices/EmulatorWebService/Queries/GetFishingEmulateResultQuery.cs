using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.WebServices.EmulatorWebService.Models;
using MediatR;

namespace Hinode.Izumi.Services.WebServices.EmulatorWebService.Queries
{
    public record GetFishingEmulateResultQuery(FishingEmulateSetup Setup) : IRequest<FishingEmulateResult>;

    public class GetFishingEmulateResultHandler : IRequestHandler<GetFishingEmulateResultQuery, FishingEmulateResult>
    {
        private readonly IMediator _mediator;

        public GetFishingEmulateResultHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<FishingEmulateResult> Handle(GetFishingEmulateResultQuery request, CancellationToken ct)
        {
            var setup = request.Setup;
            var emulateResult = new FishingEmulateResult{FishingResults = new Dictionary<long, FishingResult>()};

            for (var e = 0; e < setup.EmulateCount; e++)
            {
                var result = new FishingResult();

                for (var f = 0; f < setup.FishingCount; f++)
                {
                    var fishRarity = await _mediator.Send(new GetRandomFishRarityQuery(
                        (long) Math.Floor(setup.FishingMastery)), ct);
                    var fish = await _mediator.Send(new GetRandomFishWithParamsQuery(
                        setup.TimesDay, setup.Season, setup.Weather, fishRarity), ct);
                    var success = await _mediator.Send(new CheckFishingSuccessQuery(
                        (long) Math.Floor(setup.FishingMastery), fish.Rarity), ct);

                    if (success)
                    {
                        result.SuccessCount++;
                        result.CurrencyReceived += fish.Price;

                        switch (fish.Rarity)
                        {
                            case FishRarity.Common:
                                result.CommonFishCount++;
                                break;
                            case FishRarity.Rare:
                                result.RareFishCount++;
                                break;
                            case FishRarity.Epic:
                                result.EpicFishCount++;
                                break;
                            case FishRarity.Mythical:
                                result.MythicalFishCount++;
                                break;
                            case FishRarity.Legendary:
                                result.LegendaryFishCount++;
                                break;
                            case FishRarity.Divine:
                                result.DivineFishCount++;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        result.FailCount++;
                    }

                    if (setup.MasteryStackable)
                    {
                        var masteryReceived = await _mediator.Send(new GetMasteryFishingXpQuery(
                            (long) Math.Floor(setup.FishingMastery), success), ct);
                        setup.FishingMastery += masteryReceived;
                        result.MasteryReceived += masteryReceived;
                    }
                }

                result.SuccessPercent = (decimal) result.SuccessCount / setup.FishingCount * 100;
                emulateResult.FishingResults.Add(e, result);
            }

            emulateResult.AverageSuccessPercent = emulateResult.FishingResults.Average(x => x.Value.SuccessPercent);
            emulateResult.AverageMasteryReceived = emulateResult.FishingResults.Average(x => x.Value.MasteryReceived);
            emulateResult.AverageCurrencyReceived = emulateResult.FishingResults.Average(x => x.Value.CurrencyReceived);

            return emulateResult;
        }
    }
}
