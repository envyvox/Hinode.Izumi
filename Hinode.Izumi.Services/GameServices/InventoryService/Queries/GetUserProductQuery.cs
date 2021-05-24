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
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserProductQuery(long UserId, long ProductId) : IRequest<UserProductRecord>;

    public class GetUserProductHandler : IRequestHandler<GetUserProductQuery, UserProductRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserProductHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserProductRecord> Handle(GetUserProductQuery request, CancellationToken cancellationToken)
        {
            var (userId, productId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserProductRecord>(@"
                    select up.*, p.name from user_products as up
                        inner join products p
                            on p.id = up.product_id
                    where up.user_id = @userId
                      and up.product_id = @productId",
                    new {userId, productId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var product = await _mediator.Send(new GetProductQuery(productId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(product.Name), _local.Localize(product.Name, 2))));

            return null;
        }
    }
}
