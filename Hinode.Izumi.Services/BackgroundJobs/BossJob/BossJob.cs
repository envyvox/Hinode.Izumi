using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MessageJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Box = Hinode.Izumi.Data.Enums.Box;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.BossJob
{
    [InjectableService]
    public class BossJob : IBossJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IInventoryService _inventoryService;
        private readonly IFieldService _fieldService;
        private readonly IReputationService _reputationService;
        private readonly IImageService _imageService;

        private readonly Random _random = new();

        private readonly Location[] _spawnLocations =
            {Location.Capital, Location.Garden, Location.Seaport, Location.Castle, Location.Village};

        private const string AttackEmote = "⚔️";

        public BossJob(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IEmoteService emoteService, IPropertyService propertyService, IStatisticService statisticService,
            IAchievementService achievementService, IInventoryService inventoryService, IFieldService fieldService,
            IReputationService reputationService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _inventoryService = inventoryService;
            _fieldService = fieldService;
            _reputationService = reputationService;
            _imageService = imageService;
        }

        public async Task Anons()
        {
            // получаем текущее событие
            var currentEvent = (Event) await _propertyService.GetPropertyValue(Property.CurrentEvent);
            // если текущее событие это майское событие - то босс не должен появляться
            if (currentEvent == Event.May) return;

            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем id канала дневник
            var diaryId = channels[DiscordChannel.Diary].Id;
            // получаем случайную локацию
            // ReSharper disable once PossibleNullReferenceException
            var randomLocation = (Location) _spawnLocations.GetValue(_random.Next(_spawnLocations.Length));

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о вторжении босса
                .WithDescription(IzumiEventMessage.BossNotify.Parse(
                    randomLocation.Localize(true)));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(diaryId), embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>");

            BackgroundJob.Schedule<IBossJob>(
                x => x.Spawn(randomLocation),
                TimeSpan.FromMinutes(
                    // получаем время оповещения о вторжении босса
                    await _propertyService.GetPropertyValue(Property.BossNotifyTime)));
        }


        public async Task Spawn(Location location)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем репутацию этой локации
            var reputation = _reputationService.GetReputationByLocation(location);
            // получаем количество получаемой репутации за убийство босса
            var reputationReward = await _propertyService.GetPropertyValue(Property.BossReputationReward);

            // заполняем канал
            DiscordChannel channel;
            // заполняем нпс
            Npc npc;
            // заполняем изображение босса
            Image bossImage;
            // заполняем коробку
            Box box;

            // заполняем данные в зависимости от репутации
            switch (reputation)
            {
                case Reputation.Capital:

                    channel = DiscordChannel.CapitalEvents;
                    npc = Npc.Toredo;
                    bossImage = Image.BossCapital;
                    box = Box.CapitalBossReward;

                    break;
                case Reputation.Garden:

                    channel = DiscordChannel.GardenEvents;
                    npc = Npc.Nari;
                    bossImage = Image.BossGarden;
                    box = Box.GardenBossReward;

                    break;
                case Reputation.Seaport:

                    channel = DiscordChannel.SeaportEvents;
                    npc = Npc.Ivao;
                    bossImage = Image.BossSeaport;
                    box = Box.SeaportBossReward;

                    break;
                case Reputation.Castle:

                    channel = DiscordChannel.CastleEvents;
                    npc = Npc.Ioshiro;
                    bossImage = Image.BossCastle;
                    box = Box.CastleBossReward;

                    break;
                case Reputation.Village:

                    channel = DiscordChannel.VillageEvents;
                    npc = Npc.Kio;
                    bossImage = Image.BossVillage;
                    box = Box.VillageBossReward;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // получаем необходимый канал
            var eventChannel = await _discordGuildService.GetSocketTextChannel(channels[channel].Id);
            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(npc.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(npc.Image()))
                // описание вторжения ежедневного босса
                .WithDescription(
                    IzumiEventMessage.BossHere.Parse(
                        location.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемая награда
                .AddField(IzumiEventMessage.BossRewardFieldName.Parse(),
                    IzumiEventMessage.BossRewardReputation.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(long.MaxValue)), reputationReward,
                        location.Localize(true)) +
                    $"{emotes.GetEmoteOrBlank(box.Emote())} {box.Localize()}")
                // изображение босса
                .WithImageUrl(await _imageService.GetImageUrl(bossImage))
                // сколько времени дается на убийство ежедневного босса
                .WithFooter(IzumiEventMessage.BossHereFooter.Parse(
                    // получаем длительность боя с ежедневным боссом
                    await _propertyService.GetPropertyValue(Property.BossKillTime)));

            // отправляем сообщение
            var message = await eventChannel.SendMessageAsync(
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>",
                false, _discordEmbedService.BuildEmbed(embed));
            // добавляем реакцию для атаки
            await message.AddReactionAsync(new Emoji(AttackEmote));

            // запускаем джобу с убийством босса
            BackgroundJob.Schedule<IBossJob>(x => x.Kill(
                    (long) message.Channel.Id, (long) message.Id, reputation, npc, bossImage, box),
                TimeSpan.FromMinutes(
                    // получаем длительность боя с ежедневным боссом
                    await _propertyService.GetPropertyValue(Property.BossKillTime)));
        }


        public async Task Kill(long channelId, long messageId, Reputation reputation, Npc npc, Image bossImage, Box box)
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
            // получаем необходимое количество пользователей для убийства ежедневного босса
            var requiredUsersLength = await _propertyService.GetPropertyValue(Property.BossRequiredUsers);

            // снимаем все реакции с сообщения
            await message.RemoveAllReactionsAsync();

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(npc.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(npc.Image()))
                // изображение босса
                .WithImageUrl(await _imageService.GetImageUrl(bossImage));

            if (users.Length < requiredUsersLength)
            {
                // получаем дебафф от втождения ежедневного босса
                var debuff = reputation switch
                {
                    // определяем дебафф
                    Reputation.Capital => BossDebuff.CapitalStop,
                    Reputation.Garden => BossDebuff.GardenStop,
                    Reputation.Seaport => BossDebuff.SeaportStop,
                    Reputation.Castle => BossDebuff.CastleStop,
                    Reputation.Village => BossDebuff.VillageStop,
                    _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                };

                if (reputation == Reputation.Village) await _fieldService.UpdateState(FieldState.Planted);

                embed.WithDescription(IzumiEventMessage.BossNotKilled.Parse(
                    debuff.Localize()));

                // обновляем свойство мира на новый дебаф
                await _propertyService.UpdateProperty(Property.BossDebuff, debuff.GetHashCode());

                // добавляем джобу для сброса дебафа
                BackgroundJob.Schedule<IBossJob>(x => x.ResetDebuff(),
                    TimeSpan.FromHours(
                        // получаем длительность эффекта дебаффа от вторжения ежедневного босса
                        await _propertyService.GetPropertyValue(Property.BossDebuffExpiration)));
            }
            else
            {
                // получаем id пользователей
                var usersId = users.Select(x => (long) x.Id).ToArray();
                // получаем получаемую репутацию за убийство ежедневного босса
                var reputationReward = await _propertyService.GetPropertyValue(Property.BossReputationReward);

                // добавляем пользователям коробки
                await _inventoryService.AddItemToUser(usersId, InventoryCategory.Box, box.GetHashCode());
                // добавляем пользователям репутацию
                await _reputationService.AddReputationToUser(usersId, reputation, reputationReward);
                // добавляем пользователям статистику
                await _statisticService.AddStatisticToUser(usersId, Statistic.BossKilled);

                // проверяем достижения у пользователей
                switch (reputation)
                {
                    case Reputation.Capital:

                        await _achievementService.CheckAchievement(usersId.ToArray(),
                            new[]
                            {
                                Achievement.Reach500ReputationCapital,
                                Achievement.Reach1000ReputationCapital,
                                Achievement.Reach2000ReputationCapital,
                                Achievement.Reach5000ReputationCapital,
                                Achievement.Reach10000ReputationCapital
                            });

                        break;
                    case Reputation.Garden:

                        await _achievementService.CheckAchievement(usersId.ToArray(),
                            new[]
                            {
                                Achievement.Reach500ReputationGarden,
                                Achievement.Reach1000ReputationGarden,
                                Achievement.Reach2000ReputationGarden,
                                Achievement.Reach5000ReputationGarden,
                                Achievement.Reach10000ReputationGarden
                            });

                        break;
                    case Reputation.Seaport:

                        await _achievementService.CheckAchievement(usersId.ToArray(),
                            new[]
                            {
                                Achievement.Reach500ReputationSeaport,
                                Achievement.Reach1000ReputationSeaport,
                                Achievement.Reach2000ReputationSeaport,
                                Achievement.Reach5000ReputationSeaport,
                                Achievement.Reach10000ReputationSeaport
                            });

                        break;
                    case Reputation.Castle:

                        await _achievementService.CheckAchievement(usersId.ToArray(),
                            new[]
                            {
                                Achievement.Reach500ReputationCastle,
                                Achievement.Reach1000ReputationCastle,
                                Achievement.Reach2000ReputationCastle,
                                Achievement.Reach5000ReputationCastle,
                                Achievement.Reach10000ReputationCastle,
                            });

                        break;
                    case Reputation.Village:

                        await _achievementService.CheckAchievement(usersId.ToArray(),
                            new[]
                            {
                                Achievement.Reach500ReputationVillage,
                                Achievement.Reach1000ReputationVillage,
                                Achievement.Reach2000ReputationVillage,
                                Achievement.Reach5000ReputationVillage,
                                Achievement.Reach10000ReputationVillage
                            });

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // подтверждаем убийтство босса
                embed.WithDescription(IzumiEventMessage.BossKilled.Parse());

                // получаем каналы сервера
                var channels = await _discordGuildService.GetChannels();
                // получаем канал дневник
                var diaryChan = await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id);
                // создаем строку с наградой
                var rewardString = IzumiEventMessage.ReputationAdded.Parse(
                    emotes.GetEmoteOrBlank(reputation.Emote(long.MaxValue)), reputationReward,
                    reputation.Location().Localize(true));

                var embedReward = new EmbedBuilder()
                    .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                    // описываем полученные награды
                    .WithDescription(IzumiEventMessage.BossRewardNotify.Parse(
                        reputation.Location().Localize(true), rewardString));

                await _discordEmbedService.SendEmbed(diaryChan, embedReward);
            }

            // изменяем сообщение
            await _discordEmbedService.ModifyEmbed(message, embed);
            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IMessageJob>(x =>
                    x.Delete(channelId, messageId),
                TimeSpan.FromHours(24));
        }


        public async Task ResetDebuff() =>
            await _propertyService.UpdateProperty(Property.BossDebuff, BossDebuff.None.GetHashCode());
    }
}
