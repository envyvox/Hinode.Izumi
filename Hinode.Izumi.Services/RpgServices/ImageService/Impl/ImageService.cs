using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;

namespace Hinode.Izumi.Services.RpgServices.ImageService.Impl
{
    [InjectableService]
    public class ImageService : IImageService
    {
        private readonly IConnectionManager _con;

        public ImageService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<string> GetImageUrl(Image image) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select url from images
                    where type = @image
                    order by random()
                    limit 1",
                    new {image});
    }
}
