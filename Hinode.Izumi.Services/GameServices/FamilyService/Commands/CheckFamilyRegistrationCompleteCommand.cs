using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record CheckFamilyRegistrationCompleteCommand(long FamilyId) : IRequest;
}
