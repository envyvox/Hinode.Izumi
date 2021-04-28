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
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ContractService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractAcceptCommand
{
    [InjectableService]
    public class ContractAcceptCommand : IContractAcceptCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocationService _locationService;
        private readonly IReputationService _reputationService;
        private readonly IImageService _imageService;
        private readonly IContractService _contractService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _local;
        private readonly ICalculationService _calc;
        private readonly TimeZoneInfo _timeZoneInfo;

        public ContractAcceptCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocationService locationService, IReputationService reputationService, IImageService imageService,
            IContractService contractService, IUserService userService, ILocalizationService local,
            ICalculationService calc, TimeZoneInfo timeZoneInfo)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _locationService = locationService;
            _reputationService = reputationService;
            _imageService = imageService;
            _contractService = contractService;
            _userService = userService;
            _local = local;
            _calc = calc;
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task Execute(SocketCommandContext context, long contractId)
        {
            // получаем информацию о контракте с таким номером
            var contract = await _contractService.GetContract(contractId);
            // получаем текущую локацию пользователя
            var userLocation = await _locationService.GetUserLocation((long) context.User.Id);

            // проверяем находится ли пользователь в локации контракта
            if (userLocation != contract.Location)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ContractWrongLocation.Parse(
                    contract.Location.Localize(true))));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем текущее время
                var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
                // получаем репутацию в этой локации
                var reputationType = _reputationService.GetReputationByLocation(userLocation);
                // получаем пользователя из базы
                var user = await _userService.GetUser((long) context.User.Id);
                // определяем длительность контракта
                var contractTime = _calc.ActionTime(contract.Time, user.Energy);

                // обновляем текущую локацию пользователя
                await _locationService.UpdateUserLocation((long) context.User.Id, Location.WorkOnContract);
                // добавляем информацию о перемещении
                await _locationService.AddUserMovement(
                    (long) context.User.Id, Location.WorkOnContract, userLocation, timeNow.AddHours(contractTime));
                // добавляем информацию о том, что пользователь выполняет контракт
                await _contractService.AddContractToUser((long) context.User.Id, contractId);
                // отнимаем энергию у пользователя
                await _userService.RemoveEnergyFromUser((long) context.User.Id, contract.Energy);

                // запускаем джобу для окончания работы по контракту
                BackgroundJob.Schedule<IContractJob>(x =>
                        x.Execute((long) context.User.Id, contractId),
                    TimeSpan.FromHours(contractTime));

                var embed = new EmbedBuilder()
                    .WithAuthor(contract.Name)
                    // баннер контрактов
                    .WithImageUrl(await _imageService.GetImageUrl(Image.Contracts))
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

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
