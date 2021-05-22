using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentAuthorVotesQuery(long UserId, Vote Vote) : IRequest<long>;
}
