using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record CheckContentMessageDislikesCommand(long ContentMessageId) : IRequest;
}
