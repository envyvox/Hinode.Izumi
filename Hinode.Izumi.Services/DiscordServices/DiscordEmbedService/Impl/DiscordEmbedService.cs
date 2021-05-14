using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Impl
{
    [InjectableService]
    public class DiscordEmbedService : IDiscordEmbedService
    {
        private readonly IDiscordGuildService _discordGuildService;

        public DiscordEmbedService(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public Embed BuildEmbed(EmbedBuilder embedBuilder) =>
            embedBuilder
                // устанавливаем цвет embed-сообщения по-умолчанию
                .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                .Build();

        public async Task<IUserMessage> SendEmbed(SocketUser socketUser, EmbedBuilder embedBuilder, string message = "")
        {
            try
            {
                // отправляем embed-сообщение пользователю
                return await socketUser.SendMessageAsync(message, false, BuildEmbed(embedBuilder));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IUserMessage> SendEmbed(ISocketMessageChannel socketMessageChannel, EmbedBuilder embedBuilder,
            string message = "")
        {
            try
            {
                // отправляем embed-сообщение в текстовый канал
                return await socketMessageChannel.SendMessageAsync(message, false, BuildEmbed(embedBuilder));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IUserMessage> SendEmbed(long channelId, EmbedBuilder embedBuilder, string message = "")
        {
            var channel = await _discordGuildService.GetSocketTextChannel(channelId);
            return await SendEmbed(channel, embedBuilder, message);
        }

        public async Task<IUserMessage> SendEmbed(DiscordChannel channel, EmbedBuilder embedBuilder,
            string message = "")
        {
            var channels = await _discordGuildService.GetChannels();
            var channelId = channels[channel].Id;
            return await SendEmbed(channelId, embedBuilder, message);
        }

        public async Task ModifyEmbed(IUserMessage userMessage, EmbedBuilder embedBuilder) =>
            // заменяем указанное embed-сообщение на новое
            await userMessage.ModifyAsync(x => x.Embed = BuildEmbed(embedBuilder));
    }
}
