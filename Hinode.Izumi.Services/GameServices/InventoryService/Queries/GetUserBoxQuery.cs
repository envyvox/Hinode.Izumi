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
    public record GetUserBoxQuery(long UserId, Box Box) : IRequest<UserBoxRecord>;

    public class GetUserBoxHandler : IRequestHandler<GetUserBoxQuery, UserBoxRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserBoxHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserBoxRecord> Handle(GetUserBoxQuery request, CancellationToken cancellationToken)
        {
            var (userId, box) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserBoxRecord>(@"
                    select * from user_boxes
                    where user_id = @userId
                      and box = @box",
                    new {userId, box});


            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(box.Emote()), _local.Localize(box.ToString()))));

            return null;
        }
    }
}
