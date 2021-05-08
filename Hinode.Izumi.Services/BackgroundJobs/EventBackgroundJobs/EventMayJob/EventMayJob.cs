using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.DiscordJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.BannerService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.EventBackgroundJobs.EventMayJob
{
    [InjectableService]
    public class EventMayJob : IEventMayJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IFoodService _foodService;
        private readonly ILocalizationService _local;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IReputationService _reputationService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IBannerService _bannerService;

        private const string PicnicEmote = "🔥";
        private const string AttackEmote = "⚔️";

        public EventMayJob(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, IDiscordGuildService discordGuildService, IImageService imageService,
            IUserService userService, IInventoryService inventoryService, IFoodService foodService,
            ILocalizationService local, TimeZoneInfo timeZoneInfo, IReputationService reputationService,
            IStatisticService statisticService, IAchievementService achievementService, IBannerService bannerService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _discordGuildService = discordGuildService;
            _imageService = imageService;
            _userService = userService;
            _inventoryService = inventoryService;
            _foodService = foodService;
            _local = local;
            _timeZoneInfo = timeZoneInfo;
            _reputationService = reputationService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _bannerService = bannerService;
        }

        public async Task Start()
        {
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();

            // обновляем текущее событие в базе
            await _propertyService.UpdateProperty(Property.CurrentEvent, (long) Event.May);

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
                        await _propertyService.GetPropertyValue(Property.EventReduceTransitTime)))
                // рассказываем про конец события
                .WithFooter(IzumiEventMessage.EventMayStartFooter.Parse());

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id), embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.YearlyEvents].Id}>");

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
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();

            // обновляем текущее событие в базе
            await _propertyService.UpdateProperty(Property.CurrentEvent, (long) Event.None);

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // подтверждаем конец события
                .WithDescription(IzumiEventMessage.EventMayEndDesc.Parse());

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id), embed);
        }

        public async Task PicnicAnons()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем id канала дневник
            var diaryId = channels[DiscordChannel.Diary].Id;

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о пикнике
                .WithDescription(IzumiEventMessage.EventMayPicnicAnonsDesc.Parse(
                    Location.Village.Localize(true), 30.Minutes().Humanize(1, new CultureInfo("ru-RU")),
                    emotes.GetEmoteOrBlank("Energy")));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(diaryId), embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>");

            // запускаем джобу с появлением пикника через пол часа
            BackgroundJob.Schedule<IEventMayJob>(
                x => x.PicnicSpawn(),
                TimeSpan.FromMinutes(30));
        }

        public async Task PicnicSpawn()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем канал события
            var eventChannel = await _discordGuildService.GetSocketTextChannel(
                channels[DiscordChannel.VillageEvents].Id);
            // получаем блюдо которое нужно выдать за участие в пикнике
            var food = await _foodService.GetFood(
                // получаем id необходимого нам блюда
                await _propertyService.GetPropertyValue(Property.EventMayPicnicFoodId));
            // получаем количество выдаваемого блюда
            var foodAmount = await _propertyService.GetPropertyValue(Property.EventMayPicnicFoodAmount);

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcVillageKio))
                // подверждаем появление пикника и рассказываем как в нем учавствовать
                .WithDescription(IzumiEventMessage.EventMayPicnicSpawnDesc.Parse(
                    PicnicEmote))
                // ожидаемая награда за учавствие в пикнике
                .AddField(IzumiEventMessage.EventMayPicnicSpawnRewardFieldName.Parse(),
                    IzumiEventMessage.EventMayPicnicSpawnRewardFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("Energy"), emotes.GetEmoteOrBlank(food.Name), foodAmount,
                        _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)))
                // изображение пикника
                .WithImageUrl(await _imageService.GetImageUrl(Image.EventMayPicnic))
                // длительность пикника
                .WithFooter(IzumiEventMessage.EventMayPicnicSpawnFooter.Parse(
                    10.Minutes().Humanize(1, new CultureInfo("ru-RU"))));

            // отправляем сообщение
            var message = await eventChannel.SendMessageAsync(
                null, false, _discordEmbedService.BuildEmbed(embed));
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
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем сообщение
            var message = await _discordGuildService.GetIUserMessage(channelId, messageId);
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
            var food = await _foodService.GetFood(
                // получаем id необходимого нам блюда
                await _propertyService.GetPropertyValue(Property.EventMayPicnicFoodId));
            // получаем количество выдаваемого блюда
            var foodAmount = await _propertyService.GetPropertyValue(Property.EventMayPicnicFoodAmount);

            // снимаем реакции с сообщения
            await message.RemoveAllReactionsAsync();
            // полностью восстанавливаем энергию пользователям
            await _userService.AddEnergyToUser(users, 100);
            // выдаем пользователям это блюдо
            await _inventoryService.AddItemToUser(users, InventoryCategory.Food, food.Id,
                // получаем количество которое необходимо выдать
                await _propertyService.GetPropertyValue(Property.EventMayPicnicFoodAmount));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcVillageKio))
                // изображение пикника
                .WithImageUrl(await _imageService.GetImageUrl(Image.EventMayPicnic))
                // подверждаем что пикник закончен и пользователи получили награду
                .WithDescription(IzumiEventMessage.EventMayPicnicEndDesc.Parse(
                    emotes.GetEmoteOrBlank(food.Name), foodAmount,
                    _local.Localize(LocalizationCategory.Food, food.Id, foodAmount)));

            // изменяем сообщение
            await _discordEmbedService.ModifyEmbed(message, embed);
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

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id), embedReward);
        }

        public async Task BossAnons()
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем id канала дневник
            var diaryId = channels[DiscordChannel.Diary].Id;

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о вторжении босса
                .WithDescription(IzumiEventMessage.BossNotify.Parse(
                    Location.Village.Localize(true)));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(diaryId), embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>");

            BackgroundJob.Schedule<IEventMayJob>(
                x => x.BossSpawn(),
                TimeSpan.FromMinutes(
                    // получаем время оповещения о вторжении босса
                    await _propertyService.GetPropertyValue(Property.BossNotifyTime)));
        }

        public async Task BossSpawn()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем количество получаемой репутации за убийство босса
            var reputationReward = await _propertyService.GetPropertyValue(Property.BossReputationReward);

            // получаем баннер который нужно выдать
            var banner = await _bannerService.GetBanner(
                await _propertyService.GetPropertyValue(Property.EventMayBossBannerId));
            // получаем титул который нужно выдать
            var title = (Title) await _propertyService.GetPropertyValue(Property.EventMayBossTitleId);

            // получаем необходимый канал
            var eventChannel =
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.VillageEvents].Id);
            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Npc.Kio.Image()))
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
                .WithImageUrl(await _imageService.GetImageUrl(Image.BossVillage))
                // сколько времени дается на убийство ежедневного босса
                .WithFooter(IzumiEventMessage.BossHereFooter.Parse(
                    // получаем длительность боя с ежедневным боссом
                    await _propertyService.GetPropertyValue(Property.BossKillTime)));

            // отправляем сообщение
            var message = await eventChannel.SendMessageAsync(
                null, false, _discordEmbedService.BuildEmbed(embed));
            // добавляем реакцию для атаки
            await message.AddReactionAsync(new Emoji(AttackEmote));

            // запускаем джобу с убийством босса
            BackgroundJob.Schedule<IEventMayJob>(x => x.BossKill(
                    (long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromMinutes(
                    // получаем длительность боя с ежедневным боссом
                    await _propertyService.GetPropertyValue(Property.BossKillTime)));
        }

        public async Task BossKill(long channelId, long messageId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем сообщение
            var message = await _discordGuildService.GetIUserMessage(channelId, messageId);
            // получаем пользователей нажавших на реакцию
            var reactionUsers = await message
                .GetReactionUsersAsync(new Emoji(AttackEmote), int.MaxValue)
                .FlattenAsync();
            // получаем из всех пользователей только людей (без ботов)
            var users = reactionUsers.Where(x => x.IsBot == false).ToArray();
            // получаем id пользователей
            var usersId = users.Select(x => (long) x.Id).ToArray();
            // получаем получаемую репутацию за убийство ежедневного босса
            var reputationReward = await _propertyService.GetPropertyValue(Property.BossReputationReward);
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем канал дневник
            var diaryChan = await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id);

            // получаем баннер который нужно выдать
            var banner = await _bannerService.GetBanner(
                await _propertyService.GetPropertyValue(Property.EventMayBossBannerId));
            // получаем титул который нужно выдать
            var title = (Title) await _propertyService.GetPropertyValue(Property.EventMayBossTitleId);

            // снимаем все реакции с сообщения
            await message.RemoveAllReactionsAsync();
            // добавляем пользователям коробки
            await _inventoryService.AddItemToUser
                (usersId, InventoryCategory.Box, Box.Village.GetHashCode());
            // добавляем пользователям репутацию
            await _reputationService.AddReputationToUser(usersId, Reputation.Village, reputationReward);
            // добавляем пользователям статистику
            await _statisticService.AddStatisticToUser(usersId, Statistic.BossKilled);
            // проверяем достижения у пользователей
            await _achievementService.CheckAchievement(usersId.ToArray(),
                new[]
                {
                    Achievement.Reach500ReputationVillage,
                    Achievement.Reach1000ReputationVillage,
                    Achievement.Reach2000ReputationVillage,
                    Achievement.Reach5000ReputationVillage,
                    Achievement.Reach10000ReputationVillage
                });
            // добавляем баннер пользователям
            await _bannerService.AddBannerToUser(usersId, banner.Id);
            // добавляем титул пользователям
            await _userService.AddTitleToUser(usersId, title);

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Npc.Kio.Image()))
                // изображение босса
                .WithImageUrl(await _imageService.GetImageUrl(Image.BossVillage))
                // подтверждаем убийтство босса
                .WithDescription(IzumiEventMessage.BossKilled.Parse());

            // изменяем сообщение
            await _discordEmbedService.ModifyEmbed(message, embed);
            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IDiscordJob>(x =>
                    x.DeleteMessage(channelId, messageId),
                TimeSpan.FromHours(24));

            // создаем строку с наградой
            var rewardString =
                IzumiEventMessage.ReputationAdded.Parse(
                    emotes.GetEmoteOrBlank(Reputation.Village.Emote(long.MaxValue)), reputationReward,
                    Location.Village.Localize(true)) +
                $"{emotes.GetEmoteOrBlank(Box.Village.Emote())} {_local.Localize(Box.Village.ToString())}, " +
                $"{banner.Rarity.Localize().ToLower()} «[{banner.Name}]({banner.Url})», титул {emotes.GetEmoteOrBlank(title.Emote())} {title.Localize()}";

            var embedReward = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // описываем полученные награды
                .WithDescription(IzumiEventMessage.BossRewardNotify.Parse(
                    Location.Village.Localize(true), rewardString));

            await _discordEmbedService.SendEmbed(diaryChan, embedReward);
        }
    }
}
