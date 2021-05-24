using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserDrinkQuery(long UserId, long DrinkId) : IRequest<UserDrinkRecord>;

    public class GetUserDrinkHandler : IRequestHandler<GetUserDrinkQuery, UserDrinkRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserDrinkHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserDrinkRecord> Handle(GetUserDrinkQuery request, CancellationToken cancellationToken)
        {
            var (userId, drinkId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserDrinkRecord>(@"
                    select ud.*, d.name from user_drinks as ud
                        inner join drinks d
                            on d.id = ud.drink_id
                    where ud.user_id = @userId
                      and ud.drink_id = @drinkId",
                    new {userId, drinkId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var drink = await _mediator.Send(new GetDrinkQuery(drinkId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(drink.Name), _local.Localize(drink.Name, 2))));

            return null;
        }
    }
}
