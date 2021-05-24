using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.IngredientService.Commands
{
    public record CheckProjectIngredientsCommand(long UserId, long ProjectId, long Amount = 1) : IRequest;

    public class CheckProjectIngredientsHandler : IRequestHandler<CheckProjectIngredientsCommand>
    {
        private readonly IMediator _mediator;

        public CheckProjectIngredientsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckProjectIngredientsCommand request, CancellationToken cancellationToken)
        {
            var (userId, projectId, amount) = request;
            var ingredients = await _mediator.Send(new GetProjectIngredientsQuery(projectId), cancellationToken);

            foreach (var ingredient in ingredients)
                await _mediator.Send(new CheckIngredientAmountCommand(
                        userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount),
                    cancellationToken);

            return new Unit();
        }
    }
}
