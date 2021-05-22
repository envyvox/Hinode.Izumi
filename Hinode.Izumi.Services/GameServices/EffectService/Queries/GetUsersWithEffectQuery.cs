using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetUsersWithEffectQuery(Effect Effect) : IRequest<UserRecord[]>;
}
