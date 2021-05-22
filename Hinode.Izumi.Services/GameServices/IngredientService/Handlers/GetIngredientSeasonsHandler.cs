using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class GetIngredientSeasonsHandler : IRequestHandler<GetIngredientSeasonsQuery, List<Season>>
    {
        private readonly IMediator _mediator;

        public GetIngredientSeasonsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Season>> Handle(GetIngredientSeasonsQuery request, CancellationToken cancellationToken)
        {
            var (category, ingredientId) = request;
            var seasons = new List<Season>();

            switch (category)
            {
                case IngredientCategory.Gathering:
                case IngredientCategory.Product:
                case IngredientCategory.Seafood:
                    // собирательские предметы, продукты и морепродукты игнорируются, т.к. не имеют привязки к сезону
                    break;
                case IngredientCategory.Crafting:

                    var craftingSeasons = new List<Season>();
                    var craftingIngredients = await _mediator.Send(
                        new GetCraftingIngredientsQuery(ingredientId), cancellationToken);

                    foreach (var craftingIngredient in craftingIngredients)
                    {
                        var craftingIngredientSeasons = await _mediator.Send(
                            new GetIngredientSeasonsQuery(craftingIngredient.Category, craftingIngredient.IngredientId),
                            cancellationToken);

                        foreach (var craftingIngredientSeason in craftingIngredientSeasons)
                        {
                            if (!craftingSeasons.Contains(craftingIngredientSeason))
                                craftingSeasons.Add(craftingIngredientSeason);
                        }
                    }

                    foreach (var craftingSeason in craftingSeasons)
                    {
                        if (!seasons.Contains(craftingSeason))
                            seasons.Add(craftingSeason);
                    }

                    break;
                case IngredientCategory.Alcohol:

                    var alcoholSeasons = new List<Season>();
                    var alcoholIngredients = await _mediator.Send(
                        new GetAlcoholIngredientsQuery(ingredientId), cancellationToken);

                    foreach (var alcoholIngredient in alcoholIngredients)
                    {
                        var alcoholIngredientSeasons = await _mediator.Send(
                            new GetIngredientSeasonsQuery(alcoholIngredient.Category, alcoholIngredient.IngredientId),
                            cancellationToken);

                        foreach (var alcoholIngredientSeason in alcoholIngredientSeasons)
                        {
                            if (!alcoholSeasons.Contains(alcoholIngredientSeason))
                                alcoholSeasons.Add(alcoholIngredientSeason);
                        }
                    }

                    foreach (var alcoholSeason in alcoholSeasons)
                    {
                        if (!seasons.Contains(alcoholSeason))
                            seasons.Add(alcoholSeason);
                    }

                    break;
                case IngredientCategory.Drink:

                    var drinkSeasons = new List<Season>();
                    var drinkIngredients = await _mediator.Send(
                        new GetDrinkIngredientsQuery(ingredientId), cancellationToken);

                    foreach (var drinkIngredient in drinkIngredients)
                    {
                        var drinkIngredientSeasons = await _mediator.Send(
                            new GetIngredientSeasonsQuery(drinkIngredient.Category, drinkIngredient.IngredientId),
                            cancellationToken);

                        foreach (var drinkIngredientSeason in drinkIngredientSeasons)
                        {
                            if (!drinkSeasons.Contains(drinkIngredientSeason))
                                drinkSeasons.Add(drinkIngredientSeason);
                        }
                    }

                    foreach (var drinkSeason in drinkSeasons)
                    {
                        if (!seasons.Contains(drinkSeason))
                            seasons.Add(drinkSeason);
                    }

                    break;
                case IngredientCategory.Crop:

                    var crop = await _mediator.Send(new GetCropByIdQuery(ingredientId), cancellationToken);

                    if (!seasons.Contains(crop.Season)) seasons.Add(crop.Season);

                    break;
                case IngredientCategory.Food:

                    var foodSeasons = new List<Season>();
                    var foodIngredients = await _mediator.Send(
                        new GetFoodIngredientsQuery(ingredientId), cancellationToken);

                    foreach (var foodIngredient in foodIngredients)
                    {
                        var foodIngredientSeasons = await _mediator.Send(
                            new GetIngredientSeasonsQuery(foodIngredient.Category, foodIngredient.IngredientId),
                            cancellationToken);

                        foreach (var foodIngredientSeason in foodIngredientSeasons)
                        {
                            if (!foodSeasons.Contains(foodIngredientSeason))
                                foodSeasons.Add(foodIngredientSeason);
                        }
                    }

                    foreach (var foodSeason in foodSeasons)
                    {
                        if (!seasons.Contains(foodSeason))
                            seasons.Add(foodSeason);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return seasons;
        }
    }
}
