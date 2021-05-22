using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Commands
{
    public record RemoveCardFromDeckCommand(long UserId, long CardId) : IRequest;
}
