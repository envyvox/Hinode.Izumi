using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Commands;
using Hinode.Izumi.Services.WebServices.EmoteWebService.Models;
using MediatR;

namespace Hinode.Izumi.Services.WebServices.EmoteWebService.Impl
{
    [InjectableService]
    public class EmoteWebService : IEmoteWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public EmoteWebService(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<IEnumerable<EmoteWebModel>> GetAllEmotes() =>
            await _con.GetConnection()
                .QueryAsync<EmoteWebModel>(@"
                    select * from emotes
                    order by name");

        public async Task<EmoteWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<EmoteWebModel>(@"
                    select * from emotes
                    where id = @id",
                    new {id});

        public async Task UploadEmotes() => await _mediator.Send(new UploadEmotesCommand());
    }
}
