namespace Hinode.Izumi.Services.GameServices.PropertyService.Records
{
    public record AlcoholPropertyRecord(
        long Mastery0,
        long Mastery50,
        long Mastery100,
        long Mastery150,
        long Mastery200,
        long Mastery250)
    {
        public AlcoholPropertyRecord() : this(default, default, default, default, default, default)
        {
        }
    }
}
