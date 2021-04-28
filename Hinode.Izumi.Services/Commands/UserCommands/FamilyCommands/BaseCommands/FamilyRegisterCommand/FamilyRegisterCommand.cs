using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.RpgServices.CertificateService;
using Hinode.Izumi.Services.RpgServices.FamilyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand
{
    [InjectableService]
    public class FamilyRegisterCommand : IFamilyRegisterCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICertificateService _certificateService;
        private readonly IFamilyService _familyService;

        public FamilyRegisterCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICertificateService certificateService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _certificateService = certificateService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context, string familyName)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // проверяем есть ли у пользователя сертификат на регистрацию семьи
            await _certificateService.GetUserCertificate(
                (long) context.User.Id, Certificate.FamilyRegistration.GetHashCode());

            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily((long) context.User.Id);

            if (hasFamily)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyAlready.Parse()));
            }
            else
            {
                // проверяем название семьи на валидность
                var checkValid = StringExtensions.CheckValid(familyName);

                if (!checkValid)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameNotValid.Parse(familyName)));
                }
                else
                {
                    // проверяем есть ли семья с таким названием
                    var checkFamily = await _familyService.CheckFamily(familyName);

                    if (checkFamily)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameTaken.Parse(familyName)));
                    }
                    else
                    {
                        // создаем семью
                        await _familyService.AddFamily(familyName);
                        // добавляем пользователя в семью
                        await _familyService.AddUserToFamily((long) context.User.Id, familyName);
                        // отнимаем сертификат у пользователя
                        await _certificateService.RemoveCertificateFromUser(
                            (long) context.User.Id, Certificate.FamilyRegistration.GetHashCode());

                        var embed = new EmbedBuilder()
                            // подтверждаем успешное создание семьи
                            .WithDescription(
                                IzumiReplyMessage.FamilyRegistrationSuccess.Parse(familyName) +
                                IzumiReplyMessage.CertRemoved.Parse(
                                    emotes.GetEmoteOrBlank("Certificate"),
                                    Certificate.FamilyRegistration.Localize().ToLower()));

                        await _discordEmbedService.SendEmbed(context.User, embed);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
