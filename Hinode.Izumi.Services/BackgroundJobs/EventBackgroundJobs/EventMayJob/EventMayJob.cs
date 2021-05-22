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
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Commands;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Commands;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventMayJob
{
    [InjectableService]
    public class EventMayJob : IEventMayJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly TimeZoneInfo _timeZoneInfo;
        private const string PicnicEmote = "🔥";
        private const string AttackEmote = "⚔️";

        public EventMayJob(IMediator mediator, ILocalizationService local, TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _local = local;
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task Start()
        {
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            // получаем каналы сервера
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());

            // обновляем текущее событие в базе
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.May));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // описание события
                .WithDescription(
                    IzumiEventMessage.EventMayStartDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // рассказываем про пикник
                .AddField(IzumiEventMessage.EventMayStartPicnicFieldName.Parse(),
                    IzumiEventMessage.EventMayStartPicnicFieldDesc.Parse(
                        Npc.Kio.Name(), channels[DiscordChannel.Diary].Id))
                // рассказываем про ускорение перемещения
                .AddField(IzumiEventMessage.EventTimeReduceTransitFieldName.Parse(),
                    IzumiEventMessage.EventTimeReduceTransitFieldDesc.Parse(
                        await _mediator.Send(new GetPropertyValueQuery(Property.EventReduceTransitTime))))
                // рассказываем про конец события
                .WithFooter(IzumiEventMessage.EventMayStartFooter.Parse());

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.YearlyEvents].Id}>"));

            // запускаем джобу появления пикника
            RecurringJob.AddOrUpdate<IEventMayJob>(
                x => x.PicnicAnons(),
                // в 19:30, с 1 по 8 мая включительно
                "30 19 1-8 5 *", _timeZoneInfo);

            // запускаем джобу появления босса события
            RecurringJob.AddOrUpdate<IEventMayJob>(
                x => x.BossAnons(),
                // в 19:30, 9 мая
                "30 19 9 5 *", _timeZoneInfo);
        }

        public async Task End()
        {
            // обновляем текущее событие в базе
            await _mediator.Send(new UpdatePropertyCommand(Property.CurrentEvent, (long) Event.None));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // подтверждаем конец события
                .WithDescription(IzumiEventMessage.EventMayEndDesc.Parse());

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed));
        }

        public async Task PicnicAnons()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о пикнике
                .WithDescription(IzumiEventMessage.EventMayPicnicAnonsDesc.Parse(
                    Location.Village.Localize(true), 30.Minutes().Humanize(1, new CultureInfo("ru-RU")),
                    emotes.GetEmoteOrBlank("Energy")));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));

            // запускаем джобу с появлением пикника через пол часа
            BackgroundJob.Schedule<IEventMayJob>(
                x => x.PicnicSpawn(),
                TimeSpan.FromMinutes(30));
        }

        public async Task PicnicSpawn()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            // получаем блюдо которое нужно выдать за участие в пикнике
            var food = await _mediator.Send(new GetFoodQuery(
                // получаем id необходимого нам блюда
                await _mediator.Send(new GetPropertyValueQuery(Property.EventMayPicnicFoodId))));
            // получаем количество выдаваемого блюда
            var foodAmount = await _mediator.Send(new GetPropertyValueQuery(Property.EventMayPicnicFoodAmount));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.NpcVillageKio)))
                // подверждаем появление пикника и рассказываем как в нем учавствовать
                .WithDescription(IzumiEventMessage.EventMayPicnicSpawnDesc.Parse(
                    PicnicEmote))
                // ожидаемая награда за учавствие в пикнике
                .AddField(IzumiEventMessage.EventMayPicnicSpawnRewardFieldName.Parse(),
                    IzumiEventMessage.EventMayPicnicSpawnRewardFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("Energy"), emotes.GetEmoteOrBlank(food.Name), foodAmount,
                        _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)))
                // изображение пикника
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.EventMayPicnic)))
                // длительность пикника
                .WithFooter(IzumiEventMessage.EventMayPicnicSpawnFooter.Parse(
                    10.Minutes().Humanize(1, new CultureInfo("ru-RU"))));

            // отправляем сообщение
            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.VillageEvents, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));
            // добавляем реакцию для участия в пикнике
            await message.AddReactionAsync(new Emoji(PicnicEmote));

            // запускаем джобу окончания пикника
            BackgroundJob.Schedule<IEventMayJob>(
                x => x.PicnicEnd((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromMinutes(10));
        }

        public async Task PicnicEnd(long channelId, long messageId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем сообщение
            var message = await _mediator.Send(new GetDiscordUserMessageQuery(channelId, messageId));
            // получаем пользователей нажавших на реакцию
            var reactionUsers = await message
                .GetReactionUsersAsync(new Emoji(PicnicEmote), int.MaxValue)
                .FlattenAsync();
            // получаем из всех пользователей только людей (без ботов)
            var users = reactionUsers
                .Where(x => x.IsBot == false)
                .Select(x => (long) x.Id)
                .ToArray();
            // получаем блюдо которое нужно выдать за участие в пикнике
            var food = await _mediator.Send(new GetFoodQuery(
                // получаем id необходимого нам блюда
                await _mediator.Send(new GetPropertyValueQuery(Property.EventMayPicnicFoodId))));
            // получаем количество выдаваемого блюда
            var foodAmount = await _mediator.Send(new GetPropertyValueQuery(Property.EventMayPicnicFoodAmount));

            // снимаем реакции с сообщения
            await message.RemoveAllReactionsAsync();
            // полностью восстанавливаем энергию пользователям
            await _mediator.Send(new AddEnergyToUsersCommand(users, 100));
            // выдаем пользователям это блюдо
            await _mediator.Send(new AddItemToUsersByInventoryCategoryCommand(users, InventoryCategory.Food, food.Id,
                // получаем количество которое необходимо выдать
                await _mediator.Send(new GetPropertyValueQuery(Property.EventMayPicnicFoodAmount))));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.NpcVillageKio)))
                // изображение пикника
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.EventMayPicnic)))
                // подверждаем что пикник закончен и пользователи получили награду
                .WithDescription(IzumiEventMessage.EventMayPicnicEndDesc.Parse(
                    emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            // изменяем сообщение
            await _mediator.Send(new ModifyEmbedCommand(message, embed));
            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage(channelId, messageId),
                TimeSpan.FromHours(24));

            var embedReward = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о том, что пользователи получили награду за пикник
                .WithDescription(IzumiEventMessage.EventMayPicnicEndRewardDesc.Parse(
                    Location.Village.Localize(true), emotes.GetEmoteOrBlank("Energy"),
                    emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embedReward));
        }

        public async Task BossAnons()
        {
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о вторжении босса
                .WithDescription(IzumiEventMessage.BossNotify.Parse(
                    Location.Village.Localize(true)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));

            BackgroundJob.Schedule<IEventMayJob>(
                x => x.BossSpawn(),
                TimeSpan.FromMinutes(
                    // получаем время оповещения о вторжении босса
                    await _mediator.Send(new GetPropertyValueQuery(Property.BossNotifyTime))));
        }

        public async Task BossSpawn()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем роли сервера
            var roles = await _mediator.Send(new GetDiscordRolesQuery());
            // получаем количество получаемой репутации за убийство босса
            var reputationReward = await _mediator.Send(new GetPropertyValueQuery(Property.BossReputationReward));

            // получаем баннер который нужно выдать
            var banner = await _mediator.Send(new GetBannerQuery(
                await _mediator.Send(new GetPropertyValueQuery(Property.EventMayBossBannerId))));
            // получаем титул который нужно выдать
            var title = (Title) await _mediator.Send(new GetPropertyValueQuery(Property.EventMayBossTitleId));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Npc.Kio.Image())))
                // описание вторжения ежедневного босса
                .WithDescription(
                    IzumiEventMessage.BossHere.Parse(
                        Location.Village.Localize(), AttackEmote) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемая награда
                .AddField(IzumiEventMessage.BossRewardFieldName.Parse(),
                    IzumiEventMessage.BossRewardReputation.Parse(
                        emotes.GetEmoteOrBlank(Reputation.Village.Emote(long.MaxValue)), reputationReward,
                        Location.Village.Localize(true)) +
                    $"{emotes.GetEmoteOrBlank(Box.Village.Emote())} {_local.Localize(Box.Village.ToString())}\n" +
                    $"{banner.Rarity.Localize().ToLower()} «[{banner.Name}]({banner.Url})»\n " +
                    $"титул {emotes.GetEmoteOrBlank(title.Emote())} {title.Localize()}")
                // изображение босса
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.BossVillage)))
                // сколько времени дается на убийство ежедневного босса
                .WithFooter(IzumiEventMessage.BossHereFooter.Parse(
                    // получаем длительность боя с ежедневным боссом
                    await _mediator.Send(new GetPropertyValueQuery(Property.BossKillTime))));

            // отправляем сообщение
            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.VillageEvents, embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>"));
            // добавляем реакцию для атаки
            await message.AddReactionAsync(new Emoji(AttackEmote));

            // запускаем джобу с убийством босса
            BackgroundJob.Schedule<IEventMayJob>(x => x.BossKill(
                    (long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromMinutes(
                    // получаем длительность боя с ежедневным боссом
                    await _mediator.Send(new GetPropertyValueQuery(Property.BossKillTime))));
        }

        public async Task BossKill(long channelId, long messageId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем сообщение
            var message = await _mediator.Send(new GetDiscordUserMessageQuery(channelId, messageId));
            // получаем пользователей нажавших на реакцию
            var reactionUsers = await message
                .GetReactionUsersAsync(new Emoji(AttackEmote), int.MaxValue)
                .FlattenAsync();
            // получаем из всех пользователей только людей (без ботов)
            var users = reactionUsers.Where(x => x.IsBot == false).ToArray();
            // получаем id пользователей
            var usersId = users.Select(x => (long) x.Id).ToArray();
            // получаем получаемую репутацию за убийство ежедневного босса
            var reputationReward = await _mediator.Send(new GetPropertyValueQuery(Property.BossReputationReward));

            // получаем баннер который нужно выдать
            var banner = await _mediator.Send(new GetBannerQuery(
                await _mediator.Send(new GetPropertyValueQuery(Property.EventMayBossBannerId))));
            // получаем титул который нужно выдать
            var title = (Title) await _mediator.Send(new GetPropertyValueQuery(Property.EventMayBossTitleId));

            // снимаем все реакции с сообщения
            await message.RemoveAllReactionsAsync();
            // добавляем пользователям коробки
            await _mediator.Send(new AddItemToUsersByInventoryCategoryCommand(
                usersId, InventoryCategory.Box, Box.Village.GetHashCode()));
            // добавляем пользователям репутацию
            await _mediator.Send(new AddReputationToUsersCommand(usersId, Reputation.Village, reputationReward));
            // добавляем пользователям статистику
            await _mediator.Send(new AddStatisticToUsersCommand(usersId, Statistic.BossKilled));
            // добавляем баннер пользователям
            await _mediator.Send(new AddBannerToUsersCommand(usersId, banner.Id));
            // добавляем титул пользователям
            await _mediator.Send(new AddTitleToUsersCommand(usersId, title));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Npc.Kio.Image())))
                // изображение босса
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.BossVillage)))
                // подтверждаем убийтство босса
                .WithDescription(IzumiEventMessage.BossKilled.Parse());

            // изменяем сообщение
            await _mediator.Send(new ModifyEmbedCommand(message, embed));
            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage(channelId, messageId),
                TimeSpan.FromHours(24));

            var embedReward = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // описываем полученные награды
                .WithDescription(IzumiEventMessage.BossRewardNotify.Parse(
                    Location.Village.Localize(true),
                    IzumiEventMessage.ReputationAdded.Parse(
                        emotes.GetEmoteOrBlank(
                            Reputation.Village.Emote(long.MaxValue)), reputationReward,
                        Location.Village.Localize(true)) +
                    $"{emotes.GetEmoteOrBlank(Box.Village.Emote())} {_local.Localize(Box.Village.ToString())}, " +
                    $"{banner.Rarity.Localize().ToLower()} «[{banner.Name}]({banner.Url})», титул {emotes.GetEmoteOrBlank(title.Emote())} {title.Localize()}"));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embedReward));
        }
    }
}
