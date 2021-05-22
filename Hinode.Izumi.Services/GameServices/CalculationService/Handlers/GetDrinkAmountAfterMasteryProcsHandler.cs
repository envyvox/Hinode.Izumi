using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetDrinkAmountAfterMasteryProcsHandler : IRequestHandler<GetDrinkAmountAfterMasteryProcsQuery, long>
    {
        private readonly IMediator _mediator;

        public GetDrinkAmountAfterMasteryProcsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetDrinkAmountAfterMasteryProcsQuery request, CancellationToken cancellationToken)
        {
            var (drinkId, userCraftingMastery, amount) = request;
            // TODO добавить сюда проверки после того как обсудим работу изготовления напитков
            return await Task.FromResult(amount);
        }
    }
}
