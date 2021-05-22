using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetCraftingAmountAfterMasteryProcsHandler
        : IRequestHandler<GetCraftingAmountAfterMasteryProcsQuery, long>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public GetCraftingAmountAfterMasteryProcsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetCraftingAmountAfterMasteryProcsQuery request,
            CancellationToken cancellationToken)
        {
            var (craftingId, userCraftingMastery, amount) = request;
            var doubleChance = (await _mediator.Send(
                    new GetCraftingPropertiesQuery(craftingId, CraftingProperty.CraftingDoubleChance),
                    cancellationToken))
                .MasteryMaxValue(userCraftingMastery);
            var returnAmount = amount;

            for (var i = 0; i < amount; i++)
                if (doubleChance >= _random.Next(0, 101))
                    returnAmount++;

            return returnAmount;
        }
    }
}
