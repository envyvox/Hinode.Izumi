using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyUpdateUserStatusCommand
{
    [InjectableService]
    public class FamilyUpdateUserStatusCommand : IFamilyUpdateUserStatusCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IUserService _userService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IFamilyService _familyService;

        public FamilyUpdateUserStatusCommand(IDiscordEmbedService discordEmbedService, IUserService userService,
            IDiscordGuildService discordGuildService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _userService = userService;
            _discordGuildService = discordGuildService;
            _familyService = familyService;
        }

        public async Task Execute(SocketCommandContext context, UserInFamilyStatus newStatus, string username)
        {
            // получаем пользователя в семье
            var userFamily = await _familyService.GetUserFamily((long) context.User.Id);

            // проверяем что пользователь является владельцем семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // получаем семью пользователя
                var family = await _familyService.GetFamily(userFamily.FamilyId);

                // проверяем что семья прошла этап регистрации
                if (family.Status == FamilyStatus.Registration)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyStatusRegistration.Parse()));
                }
                else
                {
                    // получаем пользователя цель
                    var tUser = await _userService.GetUser(username);

                    // проверяем что пользователь не является целью
                    if (tUser.Id == (long) context.User.Id)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilySetUserStatusYourself.Parse()));
                    }
                    else
                    {
                        // получаем информацию о том, в какой семье состоит цель
                        var tUserFamily = await _familyService.GetUserFamily(tUser.Id);

                        // проверяем что пользователь и цель состоят в одной семье
                        if (tUserFamily.FamilyId != userFamily.FamilyId)
                        {
                            await Task.FromException(new Exception(IzumiReplyMessage.UserNotInYourFamily.Parse()));
                        }
                        // проверяем что пользователь не пытается повысить до главы семьи
                        else if (newStatus == UserInFamilyStatus.Head)
                        {
                            await Task.FromException(
                                new Exception(IzumiReplyMessage.FamilySetUserStatusCantBeHead.Parse()));
                        }
                        else
                        {
                            // обновляем статус цели в семье
                            await _familyService.UpdateUserInFamilyStatus(tUser.Id, newStatus);

                            var embed = new EmbedBuilder()
                                // подтверждаем что статус успешно обновлен
                                .WithDescription(IzumiReplyMessage.FamilySetUserStatusSuccess.Parse(
                                    newStatus.Localize()));

                            await _discordEmbedService.SendEmbed(context.User, embed);

                            var notify = new EmbedBuilder()
                                // оповещаем цель о том, что ей обновили статус в семье
                                .WithDescription(IzumiReplyMessage.FamilySetUserStatusSuccessNotify.Parse(
                                    newStatus.Localize()));

                            await _discordEmbedService.SendEmbed(
                                await _discordGuildService.GetSocketUser(tUser.Id), notify);
                            await Task.CompletedTask;
                        }
                    }
                }
            }
        }
    }
}
