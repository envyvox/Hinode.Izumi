using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserCraftingHandler : IRequestHandler<GetUserCraftingQuery, UserCraftingRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserCraftingHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserCraftingRecord> Handle(GetUserCraftingQuery request, CancellationToken cancellationToken)
        {
            var (userId, craftingId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCraftingRecord>(@"
                    select uc.*, c.name from user_craftings as uc
                        inner join craftings c
                            on c.id = uc.crafting_id
                    where uc.user_id = @userId
                      and uc.crafting_id = @craftingId",
                    new {userId, craftingId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var crafting = await _mediator.Send(new GetCraftingQuery(craftingId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(crafting.Name), _local.Localize(crafting.Name, 2))));

            return null;
        }
    }
}
