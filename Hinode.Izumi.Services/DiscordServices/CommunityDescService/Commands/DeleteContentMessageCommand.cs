using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record DeleteContentMessageCommand(long ChannelId, long MessageId) : IRequest;
}
