using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record CheckUserByNameQuery(string Name) : IRequest<bool>;
}
