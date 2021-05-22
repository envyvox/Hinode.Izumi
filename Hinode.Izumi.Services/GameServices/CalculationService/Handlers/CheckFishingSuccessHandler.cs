using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class CheckFishingSuccessHandler : IRequestHandler<CheckFishingSuccessQuery, bool>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public CheckFishingSuccessHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(CheckFishingSuccessQuery request, CancellationToken cancellationToken)
        {
            var (userFishingMastery, rarity) = request;
            var property = rarity switch
            {
                FishRarity.Common => MasteryProperty.FishFailChanceCommon,
                FishRarity.Rare => MasteryProperty.FishFailChanceRare,
                FishRarity.Epic => MasteryProperty.FishFailChanceEpic,
                FishRarity.Mythical => MasteryProperty.FishFailChanceMythical,
                FishRarity.Legendary => MasteryProperty.FishFailChanceLegendary,
                FishRarity.Divine => MasteryProperty.FishFailChanceDivine,
                _ => throw new ArgumentOutOfRangeException()
            };
            var chance = (await _mediator.Send(new GetMasteryPropertiesQuery(property), cancellationToken))
                .MasteryMaxValue(userFishingMastery);

            return _random.Next(1, 101) > chance;
        }
    }
}
