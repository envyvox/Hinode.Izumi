using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyCurrenciesQuery(long FamilyId) : IRequest<Dictionary<Currency, FamilyCurrencyRecord>>;
}
