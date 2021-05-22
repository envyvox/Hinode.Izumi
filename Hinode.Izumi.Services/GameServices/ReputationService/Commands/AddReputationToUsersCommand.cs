using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Commands
{
    public record AddReputationToUsersCommand(long[] UsersId, Reputation Reputation, long Amount) : IRequest;
}
