using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Commands
{
    public record AddCardToUserCommand(long UserId, long CardId) : IRequest;
}
