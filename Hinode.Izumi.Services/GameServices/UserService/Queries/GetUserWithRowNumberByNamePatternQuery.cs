using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record GetUserWithRowNumberByNamePatternQuery(string NamePattern) : IRequest<UserWithRowNumberRecord>;
}
