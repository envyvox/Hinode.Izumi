using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Queries
{
    public record GetAlcoholPropertiesQuery(
            long AlcoholId,
            AlcoholProperty Property)
        : IRequest<AlcoholPropertyRecord>;
}
