using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserFishHandler : IRequestHandler<GetUserFishQuery, UserFishRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserFishHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserFishRecord> Handle(GetUserFishQuery request, CancellationToken cancellationToken)
        {
            var (userId, fishId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFishRecord>(@"
                    select uf.*, f.name, f.rarity, f.seasons, f.price from user_fishes as uf
                        inner join fishes f
                            on f.id = uf.fish_id
                    where uf.user_id = @userId
                      and uf.fish_id = @fishId",
                    new {userId, fishId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var fish = await _mediator.Send(new GetFishQuery(fishId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(fish.Name), _local.Localize(fish.Name, 2))));

            return null;
        }
    }
}
