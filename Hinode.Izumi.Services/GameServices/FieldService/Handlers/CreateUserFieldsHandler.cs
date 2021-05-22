using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Handlers
{
    public class CreateUserFieldsHandler : IRequestHandler<CreateUserFieldsCommand>
    {
        private readonly IConnectionManager _con;

        public CreateUserFieldsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateUserFieldsCommand request, CancellationToken cancellationToken)
        {
            var (userId, fieldsId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_fields(user_id, field_id)
                    values (@userId, unnest(array[@fieldsId]))
                    on conflict (user_id, field_id) do nothing",
                    new {userId, fieldsId});

            return new Unit();
        }
    }
}
