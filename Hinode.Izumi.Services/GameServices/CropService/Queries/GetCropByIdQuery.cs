using Hinode.Izumi.Services.GameServices.CropService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CropService.Queries
{
    public record GetCropByIdQuery(long Id) : IRequest<CropRecord>;
}
