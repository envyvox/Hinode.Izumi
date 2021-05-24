using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Commands
{
    public record RemoveItemFromUserByIngredientCategoryCommand(
            long UserId,
            IngredientCategory Category,
            long ItemId,
            long Amount = 1)
        : IRequest;

    public class RemoveItemFromUserByIngredientCategoryHandler
        : IRequestHandler<RemoveItemFromUserByIngredientCategoryCommand>
    {
        private readonly IMediator _mediator;

        public RemoveItemFromUserByIngredientCategoryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveItemFromUserByIngredientCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var (userId, category, itemId, amount) = request;

            await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(userId, category switch
            {
                IngredientCategory.Gathering => InventoryCategory.Gathering,
                IngredientCategory.Product => InventoryCategory.Product,
                IngredientCategory.Crafting => InventoryCategory.Crafting,
                IngredientCategory.Alcohol => InventoryCategory.Alcohol,
                IngredientCategory.Drink => InventoryCategory.Drink,
                IngredientCategory.Crop => InventoryCategory.Crop,
                IngredientCategory.Food => InventoryCategory.Food,
                IngredientCategory.Seafood => InventoryCategory.Seafood,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, itemId, amount), cancellationToken);

            return new Unit();
        }
    }
}
