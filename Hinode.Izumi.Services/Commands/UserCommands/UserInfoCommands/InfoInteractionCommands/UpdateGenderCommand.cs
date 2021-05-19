using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [Group("подтвердить")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateGenderCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IUserService _userService;

        public UpdateGenderCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _userService = userService;
        }

        [Command("пол")]
        [Summary("Отправить запрос на подтверждение пола")]
        public async Task UpdateGenderTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя из базы
            var user = await _userService.GetUser((long) Context.User.Id);

            // если у пользователя уже установлен пол - он не может запросить его подверждение
            if (user.Gender != Gender.None)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UpdateGenderAlready.Parse(
                    emotes.GetEmoteOrBlank(user.Gender.ToString()), user.Gender.Localize(),
                    DiscordRole.Moderator.Name())));
            }
            else
            {
                var embed = new EmbedBuilder()
                    // подверждаем что запрос на смену пола успешно отправлен
                    .WithDescription(IzumiReplyMessage.UpdateGenderDesc.Parse(
                        DiscordRole.Moderator.Name(), emotes.GetEmoteOrBlank(Gender.None.ToString())));

                var notifyModEmbed = new EmbedBuilder()
                    // оповещаем модераторов о том, что пользователь просит подвердить ему пол
                    .WithDescription(
                        IzumiReplyMessage.UpdateGenderNotifyDesc.Parse(
                            Context.User.Mention, emotes.GetEmoteOrBlank(Gender.None.ToString())) +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // выводим готовые команды для модераторов
                    .AddField(IzumiReplyMessage.UpdateGenderNotifyFieldName.Parse(),
                        IzumiReplyMessage.UpdateGenderNotifyFieldDesc.Parse(
                            emotes.GetEmoteOrBlank(Gender.Male.ToString()), Gender.Male.Localize(), Context.User.Id,
                            emotes.GetEmoteOrBlank(Gender.Female.ToString()), Gender.Female.Localize()));

                await _discordEmbedService.SendEmbed(Context.User, embed);
                await _discordEmbedService.SendEmbed(DiscordChannel.ModeratorChat, notifyModEmbed, "@everyone");
                await Task.CompletedTask;
            }
        }
    }
}
