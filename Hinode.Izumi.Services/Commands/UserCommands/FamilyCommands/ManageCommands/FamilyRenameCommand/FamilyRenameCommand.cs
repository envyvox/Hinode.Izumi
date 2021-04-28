using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.RpgServices.CertificateService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand
{
    [InjectableService]
    public class FamilyRenameCommand : IFamilyRenameCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFamilyService _familyService;
        private readonly ICertificateService _certificateService;

        public FamilyRenameCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFamilyService familyService, ICertificateService certificateService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _familyService = familyService;
            _certificateService = certificateService;
        }

        public async Task Execute(SocketCommandContext context, string newFamilyName)
        {
            // проверяем есть ли у пользователя сертификат
            await _certificateService.GetUserCertificate(
                (long) context.User.Id, Certificate.FamilyRename.GetHashCode());
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // проверяем новое имя на валидность
                var checkValid = StringExtensions.CheckValid(newFamilyName);

                if (!checkValid)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameNotValid.Parse(newFamilyName)));
                }
                else
                {
                    // проверяем что с желаемым именем нет семьи
                    var familyWithName = await _familyService.CheckFamily(newFamilyName);

                    if (familyWithName)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameTaken.Parse(newFamilyName)));
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
                        else
                        {
                            // обновляем название семьи
                            await _familyService.UpdateFamilyName(userFamily.FamilyId, newFamilyName);

                            // получаем иконки из базы
                            var emotes = await _emoteService.GetEmotes();
                            var embed = new EmbedBuilder()
                                .WithDescription(
                                    // подтверждаем что семья успешно переименована
                                    IzumiReplyMessage.FamilyRenameSuccess.Parse(newFamilyName) +
                                    // говорим о том, что сертификат был изъят
                                    IzumiReplyMessage.CertRemoved.Parse(
                                        emotes.GetEmoteOrBlank("Certificate"),
                                        Certificate.FamilyRename.Localize().ToLower()));

                            await _discordEmbedService.SendEmbed(context.User, embed);
                            await Task.CompletedTask;
                        }
                    }
                }
            }
        }
    }
}
