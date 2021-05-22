using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Commands
{
    public record AddCardToDeckCommand(long UserId, long CardId) : IRequest;
}
