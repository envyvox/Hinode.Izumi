using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string About { get; set; }
        public Title Title { get; set; }
        public Gender Gender { get; set; }
        public Location Location { get; set; }
        public int Energy { get; set; }
        public long Points { get; set; }
        public bool Premium { get; set; }
    }
}
