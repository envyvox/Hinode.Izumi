using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.ContractService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ContractService.Queries
{
    public record GetContractsInLocationQuery(Location Location) : IRequest<ContractRecord[]>;
}
