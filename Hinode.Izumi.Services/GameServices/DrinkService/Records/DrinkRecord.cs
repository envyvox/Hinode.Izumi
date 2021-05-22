namespace Hinode.Izumi.Services.GameServices.DrinkService.Records
{
    public record DrinkRecord(long Id, string Name, long Time)
    {
        public DrinkRecord() : this(default, default, default)
        {
        }
    }
}
