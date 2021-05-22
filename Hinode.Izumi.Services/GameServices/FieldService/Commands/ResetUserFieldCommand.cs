using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record ResetUserFieldCommand(long UserId, long FieldId) : IRequest;
}
