using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.RpgServices.ImageService
{
    public interface IImageService
    {
        Task<string> GetImageUrl(Image image);
    }
}
