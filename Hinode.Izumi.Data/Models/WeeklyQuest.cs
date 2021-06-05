using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class WeeklyQuest : EntityBase
    {
        public Season Season { get; set; }
        public WeeklyQuestCategory Category { get; set; }
        public WeeklyQuestDifficulty Difficulty { get; set; }
        public string Name { get; set; }
        public InventoryCategory ItemCategory { get; set; }
        public long ItemId { get; set; }
        public long ItemAmount { get; set; }
    }
}
