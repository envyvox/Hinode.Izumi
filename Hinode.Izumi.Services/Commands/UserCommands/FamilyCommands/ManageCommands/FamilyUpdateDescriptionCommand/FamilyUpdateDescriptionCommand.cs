using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyUpdateDescriptionCommand
{
    [InjectableService]
    public class FamilyUpdateDescriptionCommand : IFamilyUpdateDescriptionCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IFamilyService _familyService;

        public FamilyUpdateDescriptionCommand(IDiscordEmbedService discordEmbedService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context, string newDescription)
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
                // получаем семью
                var family = await _familyService.GetFamily(userFamily.FamilyId);

                // проверяем что семья прошла этап регистрации
                if (family.Status == FamilyStatus.Registration)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyStatusRegistration.Parse()));
                }
                // проверяем что новое описание семьи не длинее максимального значения
                else if (newDescription.Length > 1024)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyUpdateDescriptionMaxLength.Parse()));
                }
                else
                {
                    // обновляем информацию о семье
                    await _familyService.UpdateFamilyDescription(userFamily.FamilyId, newDescription);

                    var embed = new EmbedBuilder()
                        // подтверждаем что информация о семье успешно обновлена
                        .WithDescription(IzumiReplyMessage.FamilyUpdateDescriptionSuccess.Parse());

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
