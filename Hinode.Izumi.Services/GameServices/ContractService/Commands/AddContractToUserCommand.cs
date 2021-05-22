using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Commands
{
    public record AddContractToUserCommand(long UserId, long ContractId) : IRequest;
}
