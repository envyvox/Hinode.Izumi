using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Queries
{
    public record GetIngredientNameQuery(IngredientCategory Category, long IngredientId) : IRequest<string>;

    public class GetIngredientNameHandler : IRequestHandler<GetIngredientNameQuery, string>
    {
        private readonly IMediator _mediator;

        public GetIngredientNameHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(GetIngredientNameQuery request, CancellationToken cancellationToken)
        {
            var (category, ingredientId) = request;
            return await _mediator.Send(new GetItemNameQuery(category switch
            {
                IngredientCategory.Gathering => InventoryCategory.Gathering,
                IngredientCategory.Product => InventoryCategory.Product,
                IngredientCategory.Crafting => InventoryCategory.Crafting,
                IngredientCategory.Alcohol => InventoryCategory.Alcohol,
                IngredientCategory.Drink => InventoryCategory.Drink,
                IngredientCategory.Crop => InventoryCategory.Crop,
                IngredientCategory.Food => InventoryCategory.Food,
                IngredientCategory.Seafood => InventoryCategory.Seafood,
                _ => throw new ArgumentOutOfRangeException()
            }, ingredientId), cancellationToken);
        }
    }
}
