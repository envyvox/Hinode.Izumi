using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyDeleteCommand
{
    [InjectableService]
    public class FamilyDeleteCommand : IFamilyDeleteCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;

        public FamilyDeleteCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // удаляем семью
                await _familyService.RemoveFamily(userFamily.FamilyId);

                var embed = new EmbedBuilder()
                    // подтверждаем что семья успешно удалена
                    .WithDescription(IzumiReplyMessage.FamilyDeleteSuccess.Parse());

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
