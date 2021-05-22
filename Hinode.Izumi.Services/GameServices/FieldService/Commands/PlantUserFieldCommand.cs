using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record PlantUserFieldCommand(long UserId, long FieldId, long SeedId) : IRequest;
}
