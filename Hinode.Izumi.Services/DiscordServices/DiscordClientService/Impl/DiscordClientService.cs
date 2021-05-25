using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hangfire;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.BossJob;
using Hinode.Izumi.Services.BackgroundJobs.CasinoJob;
using Hinode.Izumi.Services.BackgroundJobs.CurrencyJob;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.BackgroundJobs.EmoteJob;
using Hinode.Izumi.Services.BackgroundJobs.EnergyJob;
using Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventJuneJob;
using Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventMayJob;
using Hinode.Izumi.Services.BackgroundJobs.MarketJob;
using Hinode.Izumi.Services.BackgroundJobs.NewDayJob;
using Hinode.Izumi.Services.BackgroundJobs.PointsJob;
using Hinode.Izumi.Services.BackgroundJobs.SeasonJob;
using Hinode.Izumi.Services.BackgroundJobs.ShopJob;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.GuildMemberUpdated;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageDeleted;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceived;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionAdded;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionRemoved;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserJoined;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserLeft;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserVoiceStateUpdated;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.Impl
{
    [InjectableService(IsSingletone = true)]
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordOptions> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly CommandService _commands;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<DiscordClientService> _logger;
        private readonly IMediator _mediator;

        private DiscordSocketClient _socketClient;

        public DiscordClientService(IOptions<DiscordOptions> options, IServiceProvider serviceProvider,
            TimeZoneInfo timeZoneInfo, CommandService commands, IHostApplicationLifetime lifetime,
            ILogger<DiscordClientService> logger, IMediator mediator)
        {
            _options = options;
            _serviceProvider = serviceProvider;
            _timeZoneInfo = timeZoneInfo;
            _commands = commands;
            _lifetime = lifetime;
            _logger = logger;
            _mediator = mediator;
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMembers |
                    GatewayIntents.GuildMessageReactions |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.GuildVoiceStates |
                    GatewayIntents.DirectMessages
            });
        }

        public async Task Start()
        {
            await _commands.AddModulesAsync(
                Assembly.Load("Hinode.Izumi.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"),
                _serviceProvider);
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.BotToken);
            await _socketClient.StartAsync();

            _commands.CommandExecuted += CommandExecutedAsync;
            _socketClient.Log += Log;
            _socketClient.Ready += SocketClientOnReady;
            _socketClient.MessageReceived += SocketClientOnMessageReceived;
            _socketClient.MessageDeleted += SocketClientOnMessageDeleted;
            _socketClient.UserLeft += SocketClientOnUserLeft;
            _socketClient.UserJoined += SocketClientOnUserJoined;
            _socketClient.GuildMemberUpdated += SocketClientOnGuildMemberUpdated;
            _socketClient.UserVoiceStateUpdated += SocketClientOnUserVoiceStateUpdated;
            _socketClient.ReactionAdded += SocketClientOnReactionAdded;
            _socketClient.ReactionRemoved += SocketClientOnReactionRemoved;
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context,
            IResult result)
        {
            // если при выполнении команды произошла ошибка, нужно отправить ее пользователю
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.CommandError)))
                    .WithAuthor(IzumiErrorReasonMessage.SomethingGoneWrong.Localize())
                    .WithDescription(
                        // определяем текст ошибки в зависимости от ее типа
                        result.Error switch
                        {
                            // эти ошибки прозошли со стороны пользователя, значит выводим локализированный ответ
                            CommandError.UnknownCommand => IzumiErrorReasonMessage.UnknownCommand.Localize(),
                            CommandError.ParseFailed => IzumiErrorReasonMessage.ParseFailed.Localize(),
                            CommandError.BadArgCount => IzumiErrorReasonMessage.BadArgCount.Localize(),
                            CommandError.ObjectNotFound => IzumiErrorReasonMessage.ObjectNotFound.Localize(),
                            CommandError.MultipleMatches => IzumiErrorReasonMessage.MultipleMatches.Localize(),
                            // эти ошибки произошли с нашей стороны, значит просто выводим текст ошибки
                            CommandError.UnmetPrecondition => result.ErrorReason,
                            CommandError.Exception => result.ErrorReason,
                            CommandError.Unsuccessful => result.ErrorReason,
                            null => result.ErrorReason,
                            _ => result.ErrorReason
                        });
                await context.User.SendMessageAsync("", false, embed.Build());
            }

            await Task.CompletedTask;
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task SocketClientOnReady()
        {
            // устанавливаем статус бота в дискорде
            await _socketClient.SetGameAsync("аниме", null, ActivityType.Watching);

            try
            {
                // джобы с определенным кроном
                RecurringJob.AddOrUpdate<IEmoteJob>(
                    x => x.UploadEmotes(),
                    "0/5 * * * *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<ICasinoJob>(
                    x => x.Open(),
                    "0 18 * * *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<ICasinoJob>(
                    x => x.Close(),
                    "0 6 * * *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<IBossJob>(
                    x => x.Anons(),
                    "30 19 * * *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<IEnergyJob>(
                    x => x.HourlyRecovery(),
                    Cron.Hourly, _timeZoneInfo);

                // смена сезонов
                RecurringJob.AddOrUpdate<ISeasonJob>(
                    x => x.SpringComing(),
                    // 22 февраля
                    "0 0 22 2 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<ISeasonJob>(
                    x => x.SummerComing(),
                    // 25 мая
                    "0 0 25 5 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<ISeasonJob>(
                    x => x.AutumnComing(),
                    // 25 августа
                    "0 0 22 8 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<ISeasonJob>(
                    x => x.WinterComing(),
                    // 24 ноября
                    "0 0 24 11 *", _timeZoneInfo);

                // события
                RecurringJob.AddOrUpdate<IEventMayJob>(
                    x => x.Start(),
                    // в 18:00, 1 мая
                    "0 18 1 5 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<IEventMayJob>(
                    x => x.End(),
                    // в 00:00, 10 мая
                    "0 0 10 5 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<IEventJuneJob>(
                    x => x.Start(),
                    // в 00:00, 1 июня
                    "0 0 1 6 *", _timeZoneInfo);
                RecurringJob.AddOrUpdate<IEventJuneJob>(
                    x => x.End(),
                    // в 00:00, 8 июня
                    "0 0 8 6 *", _timeZoneInfo);

                // ежедневные джобы
                RecurringJob.AddOrUpdate<INewDayJob>(
                    x => x.StartNewDay(),
                    Cron.Daily, _timeZoneInfo);
                RecurringJob.AddOrUpdate<IMarketJob>(
                    x => x.DailyMarketReset(),
                    Cron.Daily, _timeZoneInfo);
                RecurringJob.AddOrUpdate<IShopJob>(
                    x => x.UpdateBannersInDynamicShop(),
                    Cron.Daily, _timeZoneInfo);
                RecurringJob.AddOrUpdate<ICurrencyJob>(
                    x => x.DailyIncome(),
                    Cron.Daily, _timeZoneInfo);
                RecurringJob.AddOrUpdate<IDiscordJob>(
                    x => x.RemoveExpiredRoleFromUsers(),
                    Cron.Daily, _timeZoneInfo);

                // ежемесячные джобы
                RecurringJob.AddOrUpdate<IPointsJob>(
                    x => x.ResetAdventurePoints(),
                    Cron.Monthly, _timeZoneInfo);

                _logger.LogInformation("Bot started");
                RecurringJob.Trigger("IEmoteJob.UploadEmotes");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unable to startup the bot. Application will now exit");
                _lifetime.StopApplication();
            }
        }

        private async Task SocketClientOnMessageReceived(SocketMessage socketMessage)
        {
            // если сообщение пришло не от пользователя - игнорируем
            if (socketMessage is not SocketUserMessage message) return;

            // отправляем сообщение в сервис сообщений для дополнительных проверок
            await _serviceProvider.GetService<IMessageReceived>()
                .Execute(_socketClient, socketMessage);

            // если сообщение не содержит префикса (не является командой) - игнорируем
            var argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_socketClient.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // если команда пришла не в личных сообщениях - игнорируем
            if (socketMessage.Channel.GetType() != typeof(SocketDMChannel)) return;

            // создаем контекст команды
            var context = new SocketCommandContext(_socketClient, message);

            // выполняем команду
            await _commands.ExecuteAsync(context, argPos, _serviceProvider);
        }

        private async Task SocketClientOnMessageDeleted(Cacheable<IMessage, ulong> message,
            ISocketMessageChannel channel) =>
            await _serviceProvider.GetService<IMessageDeleted>()
                .Execute(message, channel);

        private async Task SocketClientOnUserLeft(SocketGuildUser socketGuildUser) =>
            await _serviceProvider.GetService<IUserLeft>()
                .Execute(socketGuildUser);

        private async Task SocketClientOnUserJoined(SocketGuildUser socketGuildUser) =>
            await _serviceProvider.GetService<IUserJoined>()
                .Execute(socketGuildUser);

        private async Task SocketClientOnGuildMemberUpdated(SocketGuildUser oldSocketGuildUser,
            SocketGuildUser newSocketGuildUser) =>
            await _serviceProvider.GetService<IGuildMemberUpdated>()
                .Execute(_socketClient, oldSocketGuildUser, newSocketGuildUser);

        private async Task SocketClientOnUserVoiceStateUpdated(SocketUser socketUser,
            SocketVoiceState oldSocketVoiceState, SocketVoiceState newSocketVoiceState) =>
            await _serviceProvider.GetService<IUserVoiceStateUpdated>()
                .Execute(socketUser, oldSocketVoiceState, newSocketVoiceState);

        private async Task SocketClientOnReactionAdded(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction) =>
            await _serviceProvider.GetService<IReactionAdded>()
                .Execute(message, socketMessageChannel, socketReaction);

        private async Task SocketClientOnReactionRemoved(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction) =>
            await _serviceProvider.GetService<IReactionRemoved>()
                .Execute(message, socketMessageChannel, socketReaction);

        public async Task<DiscordSocketClient> GetSocketClient() => await Task.FromResult(_socketClient);
    }
}
