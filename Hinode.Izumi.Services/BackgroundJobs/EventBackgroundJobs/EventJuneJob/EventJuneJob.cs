using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventJuneJob
{
    [InjectableService]
    public class EventJuneJob : IEventJuneJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly TimeZoneInfo _timeZoneInfo;
        private const string ReactionEmote = "🎉";

        public EventJuneJob(IMediator mediator, ILocalizationService local, TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _local = local;
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task Start()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.June));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                .WithDescription(IzumiEventMessage.EventJuneStartDesc.Parse(
                    emotes.GetEmoteOrBlank("Bamboo"), emotes.GetEmoteOrBlank("BambooElephant"),
                    emotes.GetEmoteOrBlank("BambooDragon"), emotes.GetEmoteOrBlank("BambooMiniPig"),
                    emotes.GetEmoteOrBlank("BambooWhale"), emotes.GetEmoteOrBlank("BambooPanda")));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.YearlyEvents].Id}>"));

            RecurringJob.AddOrUpdate<IEventJuneJob>(
                x => x.SkyLanternAnons(),
                // в 19:30, с 1 по 6 июня включительно
                "30 19 1-6 6 *", _timeZoneInfo);
        }

        public async Task End()
        {
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.None));

            var embed = new EmbedBuilder()
                .WithDescription(IzumiEventMessage.EventJuneEndDesc.Parse());

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed));

            BackgroundJob.Schedule<IEventJuneJob>(
                x => x.RemoveEventGatheringFromUsers(),
                TimeSpan.FromDays(30));
        }

        public async Task RemoveEventGatheringFromUsers() =>
            await _mediator.Send(new RemoveEventGatheringFromAllUsersCommand(Event.June));

        public async Task SkyLanternAnons()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            var food = await _mediator.Send(new GetFoodQuery(
                await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodId))));
            var foodAmount = await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodAmount));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                .WithDescription(IzumiEventMessage.EventJuneSkyLanternAnons.Parse(
                    Location.Garden.Localize(true), 30.Minutes().Humanize(1, new CultureInfo("ru-RU")),
                    emotes.GetEmoteOrBlank("Energy"), Npc.Nari.Name(), emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));

            BackgroundJob.Schedule<IEventJuneJob>(
                x => x.SkyLanternBegin(),
                TimeSpan.FromMinutes(30));
        }

        public async Task SkyLanternBegin()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            var food = await _mediator.Send(new GetFoodQuery(
                await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodId))));
            var foodAmount = await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodAmount));

            var embed = new EmbedBuilder()
                .WithAuthor(Npc.Nari.Name())
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Npc.Nari.Image())))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.EventJuneSkyLantern)))
                .WithDescription(IzumiEventMessage.EventJuneSkyLanternBeginDesc.Parse(
                    ReactionEmote))
                .AddField(IzumiEventMessage.EventJuneSkyLanternBeginRewardFieldName.Parse(),
                    IzumiEventMessage.EventJuneSkyLanternBeginRewardFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("Energy"), emotes.GetEmoteOrBlank(food.Name), foodAmount,
                        _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)))
                .WithFooter(IzumiEventMessage.EventJuneSkyLanternBeginFooter.Parse(
                    10.Minutes().Humanize(1, new CultureInfo("ru-RU"))));

            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.GardenEvents, embed,
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));
            await message.AddReactionAsync(new Emoji(ReactionEmote));

            BackgroundJob.Schedule<IEventJuneJob>(
                x => x.SkyLanternEnd((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromMinutes(10));
        }

        public async Task SkyLanternEnd(long channelId, long messageId)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var message = await _mediator.Send(new GetDiscordUserMessageQuery(channelId, messageId));
            var reactionUsers = await message
                .GetReactionUsersAsync(new Emoji(ReactionEmote), int.MaxValue)
                .FlattenAsync();
            var users = reactionUsers
                .Where(x => x.IsBot == false)
                .Select(x => (long) x.Id)
                .ToArray();
            var food = await _mediator.Send(new GetFoodQuery(
                await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodId))));
            var foodAmount = await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneSkyLanternFoodAmount));

            await message.RemoveAllReactionsAsync();
            await _mediator.Send(new AddEnergyToUsersCommand(users, 100));
            await _mediator.Send(new AddItemToUsersByInventoryCategoryCommand(
                users, InventoryCategory.Food, food.Id, foodAmount));

            var embed = new EmbedBuilder()
                .WithAuthor(Npc.Nari.Name())
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Npc.Nari.Image())))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.EventJuneSkyLantern)))
                .WithDescription(IzumiEventMessage.EventJuneSkyLanternEndDesc.Parse(
                    emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            await _mediator.Send(new ModifyEmbedCommand(message, embed));

            BackgroundJob.Schedule<IDiscordJob>(
                x => x.DeleteMessage(channelId, messageId),
                TimeSpan.FromHours(24));

            var embedReward = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                .WithDescription(IzumiEventMessage.EventJuneSkyLanternEndRewardDesc.Parse(
                    Location.Garden.Localize(true), emotes.GetEmoteOrBlank("Energy"),
                    emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embedReward));
        }
    }
}
