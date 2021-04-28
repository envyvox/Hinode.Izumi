using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MuteJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Humanizer;

namespace Hinode.Izumi.Services.Commands.ModerationCommands.MuteCommand
{
    [InjectableService]
    public class MuteCommand : IMuteCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;

        private readonly Random _random = new();

        public MuteCommand(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketCommandContext context, long userId, long duration, string reason = null)
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем пользователя
            var user = await _discordGuildService.GetSocketUser(userId);
            // проверяем выдал ли блокировку администратор
            var admin = await _discordGuildService.CheckRoleInUser((long) context.User.Id, DiscordRole.Administration);
            // добавляем роль блокировки чата пользователю
            await _discordGuildService.ToggleRoleInUser(userId, DiscordRole.Mute, true);

            var embed = new EmbedBuilder()
                // случайный номер указа
                .WithTitle(IzumiReplyMessage.MuteTitle.Parse(
                    _random.Next(1000, 10000)))
                // указываем пользователя которому блокируют чат
                .WithDescription(IzumiReplyMessage.MuteDesc.Parse(
                    user.Mention, user.Username, channels[DiscordChannel.Rules].Id))
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    duration.Minutes().Humanize(2, new CultureInfo("ru-RU")), true)
                // причина
                .AddField(IzumiReplyMessage.MuteReasonFieldName.Parse(),
                    reason ?? IzumiReplyMessage.MuteReasonNull.Parse(), true)
                // кто выдал
                .AddField(IzumiReplyMessage.MuteSignatureFieldName.Parse(),
                    $"**{(admin ? DiscordRole.Administration.Name() : DiscordRole.Moderator.Name())}** {context.User.Mention} `@{context.User.Username}`");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Chat].Id), embed);

            // запускаем джобу для снятия блокировки
            BackgroundJob.Schedule<IMuteJob>(x => x.Unmute(userId), TimeSpan.FromMinutes(duration));
        }
    }
}
