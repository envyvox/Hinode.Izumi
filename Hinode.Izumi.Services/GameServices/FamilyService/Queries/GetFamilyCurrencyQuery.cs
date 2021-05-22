using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyCurrencyQuery(long FamilyId, Currency Currency) : IRequest<FamilyCurrencyRecord>;
}
