using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Models
{
    public class DiscordChannelModel : EntityBaseModel
    {
        /// <summary>
        /// Канал дискорда.
        /// </summary>
        public DiscordChannel Channel { get; set; }
    }
}
