namespace Hinode.Izumi.Services.GameServices.PropertyService.Records
{
    public record MasteryXpPropertyRecord(
        double Mastery0,
        double Mastery50,
        double Mastery100,
        double Mastery150,
        double Mastery200,
        double Mastery250)
    {
        public MasteryXpPropertyRecord() : this(default, default, default, default, default, default)
        {
        }
    }
}
