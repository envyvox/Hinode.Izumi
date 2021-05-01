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
            ISeedWebService seedWebService,
            ICropWebService cropWebService, IAlcoholWebService alcoholWebService, IDrinkWebService drinkWebService,
            IFishWebService fishWebService, IFoodWebService foodWebService)
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
                            names.Add(gathering.Name);
                        }

                        break;
                    case LocalizationCategory.Product:

                        var products = await _productWebService.GetAllProducts();
                        foreach (var product in products)
                        {
                            categories.Add(LocalizationCategory.Product.GetHashCode());
                            names.Add(product.Name);
                        }

                        break;
                    case LocalizationCategory.Crafting:

                        var craftings = await _craftingWebService.GetAllCrafting();
                        foreach (var crafting in craftings)
                        {
                            categories.Add(LocalizationCategory.Crafting.GetHashCode());
                            names.Add(crafting.Name);
                        }

                        break;
                    case LocalizationCategory.Alcohol:

                        var alcohols = await _alcoholWebService.GetAllAlcohols();
                        foreach (var alcohol in alcohols)
                        {
                            categories.Add(LocalizationCategory.Alcohol.GetHashCode());
                            names.Add(alcohol.Name);
                        }

                        break;
                    case LocalizationCategory.Drink:

                        var drinks = await _drinkWebService.GetAllDrinks();
                        foreach (var drink in drinks)
                        {
                            categories.Add(LocalizationCategory.Drink.GetHashCode());
                            names.Add(drink.Name);
                        }

                        break;
                    case LocalizationCategory.Seed:

                        var seeds = await _seedWebService.GetAllSeeds();
                        foreach (var seed in seeds)
                        {
                            categories.Add(LocalizationCategory.Seed.GetHashCode());
                            names.Add(seed.Name);
                        }

                        break;
                    case LocalizationCategory.Crop:

                        var crops = await _cropWebService.GetAllCrops();
                        foreach (var crop in crops)
                        {
                            categories.Add(LocalizationCategory.Crop.GetHashCode());
                            names.Add(crop.Name);
                        }

                        break;
                    case LocalizationCategory.Fish:

                        var fishes = await _fishWebService.GetAllFish();
                        foreach (var fish in fishes)
                        {
                            categories.Add(LocalizationCategory.Fish.GetHashCode());
                            names.Add(fish.Name);
                        }

                        break;
                    case LocalizationCategory.Food:

                        var foods = await _foodWebService.GetAllFood();
                        foreach (var food in foods)
                        {
                            categories.Add(LocalizationCategory.Food.GetHashCode());
                            names.Add(food.Name);
                        }

                        break;
                    case LocalizationCategory.Currency:

                        var currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>();
                        foreach (var currency in currencies)
                        {
                            categories.Add(LocalizationCategory.Currency.GetHashCode());
                            names.Add(currency.ToString());
                        }

                        break;
                    case LocalizationCategory.Bar:
                        break;
                    case LocalizationCategory.Box:

                        var boxes = Enum.GetValues(typeof(Box)).Cast<Box>();
                        foreach (var box in boxes)
                        {
                            categories.Add(LocalizationCategory.Box.GetHashCode());
                            names.Add(box.ToString());
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into localizations(category, name, single, double, multiply)
                    values (
                            unnest(array[@categories]),
                            unnest(array[@names]),
                            unnest(array[@names]),
                            unnest(array[@names]),
                            unnest(array[@names]))
                    on conflict (name) do nothing",
                    new {categories, names});
        }
    }
}
