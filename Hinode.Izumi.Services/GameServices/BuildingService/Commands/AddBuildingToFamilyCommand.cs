using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Commands
{
    public record AddBuildingToFamilyCommand(long FamilyId, Building Type) : IRequest;
}
