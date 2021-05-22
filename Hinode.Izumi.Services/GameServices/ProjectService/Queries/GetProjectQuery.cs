using Hinode.Izumi.Services.GameServices.ProjectService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Queries
{
    public record GetProjectQuery(long Id) : IRequest<ProjectRecord>;
}
