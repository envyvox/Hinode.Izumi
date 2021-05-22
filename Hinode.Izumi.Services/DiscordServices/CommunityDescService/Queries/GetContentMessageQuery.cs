using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentMessageQuery(long ChannelId, long MessageId) : IRequest<ContentMessageRecord>;
}
