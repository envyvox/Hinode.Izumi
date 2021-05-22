using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserBoxesQuery(long UserId) : IRequest<Dictionary<Box, UserBoxRecord>>;
}
