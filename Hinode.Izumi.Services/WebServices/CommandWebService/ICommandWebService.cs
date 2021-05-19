using System.Collections.Generic;
using Hinode.Izumi.Services.WebServices.CommandWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CommandWebService
{
    public interface ICommandWebService
    {
        IEnumerable<CommandInfo> GetCommands();
    }
}
