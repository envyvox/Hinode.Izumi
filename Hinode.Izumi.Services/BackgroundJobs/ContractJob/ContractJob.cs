using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.ContractService.Commands;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.ReputationService.Commands;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.ContractJob
{
    [InjectableService]
    public class ContractJob : IContractJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ContractJob(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(long userId, long contractId)
        {
            // получаем информацию о контракте
            var contract = await _mediator.Send(new GetContractQuery(contractId));
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // определяем репутацию в этой локации
            var reputationType = await _mediator.Send(new GetReputationByLocationQuery(contract.Location));

            // удаляем информацию о том, что пользователь работает по контракту
            await _mediator.Send(new RemoveContractFromUserCommand(userId));
            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, contract.Location));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));

            // добавляем пользователю валюту за выполнение контракта
            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), contract.Currency));
            // добавляем пользователю репутацию за выполнение контракта
            await _mediator.Send(new AddReputationToUserCommand(userId, reputationType, contract.Reputation));
            // добавляем пользователю статистику выполненных контрактов
            await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Contracts));
            // проверяем выполнил ли пользователь достижение
            await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.FirstContract));

            var embed = new EmbedBuilder()
                // название контракта
                .WithAuthor(contract.Name)
                // подтверждаем завершение работы по контракту
                .WithDescription(
                    IzumiReplyMessage.ContractCompletedDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // награда за выполнение контракта
                .AddField(IzumiReplyMessage.ContractCompletedRewardFieldName.Parse(),
                    IzumiReplyMessage.ContractRewardFieldDesc.Parse(emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        contract.Currency, _local.Localize(Currency.Ien.ToString(), contract.Currency),
                        emotes.GetEmoteOrBlank(reputationType.Emote(long.MaxValue)), contract.Reputation,
                        contract.Location.Localize(true)));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            await Task.CompletedTask;
        }
    }
}
