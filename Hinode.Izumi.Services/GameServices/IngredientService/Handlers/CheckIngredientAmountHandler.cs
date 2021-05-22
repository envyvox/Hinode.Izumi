using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.IngredientService.Commands;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Handlers
{
    public class CheckIngredientAmountHandler : IRequestHandler<CheckIngredientAmountCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CheckIngredientAmountHandler(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CheckIngredientAmountCommand request, CancellationToken cancellationToken)
        {
            var (userId, category, ingredientId, ingredientAmount, craftingAmount) = request;
            long userAmount;

            switch (category)
            {
                case IngredientCategory.Gathering:

                    var userGathering = await _mediator.Send(
                        new GetUserGatheringQuery(userId, ingredientId), cancellationToken);
                    userAmount = userGathering.Amount;

                    break;
                case IngredientCategory.Product:

                    var userProduct = await _mediator.Send(
                        new GetUserProductQuery(userId, ingredientId), cancellationToken);
                    userAmount = userProduct.Amount;

                    break;
                case IngredientCategory.Crafting:

                    var userCrafting = await _mediator.Send(
                        new GetUserCraftingQuery(userId, ingredientId), cancellationToken);
                    userAmount = userCrafting.Amount;

                    break;
                case IngredientCategory.Alcohol:

                    var userAlcohol = await _mediator.Send(
                        new GetUserAlcoholQuery(userId, ingredientId), cancellationToken);
                    userAmount = userAlcohol.Amount;

                    break;
                case IngredientCategory.Drink:

                    var userDrink = await _mediator.Send(
                        new GetUserDrinkQuery(userId, ingredientId), cancellationToken);
                    userAmount = userDrink.Amount;

                    break;
                case IngredientCategory.Crop:

                    var userCrop = await _mediator.Send(
                        new GetUserCropQuery(userId, ingredientId), cancellationToken);
                    userAmount = userCrop.Amount;

                    break;
                case IngredientCategory.Food:

                    var userFood = await _mediator.Send(
                        new GetUserFoodQuery(userId, ingredientId), cancellationToken);
                    userAmount = userFood.Amount;

                    break;
                case IngredientCategory.Seafood:

                    // TODO CHECK SEAFOOD AMOUNT
                    userAmount = 0;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (userAmount < ingredientAmount * craftingAmount)
            {
                var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
                var ingredientName = await _mediator.Send(
                    new GetIngredientNameQuery(category, ingredientId), cancellationToken);

                await Task.FromException(new Exception(IzumiReplyMessage.NoRequiredIngredientAmount.Parse(
                    emotes.GetEmoteOrBlank(ingredientName), _local.Localize(ingredientName, 5))));
            }

            return new Unit();
        }
    }
}
