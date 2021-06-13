namespace Hinode.Izumi.Data.Models
{
    public class Image : EntityBase
    {
        public Enums.Image Type { get; set; }
        public string Url { get; set; }
    }
}
