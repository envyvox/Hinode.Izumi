namespace Hinode.Izumi.Data.Models.DiscordModels
{
    /// <summary>
    /// Роль дискорда. Id модели = Id роли.
    /// </summary>
    public class DiscordRole : EntityBase
    {
        /// <summary>
        /// Роль дискорда.
        /// </summary>
        public Enums.DiscordEnums.DiscordRole Role { get; set; }
    }
}
