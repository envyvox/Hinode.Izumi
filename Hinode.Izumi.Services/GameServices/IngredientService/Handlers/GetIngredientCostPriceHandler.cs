using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class GetIngredientCostPriceHandler : IRequestHandler<GetIngredientCostPriceQuery, long>
    {
        private readonly IMediator _mediator;

        public GetIngredientCostPriceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetIngredientCostPriceQuery request, CancellationToken cancellationToken)
        {
            var (category, ingredientId) = request;
            long costPrice = 0;

            switch (category)
            {
                case IngredientCategory.Gathering:

                    var gathering = await _mediator.Send(new GetGatheringQuery(ingredientId), cancellationToken);
                    costPrice += gathering.Price;

                    break;
                case IngredientCategory.Product:

                    var product = await _mediator.Send(new GetProductQuery(ingredientId), cancellationToken);
                    costPrice += product.Price;

                    break;
                case IngredientCategory.Crafting:

                    costPrice += await _mediator.Send(new GetCraftingCostPriceQuery(ingredientId), cancellationToken);

                    break;
                case IngredientCategory.Alcohol:

                    costPrice += await _mediator.Send(new GetAlcoholCostPriceQuery(ingredientId), cancellationToken);

                    break;
                case IngredientCategory.Drink:

                    costPrice += await _mediator.Send(new GetDrinkCostPriceQuery(ingredientId), cancellationToken);

                    break;
                case IngredientCategory.Crop:

                    var crop = await _mediator.Send(new GetCropByIdQuery(ingredientId), cancellationToken);
                    costPrice += crop.Price;

                    break;
                case IngredientCategory.Food:

                    costPrice += await _mediator.Send(new GetFoodCostPriceQuery(ingredientId), cancellationToken);

                    break;
                case IngredientCategory.Seafood:

                    // TODO ADD SEAFOOD COSTPRICE

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return costPrice;
        }
    }
}
