namespace Hinode.Izumi.Services.GameServices.LocalizationService.Records
{
    public record LocalizationRecord(
        long ItemId,
        string Name,
        string Single,
        string Double,
        string Multiply)
    {
        public LocalizationRecord() : this(default, default, default, default, default)
        {
        }
    }
}
