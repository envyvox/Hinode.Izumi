using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetRandomFishWithRarityQuery(FishRarity Rarity) : IRequest<FishRecord>;
}
