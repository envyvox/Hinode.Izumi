using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Commands
{
    public record AddCollectionToUserCommand(long UserId, CollectionCategory Category, long ItemId) : IRequest;
}
