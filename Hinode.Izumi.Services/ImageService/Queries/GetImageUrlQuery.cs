using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.ImageService.Queries
{
    public record GetImageUrlQuery(Image Type) : IRequest<string>;
}
