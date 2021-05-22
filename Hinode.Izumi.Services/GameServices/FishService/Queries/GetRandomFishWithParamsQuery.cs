using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetRandomFishWithParamsQuery(
            TimesDay TimesDay,
            Season Season,
            Weather Weather,
            FishRarity FishRarity)
        : IRequest<FishRecord>;
}
