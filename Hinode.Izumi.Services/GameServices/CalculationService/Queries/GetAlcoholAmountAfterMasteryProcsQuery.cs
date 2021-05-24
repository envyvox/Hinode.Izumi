using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetAlcoholAmountAfterMasteryProcsQuery(
            long AlcoholId,
            long UserCraftingMastery,
            long Amount)
        : IRequest<long>;

    public class GetAlcoholAmountAfterMasteryProcsHandler
        : IRequestHandler<GetAlcoholAmountAfterMasteryProcsQuery, long>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public GetAlcoholAmountAfterMasteryProcsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetAlcoholAmountAfterMasteryProcsQuery request,
            CancellationToken cancellationToken)
        {
            var (alcoholId, userCraftingMastery, amount) = request;
            var doubleChance = (await _mediator.Send(
                    new GetAlcoholPropertiesQuery(alcoholId, AlcoholProperty.CraftingDoubleChance), cancellationToken))
                .MasteryMaxValue(userCraftingMastery);
            var returnAmount = amount;

            for (var i = 0; i < amount; i++)
                if (doubleChance >= _random.Next(0, 101))
                    returnAmount++;

            return returnAmount;
        }
    }
}
