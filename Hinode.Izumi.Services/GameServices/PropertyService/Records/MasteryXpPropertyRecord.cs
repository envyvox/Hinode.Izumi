namespace Hinode.Izumi.Services.GameServices.PropertyService.Records
{
    public record MasteryXpPropertyRecord(
        long Mastery0,
        long Mastery50,
        long Mastery100,
        long Mastery150,
        long Mastery200,
        long Mastery250)
    {
        public MasteryXpPropertyRecord() : this(default, default, default, default, default, default)
        {
        }
    }
}
