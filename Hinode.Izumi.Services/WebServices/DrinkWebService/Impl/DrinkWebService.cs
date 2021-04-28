using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.DrinkWebService.Models;

namespace Hinode.Izumi.Services.WebServices.DrinkWebService.Impl
{
    [InjectableService]
    public class DrinkWebService : IDrinkWebService
    {
        private readonly IConnectionManager _con;

        public DrinkWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<DrinkWebModel>> GetAllDrinks() =>
            await _con.GetConnection()
                .QueryAsync<DrinkWebModel>(@"
                    select * from drinks
                    order by id");

        public async Task<DrinkWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<DrinkWebModel>(@"
                    select * from drinks
                    where id = @id",
                    new {id});

        public async Task<DrinkWebModel> Update(DrinkWebModel model) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<DrinkWebModel>(@"
                    insert into drinks(name, time)
                    values (@name, @time)
                    on conflict (name) do update
                    set name = @name,
                        time = @time,
                        updated_at = now()
                    returning *",
                    new
                    {
                        name = model.Name,
                        time = model.Time
                    });

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from drinks
                    where id = @id",
                    new {id});
    }
}
