using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Commands
{
    public record DeleteMarketRequestCommand(long Id) : IRequest;

    public class DeleteMarketRequestHandler : IRequestHandler<DeleteMarketRequestCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteMarketRequestHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteMarketRequestCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from market_requests
                    where id = @id",
                    new {id = request.Id});

            return new Unit();
        }
    }
}
