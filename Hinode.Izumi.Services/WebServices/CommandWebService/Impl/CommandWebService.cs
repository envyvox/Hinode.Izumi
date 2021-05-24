using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Framework.Autofac;
using CommandInfo = Hinode.Izumi.Services.WebServices.CommandWebService.Models.CommandInfo;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Impl
{
    [InjectableService]
    public class CommandWebService : ICommandWebService
    {
        private readonly CommandService _commandService;

        public CommandWebService(CommandService commandService)
        {
            _commandService = commandService;
        }

        public IEnumerable<CommandInfo> GetCommands()
        {
            // заполняем информацию для каждой команды, где есть описание
            var commandsInfo = new List<CommandInfo>();
            foreach (var command in _commandService.Commands
                .Where(x => x.Summary is not null))
            {
                // получаем категории команды
                var categories = command.Module.Attributes
                    .Where(attribute => attribute.GetType() == typeof(CommandCategoryAttribute))
                    .SelectMany(attribute => ((CommandCategoryAttribute) attribute).Categories)
                    .ToArray();
                // получаем параметры команды
                var parameters = command.Parameters.Aggregate(string.Empty,
                    (current, parameter) => current + $"[{parameter.Summary}] ");
                // получаем примеры использования команды
                var usages = command.Attributes
                    .Where(attribute => attribute.GetType() == typeof(CommandUsageAttribute))
                    .SelectMany(attribute => ((CommandUsageAttribute) attribute).Usages)
                    .ToArray();
                // получаем требуемую локацию группы
                var location = command.Module.Preconditions
                    .Where(attribute => attribute.GetType() == typeof(IzumiRequireLocation))
                    .Select(attribute => ((IzumiRequireLocation) attribute).Location)
                    .FirstOrDefault();

                // если требуемая локация группы пустая, нужно убедиться что требования нет на самой команде
                if (location == 0)
                {
                    location = command.Preconditions
                        .Where(attribute => attribute.GetType() == typeof(IzumiRequireLocation))
                        .Select(attribute => ((IzumiRequireLocation) attribute).Location)
                        .FirstOrDefault();
                }

                // заполняем информацию о команде
                commandsInfo.Add(new CommandInfo
                {
                    Categories = categories,
                    Location = location,
                    Command = parameters.Length > 0
                        ? $"!{command.Aliases[0]} {parameters.Remove(parameters.Length - 1)}"
                        : $"!{command.Aliases[0]}",
                    Summary = command.Summary,
                    Usages = usages
                });
            }

            // возвращаем список команд
            return commandsInfo;
        }
    }
}
