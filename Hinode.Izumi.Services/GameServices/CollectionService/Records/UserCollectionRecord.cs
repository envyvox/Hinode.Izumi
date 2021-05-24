namespace Hinode.Izumi.Services.GameServices.CollectionService.Records
{
    public record UserCollectionRecord(long UserId, long ItemId)
    {
        public UserCollectionRecord() : this(default, default)
        {
        }
    }
}
