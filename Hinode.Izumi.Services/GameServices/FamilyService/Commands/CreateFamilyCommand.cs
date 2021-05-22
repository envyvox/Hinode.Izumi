using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record CreateFamilyCommand(string Name) : IRequest;
}
