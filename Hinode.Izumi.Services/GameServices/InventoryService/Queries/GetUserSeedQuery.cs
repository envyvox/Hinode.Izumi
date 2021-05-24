using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserSeedQuery(long UserId, long SeedId) : IRequest<UserSeedRecord>;

    public class GetUserSeedHandler : IRequestHandler<GetUserSeedQuery, UserSeedRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserSeedHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserSeedRecord> Handle(GetUserSeedQuery request, CancellationToken cancellationToken)
        {
            var (userId, seedId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserSeedRecord>(@"
                    select us.*, s.name, s.season from user_seeds as us
                        inner join seeds s
                            on s.id = us.seed_id
                    where us.user_id = @userId
                      and us.seed_id = @seedId",
                    new {userId, seedId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var seed = await _mediator.Send(new GetSeedQuery(seedId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name, 2))));

            return null;
        }
    }
}
