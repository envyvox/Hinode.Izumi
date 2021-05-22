using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserFoodHandler : IRequestHandler<GetUserFoodQuery, UserFoodRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserFoodHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserFoodRecord> Handle(GetUserFoodQuery request, CancellationToken cancellationToken)
        {
            var (userId, foodId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFoodRecord>(@"
                    select uf.*, f.name, f.mastery from user_foods as uf
                        inner join foods f
                            on f.id = uf.food_id
                    where uf.user_id = @userId
                      and uf.food_id = @foodId",
                    new {userId, foodId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var food = await _mediator.Send(new GetFoodQuery(foodId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name, 2))));

            return null;
        }
    }
}
