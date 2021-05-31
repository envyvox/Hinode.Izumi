using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.ContractJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.ContractService.Commands;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ContractCommands.ContractAcceptCommand
{
    [InjectableService]
    public class ContractAcceptCommand : IContractAcceptCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ContractAcceptCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long contractId)
        {
            // получаем информацию о контракте с таким номером
            var contract = await _mediator.Send(new GetContractQuery(contractId));
            // получаем текущую локацию пользователя
            var userLocation = await _mediator.Send(new GetUserLocationQuery((long) context.User.Id));

            // проверяем находится ли пользователь в локации контракта
            if (userLocation != contract.Location)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ContractWrongLocation.Parse(
                    contract.Location.Localize(true))));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем текущее время
                var timeNow = DateTimeOffset.Now;
                // получаем репутацию в этой локации
                var reputationType = await _mediator.Send(new GetReputationByLocationQuery(userLocation));
                // получаем пользователя из базы
                var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                // определяем длительность контракта
                var contractTime = await _mediator.Send(new GetActionTimeQuery(contract.Time, user.Energy));

                // обновляем текущую локацию пользователя
                await _mediator.Send(new UpdateUserLocationCommand((long) context.User.Id, Location.WorkOnContract));
                // добавляем информацию о перемещении
                await _mediator.Send(new CreateUserMovementCommand(
                    (long) context.User.Id, Location.WorkOnContract, userLocation, timeNow.AddHours(contractTime)));
                // добавляем информацию о том, что пользователь выполняет контракт
                await _mediator.Send(new AddContractToUserCommand((long) context.User.Id, contractId));
                // отнимаем энергию у пользователя
                await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id, contract.Energy));

                // запускаем джобу для окончания работы по контракту
                var jobId = BackgroundJob.Schedule<IContractJob>(x =>
                        x.Execute((long) context.User.Id, contractId),
                    TimeSpan.FromHours(contractTime));
                await _mediator.Send(new CreateUserHangfireJobCommand(
                    (long) context.User.Id, HangfireAction.Contract, jobId));

                var embed = new EmbedBuilder()
                    .WithAuthor(contract.Name)
                    // баннер контрактов
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Contracts)))
                    // подтверждаем успешное начало работы по контракту
                    .WithDescription(
                        IzumiReplyMessage.ContractAcceptDesc.Parse() +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // ожидаемая награда
                    .AddField(IzumiReplyMessage.ContractAcceptRewardFieldName.Parse(),
                        IzumiReplyMessage.ContractRewardFieldDesc.Parse(
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), contract.Currency,
                            _local.Localize(Currency.Ien.ToString(), contract.Currency),
                            emotes.GetEmoteOrBlank(reputationType.Emote(long.MaxValue)), contract.Reputation,
                            contract.Location.Localize(true)))
                    // длительность
                    .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                        contractTime.Hours().Humanize(1, new CultureInfo("ru-RU")));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
