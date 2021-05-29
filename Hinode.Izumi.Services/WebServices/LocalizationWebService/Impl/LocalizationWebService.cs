using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.AlcoholWebService;
using Hinode.Izumi.Services.WebServices.CraftingWebService;
using Hinode.Izumi.Services.WebServices.CropWebService;
using Hinode.Izumi.Services.WebServices.DrinkWebService;
using Hinode.Izumi.Services.WebServices.FishWebService;
using Hinode.Izumi.Services.WebServices.FoodWebService;
using Hinode.Izumi.Services.WebServices.GatheringWebService;
using Hinode.Izumi.Services.WebServices.LocalizationWebService.Models;
using Hinode.Izumi.Services.WebServices.ProductWebService;
using Hinode.Izumi.Services.WebServices.SeedWebService;

namespace Hinode.Izumi.Services.WebServices.LocalizationWebService.Impl
{
    [InjectableService]
    public class LocalizationWebService : ILocalizationWebService
    {
        private readonly IConnectionManager _con;
        private readonly IGatheringWebService _gatheringWebService;
        private readonly IProductWebService _productWebService;
        private readonly ICraftingWebService _craftingWebService;
        private readonly ISeedWebService _seedWebService;
        private readonly ICropWebService _cropWebService;
        private readonly IAlcoholWebService _alcoholWebService;
        private readonly IDrinkWebService _drinkWebService;
        private readonly IFishWebService _fishWebService;
        private readonly IFoodWebService _foodWebService;

        public LocalizationWebService(IConnectionManager con, IGatheringWebService gatheringWebService,
            IProductWebService productWebService, ICraftingWebService craftingWebService,
            ISeedWebService seedWebService, ICropWebService cropWebService, IAlcoholWebService alcoholWebService,
            IDrinkWebService drinkWebService, IFishWebService fishWebService, IFoodWebService foodWebService)
        {
            _con = con;
            _gatheringWebService = gatheringWebService;
            _productWebService = productWebService;
            _craftingWebService = craftingWebService;
            _seedWebService = seedWebService;
            _cropWebService = cropWebService;
            _alcoholWebService = alcoholWebService;
            _drinkWebService = drinkWebService;
            _fishWebService = fishWebService;
            _foodWebService = foodWebService;
        }

