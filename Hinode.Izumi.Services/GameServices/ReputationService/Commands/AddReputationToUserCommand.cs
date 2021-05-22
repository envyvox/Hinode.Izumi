using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Commands
{
    public record AddReputationToUserCommand(long UserId, Reputation Reputation, long Amount) : IRequest;
}
