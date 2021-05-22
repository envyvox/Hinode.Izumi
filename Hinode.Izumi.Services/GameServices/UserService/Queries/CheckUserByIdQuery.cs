using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record CheckUserByIdQuery(long Id) : IRequest<bool>;
}
