using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserDeckLengthQuery(long UserId) : IRequest<int>;
}
