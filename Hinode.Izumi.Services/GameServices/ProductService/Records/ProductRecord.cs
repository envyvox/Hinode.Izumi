namespace Hinode.Izumi.Services.GameServices.ProductService.Records
{
    public record ProductRecord(long Id, string Name, long Price)
    {
        public ProductRecord() : this(default, default, default)
        {
        }
    }
}
