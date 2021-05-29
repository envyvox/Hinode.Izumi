using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.BuildingCommands
{
    [CommandCategory(CommandCategory.Building)]
    [IzumiRequireRegistry]
    public class BuildingListCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private Dictionary<string, EmoteRecord> _emotes;

        public BuildingListCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("постройки"), Alias("buildings")]
        [Summary("Посмотреть возведенные вами или семьей постройки")]
        public async Task BuildingListTask()
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());

            // заполняем строку персональными постройками
            var personalBuildings = await DisplayPersonalBuildings();
            // заполняем строку семейными постройками
            var familyBuildings = await DisplayFamilyBuildings();

            var embed = new EmbedBuilder()
                .WithDescription(
                    IzumiReplyMessage.BuildingListDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // персональные постройки
                .AddField($"{_emotes.GetEmoteOrBlank("List")} {BuildingCategory.Personal.Localize()}",
                    (personalBuildings.Length > 0
                        // если у пользователя есть персональные постройки - выводим их
                        ? personalBuildings
                        // если нет - предлагаем купить чертеж
                        : IzumiReplyMessage.BuildingListPersonalBuildingsNull.Parse(
                            Location.Seaport.Localize(true), _emotes.GetEmoteOrBlank("Project"))) +
                    $"{_emotes.GetEmoteOrBlank("Blank")}")
                // семейные постройки
                .AddField($"{_emotes.GetEmoteOrBlank("List")} {BuildingCategory.Family.Localize()}",
                    familyBuildings.Length > 0
                        // если у семьи пользователя есть семейные постройки - выводим их
                        ? familyBuildings
                        // если нет - предлагаем купить чертеж
                        : IzumiReplyMessage.BuildingListFamilyBuildingsNull.Parse(
                            Location.Seaport.Localize(true), _emotes.GetEmoteOrBlank("Project")));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }

        private async Task<string> DisplayPersonalBuildings()
        {
            // получаем постройки пользователя
            var userBuildings = await _mediator.Send(new GetUserBuildingsQuery((long) Context.User.Id));

            // возвращаем локализированную строку с информацией о постройках
            return userBuildings.Aggregate(string.Empty, (current, building) => current +
                $"\n{_emotes.GetEmoteOrBlank(building.Type.ToString())} {building.Name}\n*{building.Description}*\n");
        }

        private async Task<string> DisplayFamilyBuildings()
        {
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery((long) Context.User.Id));

            // если пользователь не состоит в семье - предлагаем ему вступить или создать свою
            if (!hasFamily) return IzumiReplyMessage.FamilyInfoUserFamilyNull.Parse(
                    Location.Capital.Localize(true));

            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) Context.User.Id));
            // получаем семейные постройки
            var familyBuildings = await _mediator.Send(new GetFamilyBuildingsQuery(userFamily.FamilyId));

            // возвращаем локализированную строку с информацией о постройках
            return familyBuildings.Aggregate(string.Empty, (current, building) => current +
                $"\n{_emotes.GetEmoteOrBlank(building.Type.ToString())} {building.Name}\n*{building.Description}*\n");
        }
    }
}
