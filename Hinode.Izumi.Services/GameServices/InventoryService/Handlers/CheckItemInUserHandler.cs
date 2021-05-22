using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public record CheckItemInUserHandler : IRequestHandler<CheckItemInUserQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckItemInUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckItemInUserQuery request, CancellationToken cancellationToken)
        {
            var (userId, category, itemId) = request;
            var query = category switch
            {
                InventoryCategory.Currency => @"
                    select 1 from user_currencies
                    where user_id = @userId
                      and currency = @itemId",

                InventoryCategory.Gathering => @"
                    select 1 from user_gatherings
                    where user_id = @userId
                      and gathering_id = @itemId",

                InventoryCategory.Product => @"
                    select 1 from user_products
                    where user_id = @userId
                      and product_id = @itemId",

                InventoryCategory.Crafting => @"
                    select 1 from user_craftings
                    where user_id = @userId
                      and crafting_id = @itemId",

                InventoryCategory.Alcohol => @"
                    select 1 from user_alcohols
                    where user_id = @userId
                      and alcohol_id = @itemId",

                InventoryCategory.Drink => @"
                    select 1 from user_drinks
                    where user_id = @userId
                      and drink_id = @itemId",

                InventoryCategory.Seed => @"
                    select 1 from user_seeds
                    where user_id = @userId
                      and seed_id = @itemId",

                InventoryCategory.Crop => @"
                    select 1 from user_crops
                    where user_id = @userId
                      and crop_id = @itemId",

                InventoryCategory.Fish => @"
                    select 1 from user_fishes
                    where user_id = @userId
                      and fish_id = @itemId",

                InventoryCategory.Food => @"
                    select 1 from user_foods
                    where user_id = @userId
                      and food_id = @itemId",

                InventoryCategory.Box => @"
                    select 1 from user_boxes
                    where user_id = @userId
                      and box = @itemId",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(query,
                    new {userId, itemId});
        }
    }
}
