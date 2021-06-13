namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCrop : EntityBase
    {
        public long UserId { get; set; }
        public long CropId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Crop Crop { get; set; }
    }
}
