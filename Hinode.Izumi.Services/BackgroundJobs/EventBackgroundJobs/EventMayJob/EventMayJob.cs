using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MessageJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
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

        private const string GrillEmote = "";

        public EventMayJob(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, IDiscordGuildService discordGuildService, IImageService imageService,
            IUserService userService, IInventoryService inventoryService, IFoodService foodService,
            ILocalizationService local)
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
        }

        public async Task Start()
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();

            // обновляем текущее событие в базе
            await _propertyService.UpdateProperty(Property.CurrentEvent, (long) Event.May);

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // описание события
                .WithDescription(".");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id), embed);
        }

        public async Task GrillAnons()
        {
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем id канала дневник
            var diaryId = channels[DiscordChannel.Diary].Id;

            var embed = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о мангале
                .WithDescription(".");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(diaryId), embed,
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>");

            // запускаем джобу с появлением мангала через пол часа
            BackgroundJob.Schedule<IEventMayJob>(
                x => x.GrillSpawn(),
                TimeSpan.FromMinutes(30));
        }

        public async Task GrillSpawn()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем роли сервера
            var roles = await _discordGuildService.GetRoles();
            // получаем канал события
            var eventChannel = await _discordGuildService.GetSocketTextChannel(
                channels[DiscordChannel.VillageEvents].Id);

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcVillageKio))
                // подверждаем появление мангала и рассказываем как его получить
                .WithDescription(".")
                // ожидаемая награда за получение мангала
                .AddField(".", ".")
                // изображение мангала
                // .WithImageUrl("")
                // длительность сбора мангала
                .WithFooter(".");

            // отправляем сообщение
            var message = await eventChannel.SendMessageAsync(
                // упоминаем роли события
                $"<@&{roles[DiscordRole.AllEvents].Id}> <@&{roles[DiscordRole.DailyEvents].Id}>",
                false, _discordEmbedService.BuildEmbed(embed));
            // добавляем реакцию для получения мангала
            await message.AddReactionAsync(new Emoji(GrillEmote));

            // запускаем джобу окончания мангала
            BackgroundJob.Schedule<IEventMayJob>(
                x => x.GrillEnd((long) message.Channel.Id, (long) message.Id),
                TimeSpan.FromMinutes(10));
        }

        public async Task GrillEnd(long channelId, long messageId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем сообщение
            var message = await _discordGuildService.GetIUserMessage(channelId, messageId);
            // получаем пользователей нажавших на реакцию
            var reactionUsers = await message
                .GetReactionUsersAsync(new Emoji(GrillEmote), int.MaxValue)
                .FlattenAsync();
            // получаем из всех пользователей только людей (без ботов)
            var users = reactionUsers
                .Where(x => x.IsBot == false)
                .Select(x => (long) x.Id)
                .ToArray();
            // получаем блюдо которое нужно выдать за участие в мангале
            var food = await _foodService.GetFood(
                // получаем id необходимого нам блюда
                await _propertyService.GetPropertyValue(Property.EventMayGrillFoodId));

            // снимаем реакции с сообщения
            await message.RemoveAllReactionsAsync();
            // полностью восстанавливаем энергию пользователям
            await _userService.AddEnergyToUser(users, long.MaxValue);
            // выдаем пользователям это блюдо
            await _inventoryService.AddItemToUser(users, InventoryCategory.Food, food.Id,
                // получаем количество которое необходимо выдать
                await _propertyService.GetPropertyValue(Property.EventMayGrillFoodAmount));

            var embed = new EmbedBuilder()
                // имя нпс
                .WithAuthor(Npc.Kio.Name())
                // изображение нпс
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.NpcVillageKio))
                // изображение мангала
                // .WithImageUrl("")
                // подверждаем что мангал успешно собран и пользователи получили награду
                .WithDescription(".");

            // изменяем сообщение
            await _discordEmbedService.ModifyEmbed(message, embed);
            // запускаем джобу с удалением сообщения
            BackgroundJob.Schedule<IMessageJob>(x =>
                    x.Delete(channelId, messageId),
                TimeSpan.FromHours(24));

            var embedReward = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                // оповещаем о том, что пользователи получили награду за мангал
                .WithDescription(".");

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketTextChannel(channels[DiscordChannel.Diary].Id), embedReward);
        }
    }
}
