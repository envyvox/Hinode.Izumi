using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserCardQuery(long UserId, long CardId) : IRequest<CardRecord>;

    public class GetUserCardHandler : IRequestHandler<GetUserCardQuery, CardRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserCardHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CardRecord> Handle(GetUserCardQuery request, CancellationToken cancellationToken)
        {
            var (userId, cardId) = request;
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CardRecord>(@"
                    select c.* from user_cards as uc
                        inner join cards c
                            on c.id = uc.card_id
                    where uc.user_id = @userId
                      and uc.card_id = @cardId",
                    new {userId, cardId});

            if (res is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserCard.Parse()));

            return res;
        }
    }
}
