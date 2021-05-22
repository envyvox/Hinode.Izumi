using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetAllCardLengthQuery : IRequest<long>;
}
