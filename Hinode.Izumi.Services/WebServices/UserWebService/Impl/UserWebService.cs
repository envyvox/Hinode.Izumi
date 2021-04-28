using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.UserWebService.Models;

namespace Hinode.Izumi.Services.WebServices.UserWebService.Impl
{
    [InjectableService]
    public class UserWebService : IUserWebService
    {
        private readonly IConnectionManager _con;

        public UserWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<UserWebModel>> GetAllUsers() =>
            await _con.GetConnection()
                .QueryAsync<UserWebModel>(@"
                    select * from users
                    order by name");

        public async Task<UserWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWebModel>(@"
                    select * from users
                    where id = @id",
                    new {id});

        public async Task<UserWebModel> Update(UserWebModel model) =>
            // обновляем базу
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserWebModel>(@"
                    insert into users (id, name, about, title, gender, location, energy)
                    values (@id, @name, @about, @title, @gender, @location, @energy)
                    on conflict (id) do update
                        set name = @name,
                            about = @about,
                            title = @title,
                            gender = @gender,
                            location = @location,
                            energy = @energy,
                            updated_at = now()
                    returning *",
                    new
                    {
                        id = long.Parse(model.Id),
                        name = model.Name,
                        about = model.About,
                        title = model.Title,
                        gender = model.Gender,
                        location = model.Location,
                        energy = model.Energy
                    });

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from users
                    where id = @id",
                    new {id});
    }
}
