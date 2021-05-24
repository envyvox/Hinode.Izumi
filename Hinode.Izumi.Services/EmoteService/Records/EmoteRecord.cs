namespace Hinode.Izumi.Services.EmoteService.Records
{
    public record EmoteRecord(long Id, string Name, string Code)
    {
        public EmoteRecord() : this(default, default, default)
        {
        }
    }
}
