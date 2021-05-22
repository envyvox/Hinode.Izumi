using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Queries
{
    public record CheckUserHasProjectQuery(long UserId, long ProjectId) : IRequest<bool>;
}
