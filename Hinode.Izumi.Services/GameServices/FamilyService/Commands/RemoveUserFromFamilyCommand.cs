using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record RemoveUserFromFamilyCommand(long UserId) : IRequest;
}
