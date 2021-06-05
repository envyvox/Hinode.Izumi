using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceived
{
    [InjectableService]
    public class MessageReceived : IMessageReceived
    {
        private readonly IMediator _mediator;

        public MessageReceived(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(DiscordSocketClient socketClient, SocketMessage socketMessage)
        {
            // игнорируем сообщениния от ботов
            if (socketMessage.Author.IsBot) return;

            // получаем каналы сервера
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            // получаем из них каналы доски сообщества
            var communityDescChannels = await _mediator.Send(new GetCommunityDescChannelsQuery());

            // если сообщение находится в канале доски сообщества
            if (communityDescChannels.Contains(socketMessage.Channel.Id))
            {
                // проверяем его на наличие вложений
                var hasAttachment = CheckAttachment(socketMessage);
                // если они есть - отправляем сообщение в сервис доски сообщества
                if (hasAttachment)
                {
                    // добавляем реакции голосования
                    await AddVotes((IUserMessage) socketMessage);
                    // добавляем сообщение в базу
                    await _mediator.Send(new CreateContentMessageCommand(
                        (long) socketMessage.Author.Id, (long) socketMessage.Channel.Id, (long) socketMessage.Id));
                }

                // если нет - удаляем сообщение
                else await DeleteMessage(socketMessage);
            }

            // если сообщение находится в канале предложения
            if (socketMessage.Channel.Id == (ulong) channels[DiscordChannel.Suggestions].Id)
                // добавляем реакции голосования
                await AddVotes((IUserMessage) socketMessage);

            // если это сообщение в общении
            if (socketMessage.Channel.Id == (ulong) channels[DiscordChannel.Chat].Id)
            {
                // проверяем зарегистрирован ли пользователь в игровом мире
                var checkUser = await _mediator.Send(new CheckUserByIdQuery((long) socketMessage.Author.Id));

                if (checkUser)
                {
                    // добавляем статистику пользователю
                    await _mediator.Send(new AddStatisticToUserCommand(
                        (long) socketMessage.Author.Id, Statistic.Messages));
                    // проверяем выполнил ли пользователь достижение
                    await _mediator.Send(new CheckAchievementInUserCommand(
                        (long) socketMessage.Author.Id, Achievement.FirstMessage));
                }
            }
        }

        private static async Task DeleteMessage(IDeletable message)
        {
            // задерживаем таск для того чтобы дискорд успел обработать сообщение
            await Task.Delay(1000);
            // удаляем сообщение
            await message.DeleteAsync();
        }

        private async Task AddVotes(IUserMessage message)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // добавляем реакции голосования
            await message.AddReactionsAsync(new IEmote[]
            {
                Emote.Parse(emotes.GetEmoteOrBlank("Like")),
                Emote.Parse(emotes.GetEmoteOrBlank("Dislike")),
            });
        }

        private static bool CheckAttachment(SocketMessage message) =>
            // убеждаемся что количество вложений равно 1
            message.Attachments.Count == 1 ||
            // или что в сообщении есть ссылка
            message.Content.Contains("http");
    }
}
