using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record AddUserToFamilyByFamilyIdCommand(long UserId, long FamilyId) : IRequest;
}
