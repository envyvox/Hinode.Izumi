using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Commands
{
    public record AddBuildingToUserCommand(long UserId, Building Type) : IRequest;
}
