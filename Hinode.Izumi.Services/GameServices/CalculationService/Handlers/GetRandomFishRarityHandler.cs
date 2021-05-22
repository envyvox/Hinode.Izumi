using System;
using System.Linq;
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
    public class GetRandomFishRarityHandler : IRequestHandler<GetRandomFishRarityQuery, FishRarity>
    {
        private readonly IMediator _mediator;
        private readonly Random _random = new();

        public GetRandomFishRarityHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<FishRarity> Handle(GetRandomFishRarityQuery request, CancellationToken cancellationToken)
        {
            var rarities = Enum.GetValues(typeof(FishRarity))
                .Cast<FishRarity>()
                .ToArray();

            // копипаста, не уверен точно как она работает ^^"
            while (true)
            {
                var rand = _random.Next(1, 101);
                long current = 0;
                foreach (var rarity in rarities)
                {
                    var property = rarity switch
                    {
                        FishRarity.Common => MasteryProperty.FishRarityChanceCommon,
                        FishRarity.Rare => MasteryProperty.FishRarityChanceRare,
                        FishRarity.Epic => MasteryProperty.FishRarityChanceEpic,
                        FishRarity.Mythical => MasteryProperty.FishRarityChanceMythical,
                        FishRarity.Legendary => MasteryProperty.FishRarityChanceLegendary,
                        FishRarity.Divine => MasteryProperty.FishRarityChanceDivine,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    var chance = (await _mediator.Send(new GetMasteryPropertiesQuery(property), cancellationToken))
                        .MasteryMaxValue(request.UserFishingMastery);

                    if (current <= rand && rand < current + chance) return rarity;
                    current += chance;
                }
            }
        }
    }
}
