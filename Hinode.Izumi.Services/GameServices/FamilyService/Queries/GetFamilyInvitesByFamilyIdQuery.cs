using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyInvitesByFamilyIdQuery(long FamilyId) : IRequest<FamilyInviteRecord[]>;
}
