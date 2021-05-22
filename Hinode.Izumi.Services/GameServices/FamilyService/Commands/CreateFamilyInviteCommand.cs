using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record CreateFamilyInviteCommand(long FamilyId, long UserId) : IRequest;
}
