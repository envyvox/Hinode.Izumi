using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record UpdateFamilyNameCommand(long FamilyId, string NewName) : IRequest;
}
