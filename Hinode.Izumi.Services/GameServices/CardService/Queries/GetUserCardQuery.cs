using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserCardQuery(long UserId, long CardId) : IRequest<CardRecord>;
}
