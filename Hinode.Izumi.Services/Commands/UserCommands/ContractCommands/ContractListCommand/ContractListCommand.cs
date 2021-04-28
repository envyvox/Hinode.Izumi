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
using Hinode.Izumi.Services.RpgServices.ContractService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
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

        public ContractListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocationService locationService, IContractService contractService, IImageService imageService,
            IReputationService reputationService, ITrainingService trainingService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _locationService = locationService;
            _contractService = contractService;
            _imageService = imageService;
            _reputationService = reputationService;
            _trainingService = trainingService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущую локацию пользователя
            var userLocation = await _locationService.GetUserLocation((long) context.User.Id);
            // получаем доступные в этой локации контракты
            var contracts = await _contractService.GetContract(userLocation);

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
                // заполняем информацию о контракте
                embed.AddField($"{emotes.GetEmoteOrBlank("List")} `{contract.Id}` {contract.Name}",
                    IzumiReplyMessage.ContractListFieldDesc.Parse(
                        contract.Description, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), contract.Currency,
                        _local.Localize(Currency.Ien.ToString(), contract.Currency),
                        emotes.GetEmoteOrBlank(reputationType.Emote(long.MaxValue)), contract.Reputation,
                        contract.Location.Localize(true), contract.Time.Hours().Humanize(1, new CultureInfo("ru-RU"))));
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckContracts);
            await Task.CompletedTask;
        }
    }
}
