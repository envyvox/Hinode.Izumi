using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Commands
{
    public record RemoveContractFromUserCommand(long UserId) : IRequest;
}
