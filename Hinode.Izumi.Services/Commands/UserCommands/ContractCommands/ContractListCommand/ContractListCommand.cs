using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ContractService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ContractCommands.ContractListCommand
{
    [InjectableService]
    public class ContractListCommand : IContractListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocationService _locationService;
        private readonly IContractService _contractService;
        private readonly IImageService _imageService;
        private readonly IReputationService _reputationService;
        private readonly ITrainingService _trainingService;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;
        private readonly ICalculationService _calc;

        public ContractListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocationService locationService, IContractService contractService, IImageService imageService,
            IReputationService reputationService, ITrainingService trainingService, ILocalizationService local,
            IUserService userService, ICalculationService calc)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _locationService = locationService;
            _contractService = contractService;
            _imageService = imageService;
            _reputationService = reputationService;
            _trainingService = trainingService;
            _local = local;
            _userService = userService;
            _calc = calc;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя
            var user = await _userService.GetUser((long) context.User.Id);
            // получаем доступные в этой локации контракты
            var contracts = await _contractService.GetContract(user.Location);

            var embed = new EmbedBuilder()
                // баннер контрактов
                .WithImageUrl(await _imageService.GetImageUrl(Image.Contracts))
                // рассказываем как принять контракт
                .WithDescription(IzumiReplyMessage.ContractListDesc.Parse())
                // предупреждаем что во время выполнения контракта нельзя будет заниматься чем-то еще
                .WithFooter(IzumiReplyMessage.ContractListFooter.Parse());

            // создаем embed field для каждого контракта
            foreach (var contract in contracts)
            {
                // получаем репутацию в этой локации
                var reputationType = _reputationService.GetReputationByLocation(contract.Location);
                // определяем длительность контракта
                var contractTime = _calc.ActionTime(contract.Time, user.Energy);
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

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckContracts);
            await Task.CompletedTask;
        }
    }
}
