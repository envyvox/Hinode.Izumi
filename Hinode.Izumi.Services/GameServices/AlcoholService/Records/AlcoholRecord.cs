namespace Hinode.Izumi.Services.GameServices.AlcoholService.Records
{
    public record AlcoholRecord(long Id, string Name, long Time)
    {
        public AlcoholRecord() : this(default, default, default)
        {
        }
    }
}
