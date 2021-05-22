using Discord;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordUserMessageQuery(long ChannelId, long MessageId) : IRequest<IUserMessage>;
}
