using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class AddItemToUsersByInventoryCategoryHandler : IRequestHandler<AddItemToUsersByInventoryCategoryCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddItemToUsersByInventoryCategoryHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddItemToUsersByInventoryCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var (usersId, category, itemId, amount) = request;

            var query = category switch
            {
                InventoryCategory.Currency => @"
                    insert into user_currencies as u (user_id, currency, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, currency) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Gathering => @"
                    insert into user_gatherings as u (user_id, gathering_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, gathering_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Product => @"
                    insert into user_products as u (user_id, product_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, product_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Crafting => @"
                    insert into user_craftings as u (user_id, crafting_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, crafting_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Alcohol => @"
                    insert into user_alcohols as u (user_id, alcohol_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, alcohol_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Drink => @"
                    insert into user_drinks as u (user_id, drink_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, drink_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Seed => @"
                    insert into user_seeds as u (user_id, seed_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, seed_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Crop => @"
                    insert into user_crops as u (user_id, crop_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, crop_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Fish => @"
                    insert into user_fishes as u (user_id, fish_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, fish_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Food => @"
                    insert into user_foods as u (user_id, food_id, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, food_id) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                InventoryCategory.Box => @"
                    insert into user_boxes as u (user_id, box, amount)
                    values (unnest(array[@usersId]), @itemId, @amount)
                    on conflict (user_id, box) do update
                        set amount = u.amount + @amount,
                            updated_at = now()",

                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };

            await _con.GetConnection()
                .ExecuteAsync(query,
                    new {usersId, itemId, amount});

            if (category == InventoryCategory.Currency)
                await _mediator.Send(new AddStatisticToUsersCommand(usersId, Statistic.CurrencyEarned, amount),
                    cancellationToken);

            return new Unit();
        }
    }
}
