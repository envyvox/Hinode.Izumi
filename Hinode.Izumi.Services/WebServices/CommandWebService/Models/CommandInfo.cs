using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Models
{
    public class CommandInfo
    {
        public CommandCategory[] Categories { get; set; }
        public string Command { get; set; }
        public string Summary { get; set; }
        public string[] Usages { get; set; }
    }
}
