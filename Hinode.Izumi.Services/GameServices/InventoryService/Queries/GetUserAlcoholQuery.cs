using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserAlcoholQuery(long UserId, long AlcoholId) : IRequest<UserAlcoholRecord>;

    public class GetUserAlcoholHandler : IRequestHandler<GetUserAlcoholQuery, UserAlcoholRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserAlcoholHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserAlcoholRecord> Handle(GetUserAlcoholQuery request, CancellationToken cancellationToken)
        {
            var (userId, alcoholId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserAlcoholRecord>(@"
                    select ua.*, a.name from user_alcohols as ua
                        inner join alcohols a
                            on a.id = ua.alcohol_id
                    where ua.user_id = @userId
                      and ua.alcohol_id = @alcoholId",
                    new {userId, alcoholId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var alcohol = await _mediator.Send(new GetAlcoholQuery(alcoholId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(alcohol.Name), _local.Localize(alcohol.Name, 2))));

            return null;
        }
    }
}
