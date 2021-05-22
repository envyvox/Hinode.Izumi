using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Commands
{
    public record CreateUserFieldsCommand(long UserId, long[] FieldsId) : IRequest;
}
