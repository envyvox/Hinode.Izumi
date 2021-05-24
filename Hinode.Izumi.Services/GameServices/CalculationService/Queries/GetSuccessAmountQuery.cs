using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetSuccessAmountQuery(long Chance, long DoubleChance, long Amount) : IRequest<long>;

    public class GetSuccessAmountHandler : IRequestHandler<GetSuccessAmountQuery, long>
    {
        private readonly Random _random = new();

        public async Task<long> Handle(GetSuccessAmountQuery request, CancellationToken cancellationToken)
        {
            var (chance, doubleChance, amount) = request;
            return await Task.FromResult(chance >= _random.Next(1, 101)
                ? doubleChance >= _random.Next(1, 101)
                    ? amount * 2
                    : amount
                : 0);
        }
    }
}
