using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record CheckCardInUserDeckQuery(long UserId, long CardId) : IRequest<bool>;
}
