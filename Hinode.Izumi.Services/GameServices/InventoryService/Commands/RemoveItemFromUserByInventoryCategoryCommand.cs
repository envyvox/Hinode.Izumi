using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Commands
{
    public record RemoveItemFromUserByInventoryCategoryCommand(
            long UserId,
            InventoryCategory Category,
            long ItemId,
            long Amount = 1)
        : IRequest;

    public class RemoveItemFromUserByInventoryCategoryHandler
        : IRequestHandler<RemoveItemFromUserByInventoryCategoryCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public RemoveItemFromUserByInventoryCategoryHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveItemFromUserByInventoryCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var (userId, category, itemId, amount) = request;

            var query = category switch
            {
                InventoryCategory.Currency => @"
                    update user_currencies
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and currency = @itemId",

                InventoryCategory.Gathering => @"
                    update user_gatherings
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and gathering_id = @itemId",

                InventoryCategory.Product => @"
                    update user_products
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and product_id = @itemId",

                InventoryCategory.Crafting => @"
                    update user_craftings
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and crafting_id = @itemId",

                InventoryCategory.Alcohol => @"
                    update user_alcohols
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and alcohol_id = @itemId",

                InventoryCategory.Drink => @"
                    update user_drinks
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and drink_id = @itemId",

                InventoryCategory.Seed => @"
                    update user_seeds
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and seed_id = @itemId",

                InventoryCategory.Crop => @"
                    update user_crops
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and crop_id = @itemId",

                InventoryCategory.Fish => @"
                    update user_fishes
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and fish_id = @itemId",

                InventoryCategory.Food => @"
                    update user_foods
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and food_id = @itemId",

                InventoryCategory.Box => @"
                    update user_boxes
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and box = @itemId",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            await _con.GetConnection()
                .ExecuteAsync(query,
                    new {userId, itemId, amount});

            if (category == InventoryCategory.Currency)
                await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.CurrencySpent, amount),
                    cancellationToken);

            return new Unit();
        }
    }
}
