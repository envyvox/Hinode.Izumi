using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record GetUserWithRowNumberByIdQuery(long Id) : IRequest<UserWithRowNumberRecord>;
}
