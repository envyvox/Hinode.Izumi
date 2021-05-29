using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Records
{
    public record UserCropRecord(
        long UserId,
        long CropId,
        long Amount,
        string Name,
        Season Season)
    {
        public UserCropRecord() : this(default, default, default, default, default)
        {
        }
    }
}
