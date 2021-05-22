using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Queries
{
    public record GetUserByNamePatternQuery(string NamePattern) : IRequest<UserRecord>;
}
