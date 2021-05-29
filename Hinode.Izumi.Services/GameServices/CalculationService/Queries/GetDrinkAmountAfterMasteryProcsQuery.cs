using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetDrinkAmountAfterMasteryProcsQuery(
            long DrinkId,
            long UserCraftingMastery,
            long Amount)
        : IRequest<long>;

    public class GetDrinkAmountAfterMasteryProcsHandler : IRequestHandler<GetDrinkAmountAfterMasteryProcsQuery, long>
    {
        public async Task<long> Handle(GetDrinkAmountAfterMasteryProcsQuery request,
            CancellationToken cancellationToken)
        {
            // TODO добавить сюда проверки после того как обсудим работу изготовления напитков
            return await Task.FromResult(request.Amount);
        }
    }
}
