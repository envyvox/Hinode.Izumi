using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record CheckContentMessageAuthorLikesCommand(long ContentMessageId) : IRequest;
}
