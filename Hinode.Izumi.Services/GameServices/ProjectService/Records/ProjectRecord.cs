namespace Hinode.Izumi.Services.GameServices.ProjectService.Records
{
    public record ProjectRecord(
        long Id,
        string Name,
        long Price,
        long Time,
        long? ReqBuildingId)
    {
        public ProjectRecord() : this(default, default, default, default, default)
        {
        }
    }
}
