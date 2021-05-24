using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckAlcoholIngredientsCommand(long UserId, long AlcoholId, long Amount = 1) : IRequest;

    public class CheckAlcoholIngredientsHandler : IRequestHandler<CheckAlcoholIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public CheckAlcoholIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAlcoholIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, alcoholId, amount) = request;
            var ingredients = await _mediator.Send(new GetAlcoholIngredientsQuery(alcoholId), cancellationToken);

            foreach (var ingredient in ingredients)
                await _mediator.Send(new CheckIngredientAmountCommand(
                        userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount),
                    cancellationToken);

            return new Unit();
        }
    }
}
