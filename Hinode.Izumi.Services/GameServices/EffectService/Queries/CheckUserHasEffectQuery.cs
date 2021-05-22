using Hinode.Izumi.Data.Enums.EffectEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record CheckUserHasEffectQuery(long UserId, Effect Effect) : IRequest<bool>;
}
