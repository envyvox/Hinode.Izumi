using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.RpgServices.CardService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.RpgServices.TrainingService.Impl
{
    [InjectableService]
    public class TrainingService : ITrainingService
    {
        private readonly IConnectionManager _con;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IInventoryService _inventoryService;
        private readonly ICardService _cardService;
        private readonly IImageService _imageService;
        private readonly IFoodService _foodService;
        private readonly IPropertyService _propertyService;

        public TrainingService(IConnectionManager con, IDiscordEmbedService discordEmbedService,
            IDiscordGuildService discordGuildService, IInventoryService inventoryService, ICardService cardService,
            IImageService imageService, IFoodService foodService, IPropertyService propertyService)
        {
            _con = con;
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _inventoryService = inventoryService;
            _cardService = cardService;
            _imageService = imageService;
            _foodService = foodService;
            _propertyService = propertyService;
        }

        public async Task<TrainingStep> GetUserTrainingStep(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TrainingStep>(@"
                    select step from user_trainings
                    where user_id = @userId",
                    new {userId});

        public async Task UpdateUserTrainingStep(long userId, TrainingStep step) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_trainings(user_id, step)
                    values (@userId, @step)
                    on conflict (user_id) do update
                        set step = @step,
                            updated_at = now()",
                    new {userId, step});

        public async Task CheckStep(long userId, TrainingStep step)
        {
            // получаем текущий шаг обучения пользователя
            var userStep = await GetUserTrainingStep(userId);

            // проверяем совпадает ли текущий шаг с тем, что мы ожидаем
            if (userStep != step) return;

            // подсчитываем следующий шаг обучения
            var nextStep = (TrainingStep) step.GetHashCode() + 1;
            var embed = new EmbedBuilder()
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.Training))
                // выводим название шага обучения
                .WithAuthor(nextStep.Name())
                // выводим описание шага обучения
                .WithDescription(nextStep.Description());

            // обновляем текущий шаг обучения пользователя на новый
            await UpdateUserTrainingStep(userId, nextStep);

            // выдаем определенные награды, если это предусматривает шаг обучения
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (nextStep == TrainingStep.CheckCards)
            {
                // добавляем пользователю особую карточку
                await _cardService.AddCardToUser(userId, 10);
            }
            if (nextStep == TrainingStep.Completed)
            {
                // добавляем пользователю награду за прохождение обучения
                await _inventoryService.AddItemToUser(
                    userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    // получаем количество награды за прохождение обучения
                    await _propertyService.GetPropertyValue(Property.EconomyTrainingAward));
            }
            if (nextStep == TrainingStep.CookFriedEgg)
            {
                // добавляем пользователю рецепт яичницы
                await _foodService.AddRecipeToUser(userId, 4);
                // добавляем пользователю ингредиенты яичницы
                await _inventoryService.AddItemToUser(userId, InventoryCategory.Product, 1);
            }

            if (nextStep == TrainingStep.TransitToCastle)
            {
                // выдаем пользователю особые тыквенные пироги на первое время
                await _inventoryService.AddItemToUser(userId, InventoryCategory.Food, 79, 30);
            }

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
        }
    }
}