        public async Task<IEnumerable<LocalizationWebModel>> GetAllLocalizations() =>
            await _con.GetConnection()
                .QueryAsync<LocalizationWebModel>(@"
                    select * from localizations
                    order by id");

        public async Task<LocalizationWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationWebModel>(@"
                    select * from localizations
                    where id = @id",
                    new {id});

        public async Task<LocalizationWebModel> Update(LocalizationWebModel model) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationWebModel>(@"
                    update localizations
                    set single = @single,
                        double = @double,
                        multiply = @multiply,
                        updated_at = now()
                    where id = @id
                    returning *",
                    new
                    {
                        id = model.Id,
                        single = model.Single,
                        @double = model.Double,
                        multiply = model.Multiply
                    });

        public async Task Upload()
        {
            var categories = new List<long>();
            var itemsId = new List<long>();
            var names = new List<string>();

            foreach (var category in Enum
                .GetValues(typeof(LocalizationCategory))
                .Cast<LocalizationCategory>())
            {
                switch (category)
                {
                    case LocalizationCategory.Gathering:

                        var gatherings = await _gatheringWebService.GetAllGathering();
                        foreach (var gathering in gatherings)
                        {
                            categories.Add(LocalizationCategory.Gathering.GetHashCode());
                            itemsId.Add(gathering.Id);
                            names.Add(gathering.Name);
                        }

                        break;
                    case LocalizationCategory.Product:

                        var products = await _productWebService.GetAllProducts();
                        foreach (var product in products)
                        {
                            categories.Add(LocalizationCategory.Product.GetHashCode());
                            itemsId.Add(product.Id);
                            names.Add(product.Name);
                        }

                        break;
                    case LocalizationCategory.Crafting:

                        var craftings = await _craftingWebService.GetAllCrafting();
                        foreach (var crafting in craftings)
                        {
                            categories.Add(LocalizationCategory.Crafting.GetHashCode());
                            itemsId.Add(crafting.Id);
                            names.Add(crafting.Name);
                        }

                        break;
                    case LocalizationCategory.Alcohol:

                        var alcohols = await _alcoholWebService.GetAllAlcohols();
                        foreach (var alcohol in alcohols)
                        {
                            categories.Add(LocalizationCategory.Alcohol.GetHashCode());
                            itemsId.Add(alcohol.Id);
                            names.Add(alcohol.Name);
                        }

                        break;
                    case LocalizationCategory.Drink:

                        var drinks = await _drinkWebService.GetAllDrinks();
                        foreach (var drink in drinks)
                        {
                            categories.Add(LocalizationCategory.Drink.GetHashCode());
                            itemsId.Add(drink.Id);
                            names.Add(drink.Name);
                        }

                        break;
                    case LocalizationCategory.Seed:

                        var seeds = await _seedWebService.GetAllSeeds();
                        foreach (var seed in seeds)
                        {
                            categories.Add(LocalizationCategory.Seed.GetHashCode());
                            itemsId.Add(seed.Id);
                            names.Add(seed.Name);
                        }

                        break;
                    case LocalizationCategory.Crop:

                        var crops = await _cropWebService.GetAllCrops();
                        foreach (var crop in crops)
                        {
                            categories.Add(LocalizationCategory.Crop.GetHashCode());
                            itemsId.Add(crop.Id);
                            names.Add(crop.Name);
                        }

                        break;
                    case LocalizationCategory.Fish:

                        var fishes = await _fishWebService.GetAllFish();
                        foreach (var fish in fishes)
                        {
                            categories.Add(LocalizationCategory.Fish.GetHashCode());
                            itemsId.Add(fish.Id);
                            names.Add(fish.Name);
                        }

                        break;
                    case LocalizationCategory.Food:

                        var foods = await _foodWebService.GetAllFood();
                        foreach (var food in foods)
                        {
                            categories.Add(LocalizationCategory.Food.GetHashCode());
                            itemsId.Add(food.Id);
                            names.Add(food.Name);
                        }

                        break;
                    case LocalizationCategory.Currency:

                        var currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>();
                        foreach (var currency in currencies)
                        {
                            categories.Add(LocalizationCategory.Currency.GetHashCode());
                            itemsId.Add(currency.GetHashCode());
                            names.Add(currency.ToString());
                        }

                        break;
                    case LocalizationCategory.Bar:

                        categories.Add(LocalizationCategory.Bar.GetHashCode());
                        itemsId.Add(1);
                        names.Add("Energy");

                        break;
                    case LocalizationCategory.Box:

                        var boxes = Enum.GetValues(typeof(Box)).Cast<Box>();
                        foreach (var box in boxes)
                        {
                            categories.Add(LocalizationCategory.Box.GetHashCode());
                            itemsId.Add(box.GetHashCode());
                            names.Add(box.ToString());
                        }

                        break;
                    case LocalizationCategory.Points:

                        categories.Add(LocalizationCategory.Points.GetHashCode());
                        itemsId.Add(1);
                        names.Add("AdventurePoints");

                        break;
                    case LocalizationCategory.Seafood:
                        // TODO UPLOAD SEAFOOD LOCALIZATION
                        break;
                    case LocalizationCategory.Event:

                        var bambooToys = Enum.GetValues(typeof(BambooToy)).Cast<BambooToy>();
                        foreach (var bambooToy in bambooToys)
                        {
                            categories.Add(LocalizationCategory.Event.GetHashCode());
                            itemsId.Add(bambooToy.GetHashCode());
                            names.Add(bambooToy.ToString());
                        }

                        break;
                    case LocalizationCategory.Vote:

                        var votes = Enum.GetValues(typeof(Vote)).Cast<Vote>();
                        foreach (var vote in votes)
                        {
                            categories.Add(LocalizationCategory.Vote.GetHashCode());
                            itemsId.Add(vote.GetHashCode());
                            names.Add(vote.ToString());
                        }

                        break;
                    case LocalizationCategory.Basic:

                        categories.Add(LocalizationCategory.Points.GetHashCode());
                        itemsId.Add(1);
                        names.Add("Publication");

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into localizations(category, item_id, name, single, double, multiply)
                    values (
                            unnest(array[@categories]),
                            unnest(array[@itemsId]),
                            unnest(array[@names]),
                            unnest(array[@names]),
                            unnest(array[@names]),
                            unnest(array[@names]))
                    on conflict (category, item_id) do nothing",
                    new {categories, itemsId, names});
        }
    }
}
