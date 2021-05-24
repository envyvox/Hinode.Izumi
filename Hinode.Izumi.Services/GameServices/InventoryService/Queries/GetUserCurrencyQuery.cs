using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserCurrencyQuery(long UserId, Currency Currency) : IRequest<UserCurrencyRecord>;

    public class GetUserCurrencyHandler : IRequestHandler<GetUserCurrencyQuery, UserCurrencyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserCurrencyHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserCurrencyRecord> Handle(GetUserCurrencyQuery request, CancellationToken cancellationToken)
        {
            var (userId, currency) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCurrencyRecord>(@"
                    select * from user_currencies
                    where user_id = @userId
                      and currency = @currency",
                    new {userId, currency});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(currency.ToString()), _local.Localize(currency.ToString(), 2))));

            return null;
        }
    }
}
