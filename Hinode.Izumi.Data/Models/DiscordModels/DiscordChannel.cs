namespace Hinode.Izumi.Data.Models.DiscordModels
{
    /// <summary>
    /// Канал дискорда. Id модели = Id канала.
    /// </summary>
    public class DiscordChannel : EntityBase
    {
        /// <summary>
        /// Канал дискорда.
        /// </summary>
        public Enums.DiscordEnums.DiscordChannel Channel { get; set; }
    }
}
