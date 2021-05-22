using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record AddCurrencyToFamilyCommand(long FamilyId, Currency Currency, long Amount) : IRequest;
}
