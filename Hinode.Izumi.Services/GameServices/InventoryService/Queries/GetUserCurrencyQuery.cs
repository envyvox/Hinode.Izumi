using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserCurrencyQuery(long UserId, Currency Currency) : IRequest<UserCurrencyRecord>;
}
