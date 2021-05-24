using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetItemNameQuery(InventoryCategory Category, long ItemId) : IRequest<string>;

    public class GetItemNameHandler : IRequestHandler<GetItemNameQuery, string>
    {
        private readonly IMediator _mediator;

        public GetItemNameHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(GetItemNameQuery request, CancellationToken cancellationToken)
        {
            var (category, itemId) = request;

            switch (category)
            {
                case InventoryCategory.Currency:

                    return ((Currency) itemId).ToString();

                case InventoryCategory.Box:

                    return ((Box) itemId).ToString();

                case InventoryCategory.Gathering:

                    var gathering = await _mediator.Send(new GetGatheringQuery(itemId), cancellationToken);
                    return gathering.Name;

                case InventoryCategory.Product:

                    var product = await _mediator.Send(new GetProductQuery(itemId), cancellationToken);
                    return product.Name;

                case InventoryCategory.Crafting:

                    var crafting = await _mediator.Send(new GetCraftingQuery(itemId), cancellationToken);
                    return crafting.Name;

                case InventoryCategory.Alcohol:

                    var alcohol = await _mediator.Send(new GetAlcoholQuery(itemId), cancellationToken);
                    return alcohol.Name;

                case InventoryCategory.Drink:

                    var drink = await _mediator.Send(new GetDrinkQuery(itemId), cancellationToken);
                    return drink.Name;

                case InventoryCategory.Seed:

                    var seed = await _mediator.Send(new GetSeedQuery(itemId), cancellationToken);
                    return seed.Name;

                case InventoryCategory.Crop:

                    var crop = await _mediator.Send(new GetCropByIdQuery(itemId), cancellationToken);
                    return crop.Name;

                case InventoryCategory.Fish:

                    var fish = await _mediator.Send(new GetFishQuery(itemId), cancellationToken);
                    return fish.Name;

                case InventoryCategory.Food:

                    var food = await _mediator.Send(new GetFoodQuery(itemId), cancellationToken);
                    return food.Name;

                case InventoryCategory.Seafood:

                    // TODO RETURN SEAFOOD NAME
                    return "";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
