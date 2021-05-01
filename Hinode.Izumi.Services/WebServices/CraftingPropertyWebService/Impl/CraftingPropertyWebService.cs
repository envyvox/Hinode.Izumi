using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.CraftingPropertyWebService.Models;
using Hinode.Izumi.Services.WebServices.CraftingWebService;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.CraftingPropertyWebService.Impl
{
    [InjectableService]
    public class CraftingPropertyWebService : ICraftingPropertyWebService
    {
        private readonly IConnectionManager _con;
        private readonly ICraftingWebService _craftingWebService;
        private readonly IMemoryCache _cache;

        public CraftingPropertyWebService(IConnectionManager con, ICraftingWebService craftingWebService,
            IMemoryCache cache)
        {
            _con = con;
            _craftingWebService = craftingWebService;
            _cache = cache;
        }

        public async Task<IEnumerable<CraftingPropertyWebModel>> GetAllCraftingProperties() =>
            await _con.GetConnection()
                .QueryAsync<CraftingPropertyWebModel>(@"
                    select cp.*, c.name as CraftingName from crafting_properties as cp
                        inner join craftings c
                            on c.id = cp.crafting_id
                    order by cp.crafting_id");

        public async Task<CraftingPropertyWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingPropertyWebModel>(@"
                    select cp.*, c.name as CraftingName from crafting_properties as cp
                        inner join craftings c
                            on c.id = cp.crafting_id
                    where cp.id = @id",
                    new {id});

        public async Task<CraftingPropertyWebModel> Update(CraftingPropertyWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.CraftingPropertyKey, model.CraftingId, model.Property));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingPropertyWebModel>(@"
                    update crafting_properties
                    set mastery0 = @mastery0,
                        mastery50 = @mastery50,
                        mastery100 = @mastery100,
                        mastery150 = @mastery150,
                        mastery200 = @mastery200,
                        mastery250 = @mastery250,
                        updated_at = now()
                    where id = @id
                    returning *",
                    new
                    {
                        id = model.Id,
                        mastery0 = model.Mastery0,
                        mastery50 = model.Mastery50,
                        mastery100 = model.Mastery100,
                        mastery150 = model.Mastery150,
                        mastery200 = model.Mastery200,
                        mastery250 = model.Mastery250
                    });
        }

        public async Task Upload()
        {
            // получаем все изготавливаемые предметы
            var craftings = await _craftingWebService.GetAllCrafting();
            // получаем все свойства изготавливамого предмета в массив номеров
            var craftingProperties = Enum.GetValues(typeof(CraftingProperty))
                .Cast<CraftingProperty>()
                .Select(x => (long) x.GetHashCode())
                .ToArray();

            // для каждого изготавливаемого предмета добавляем его свойства в базу с значениями по-умолчанию
            foreach (var crafting in craftings)
            {
                await _con.GetConnection()
                    .ExecuteAsync(@"
                    insert into crafting_properties(crafting_id, property)
                    values (@craftingId, unnest(array[@craftingProperties]))
                    on conflict (crafting_id, property) do nothing",
                        new {craftingId = crafting.Id, craftingProperties});
            }
        }
    }
}
