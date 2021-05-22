using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record UpdateFamilyDescriptionCommand(long FamilyId, string NewDescription) : IRequest;
}
