using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record CreateContentMessageCommand(long UserId, long ChannelId, long MessageId) : IRequest;
}
