using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record CheckFamilyWithNameQuery(string Name) : IRequest<bool>;
}
