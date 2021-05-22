using Hinode.Izumi.Data.Enums.EffectEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Commands
{
    public record RemoveEffectFromUserCommand(long UserId, Effect Effect) : IRequest;
}
