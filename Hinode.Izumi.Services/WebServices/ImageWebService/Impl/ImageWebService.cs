using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.ImageWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.ImageWebService.Impl
{
    [InjectableService]
    public class ImageWebService : IImageWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public ImageWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<ImageWebModel>> GetAllImages() =>
            await _con.GetConnection()
                .QueryAsync<ImageWebModel>(@"
                    select * from images
                    order by type");

        public async Task<ImageWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ImageWebModel>(@"
                    select * from images
                    where id = @id",
                    new {id});

        public async Task<ImageWebModel> Update(ImageWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.ImageKey, model.Type));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ImageWebModel>(@"
                    update images
                    set url = @url,
                        updated_at = now()
                    where id = @id
                    returning *",
                    new
                    {
                        id = model.Id,
                        url = model.Url
                    });
        }

        public async Task Upload()
        {
            // переписываем список изображений в массив номером (из-за проблем каста нужно сделать это вручную)
            var types = Enum.GetValues(typeof(Image))
                .Cast<Image>()
                .Select(x => x.GetHashCode())
                .ToArray();

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into images(type, url)
                    values (unnest(array[@types]), 'null')
                    on conflict (type) do nothing",
                    new {types});
        }
    }
}
