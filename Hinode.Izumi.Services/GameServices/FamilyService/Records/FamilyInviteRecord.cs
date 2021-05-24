namespace Hinode.Izumi.Services.GameServices.FamilyService.Records
{
    public record FamilyInviteRecord(long Id, long FamilyId, long UserId)
    {
        public FamilyInviteRecord() : this(default, default, default)
        {
        }
    }
}
