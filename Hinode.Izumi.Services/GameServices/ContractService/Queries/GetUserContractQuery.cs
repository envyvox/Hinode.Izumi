using Hinode.Izumi.Services.GameServices.ContractService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Queries
{
    public record GetUserContractQuery(long UserId) : IRequest<ContractRecord>;
}
