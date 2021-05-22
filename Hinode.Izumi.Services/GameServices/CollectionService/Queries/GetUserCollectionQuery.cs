using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.CollectionService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Queries
{
    public record GetUserCollectionQuery(long UserId, CollectionCategory Category) : IRequest<UserCollectionRecord[]>;
}
