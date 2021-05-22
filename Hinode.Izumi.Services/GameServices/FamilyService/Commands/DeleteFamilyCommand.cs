using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record DeleteFamilyCommand(long FamilyId) : IRequest;
}
