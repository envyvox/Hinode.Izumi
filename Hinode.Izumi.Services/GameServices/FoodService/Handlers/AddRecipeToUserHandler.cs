using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FoodService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Handlers
{
    public class AddRecipeToUserHandler : IRequestHandler<AddRecipeToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddRecipeToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddRecipeToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, foodId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_recipes(user_id, food_id)
                    values (@userId, @foodId)
                    on conflict (user_id, food_id) do nothing",
                    new {userId, foodId});

            return new Unit();
        }
    }
}
