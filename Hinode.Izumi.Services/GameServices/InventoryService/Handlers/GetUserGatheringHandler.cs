using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserGatheringHandler : IRequestHandler<GetUserGatheringQuery, UserGatheringRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserGatheringHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserGatheringRecord> Handle(GetUserGatheringQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, gatheringId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserGatheringRecord>(@"
                    select ug.*, g.name from user_gatherings as ug
                        inner join gatherings g
                            on g.id = ug.gathering_id
                    where ug.user_id = @userId
                      and ug.gathering_id = @gatheringId",
                    new {userId, gatheringId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var gathering = await _mediator.Send(new GetGatheringQuery(gatheringId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(gathering.Name), _local.Localize(gathering.Name, 2))));

            return null;
        }
    }
}
