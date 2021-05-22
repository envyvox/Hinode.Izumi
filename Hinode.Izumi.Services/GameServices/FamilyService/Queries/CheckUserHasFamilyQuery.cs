using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record CheckUserHasFamilyQuery(long UserId) : IRequest<bool>;
}
