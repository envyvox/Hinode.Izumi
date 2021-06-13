namespace Hinode.Izumi.Data.Models
{
    public class Food : EntityBase
    {
        public string Name { get; set; }
        public long Mastery { get; set; }
        public long Time { get; set; }
        public bool RecipeSellable { get; set; }
        public bool Event { get; set; }
    }
}
