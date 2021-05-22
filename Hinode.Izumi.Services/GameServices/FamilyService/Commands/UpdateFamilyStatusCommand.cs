using Hinode.Izumi.Data.Enums.FamilyEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record UpdateFamilyStatusCommand(long FamilyId, FamilyStatus NewStatus) : IRequest;
}
