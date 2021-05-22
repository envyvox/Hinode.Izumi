using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record ActivateUserVoteCommand(long UserId, long ContentMessageId, Vote Vote) : IRequest;
}
