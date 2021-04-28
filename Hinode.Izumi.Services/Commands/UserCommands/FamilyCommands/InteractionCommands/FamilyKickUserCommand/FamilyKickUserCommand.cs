using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyKickUserCommand
{
    [InjectableService]
    public class FamilyKickUserCommand : IFamilyKickUserCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFamilyService _familyService;
        private readonly IUserService _userService;
        private readonly IDiscordGuildService _discordGuildService;

        public FamilyKickUserCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFamilyService familyService, IUserService userService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _familyService = familyService;
            _userService = userService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketCommandContext context, string username)
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
                // получаем пользователя цель
                var tUser = await _userService.GetUser(username);

                // проверяем что пользователь не является целью
                if (tUser.Id == (long) context.User.Id)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyKickYourself.Parse()));
                }
                else
                {
                    // получаем информацию о том, в какой семье постоит цель
                    var tUserFamily = await _familyService.GetUserFamily(tUser.Id);

                    // проверяем что цель находится в одной семье с пользователем
                    if (tUserFamily.FamilyId != userFamily.FamilyId)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UserNotInYourFamily.Parse()));
                    }
                    else
                    {
                        // исключаем цель из семьи
                        await _familyService.RemoveUserFromFamily(tUser.Id);

                        // получаем иконки из базы
                        var emotes = await _emoteService.GetEmotes();
                        var embed = new EmbedBuilder()
                            // подтверждаем успешное исключение цели из семьи
                            .WithDescription(IzumiReplyMessage.FamilyKickUserSuccess.Parse(
                                emotes.GetEmoteOrBlank(tUser.Title.Emote()),
                                tUser.Title.Localize(), tUser.Name));

                        await _discordEmbedService.SendEmbed(context.User, embed);

                        // получаем семью пользователя
                        var family = await _familyService.GetFamily(tUserFamily.FamilyId);
                        var notify = new EmbedBuilder()
                            // оповещаем цель о том, что ее исключили
                            .WithDescription(IzumiReplyMessage.FamilyKickUserSuccessNotify.Parse(family.Name));

                        await _discordEmbedService.SendEmbed(
                            await _discordGuildService.GetSocketUser(tUser.Id), notify);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
