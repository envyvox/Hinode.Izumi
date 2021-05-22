using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record StartReGrowthOnUserFieldCommand(long UserId, long FieldId) : IRequest;
}
