using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ContractCommands.ContractListCommand
{
    [InjectableService]
    public class ContractListCommand : IContractListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ContractListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем пользователя
            var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
            // получаем доступные в этой локации контракты
            var contracts = await _mediator.Send(new GetContractsInLocationQuery(user.Location));

            var embed = new EmbedBuilder()
                // баннер контрактов
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Contracts)))
                // рассказываем как принять контракт
                .WithDescription(IzumiReplyMessage.ContractListDesc.Parse())
                // предупреждаем что во время выполнения контракта нельзя будет заниматься чем-то еще
                .WithFooter(IzumiReplyMessage.ContractListFooter.Parse());

            // создаем embed field для каждого контракта
            foreach (var contract in contracts)
            {
                // получаем репутацию в этой локации
                var reputationType = await _mediator.Send(new GetReputationByLocationQuery(contract.Location));
                // определяем длительность контракта
                var contractTime = await _mediator.Send(new GetActionTimeQuery(contract.Time, user.Energy));
                // заполняем длительность контракта
                var contractTimeString = contractTime == contract.Time
                    ? contract.Time.Hours().Humanize(1, new CultureInfo("ru-RU"))
                    : $"~~{contract.Time.Hours().Humanize(1, new CultureInfo("ru-RU"))}~~ {contractTime.Hours().Humanize(1, new CultureInfo("ru-RU"))}";

                // заполняем информацию о контракте
                embed.AddField($"{emotes.GetEmoteOrBlank("List")} `{contract.Id}` {contract.Name}",
                    IzumiReplyMessage.ContractListFieldDesc.Parse(
                        contract.Description, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), contract.Currency,
                        _local.Localize(Currency.Ien.ToString(), contract.Currency),
                        emotes.GetEmoteOrBlank(reputationType.Emote(long.MaxValue)), contract.Reputation,
                        contract.Location.Localize(true), contractTimeString));
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand((long) context.User.Id, TutorialStep.CheckContracts));
            await Task.CompletedTask;
        }
    }
}
