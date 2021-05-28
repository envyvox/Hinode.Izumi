using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Records
{
    public record UserCollectionRecord(
        long UserId,
        CollectionCategory Category,
        long ItemId,
        Event Event)
    {
        public UserCollectionRecord() : this(default, default, default, default)
        {
        }
    }
}
