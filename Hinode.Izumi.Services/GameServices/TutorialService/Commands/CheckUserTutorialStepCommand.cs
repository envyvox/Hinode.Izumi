using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.FoodService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.GameServices.TutorialService.Commands
{
    public record CheckUserTutorialStepCommand(long UserId, TutorialStep Step) : IRequest;

    public class CheckUserTutorialStepHandler : IRequestHandler<CheckUserTutorialStepCommand>
    {
        private readonly IMediator _mediator;

        public CheckUserTutorialStepHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckUserTutorialStepCommand request, CancellationToken cancellationToken)
        {
            var (userId, step) = request;
            // получаем текущий шаг обучения пользователя
            var userStep = await _mediator.Send(new GetUserTutorialStepQuery(userId), cancellationToken);

            // проверяем совпадает ли текущий шаг с тем, что мы ожидаем
            if (userStep != step) return new Unit();

            // подсчитываем следующий шаг обучения
            var nextStep = (TutorialStep) step.GetHashCode() + 1;

            // обновляем текущий шаг обучения пользователя на новый
            await _mediator.Send(new UpdateUserTutorialStepCommand(userId, nextStep), cancellationToken);

            // выдаем определенные награды, если это предусматривает шаг обучения
            if (nextStep == TutorialStep.CheckCards)
            {
                // добавляем пользователю особую карточку
                await _mediator.Send(new AddCardToUserCommand(userId, 10), cancellationToken);
            }

            if (nextStep == TutorialStep.Completed)
            {
                // добавляем пользователю награду за прохождение обучения
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        // получаем количество награды за прохождение обучения
                        await _mediator.Send(new GetPropertyValueQuery(Property.EconomyTrainingAward),
                            cancellationToken)),
                    cancellationToken);
            }

            if (nextStep == TutorialStep.CookFriedEgg)
            {
                // добавляем пользователю рецепт яичницы
                await _mediator.Send(new AddRecipeToUserCommand(userId, 4), cancellationToken);
                // добавляем пользователю ингредиенты яичницы
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    userId, InventoryCategory.Product, 1), cancellationToken);
            }

            if (nextStep == TutorialStep.TransitToCastle)
            {
                // выдаем пользователю особые тыквенные пироги на первое время
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    userId, InventoryCategory.Food, 79, 30), cancellationToken);
            }

            var embed = new EmbedBuilder()
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.Tutorial), cancellationToken))
                // выводим название шага обучения
                .WithAuthor(nextStep.Name())
                // выводим описание шага обучения
                .WithDescription(nextStep.Description());

            await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(userId), cancellationToken), embed),
                cancellationToken);

            return new Unit();
        }
    }
}
