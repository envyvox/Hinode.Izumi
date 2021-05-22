using Hinode.Izumi.Services.GameServices.EffectService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetUserEffectsQuery(long UserId) : IRequest<UserEffectRecord[]>;
}
