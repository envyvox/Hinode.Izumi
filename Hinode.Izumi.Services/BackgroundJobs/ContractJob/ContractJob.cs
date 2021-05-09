using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.ContractService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.StatisticService;

namespace Hinode.Izumi.Services.BackgroundJobs.ContractJob
{
    [InjectableService]
    public class ContractJob : IContractJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly IReputationService _reputationService;
        private readonly ILocationService _locationService;
        private readonly ILocalizationService _local;
        private readonly IContractService _contractService;
        private readonly IInventoryService _inventoryService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;

        public ContractJob(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IEmoteService emoteService, IReputationService reputationService, ILocationService locationService,
            ILocalizationService local, IContractService contractService, IInventoryService inventoryService,
            IStatisticService statisticService, IAchievementService achievementService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _reputationService = reputationService;
            _locationService = locationService;
            _local = local;
            _contractService = contractService;
            _inventoryService = inventoryService;
            _statisticService = statisticService;
            _achievementService = achievementService;
        }

        public async Task Execute(long userId, long contractId)
        {
            // получаем информацию о контракте
            var contract = await _contractService.GetContract(contractId);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // определяем репутацию в этой локации
            var reputationType = _reputationService.GetReputationByLocation(contract.Location);

            // удаляем информацию о том, что пользователь работает по контракту
            await _contractService.RemoveContractFromUser(userId);
            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, contract.Location);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);

            // добавляем пользователю валюту за выполнение контракта
            await _inventoryService.AddItemToUser(
                userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), contract.Currency);
            // добавляем пользователю репутацию за выполнение контракта
            await _reputationService.AddReputationToUser(userId, reputationType, contract.Reputation);
            // добавляем пользователю статистику выполненных контрактов
            await _statisticService.AddStatisticToUser(userId, Statistic.Contracts);
            // проверяем выполнил ли пользователь достижение
            await _achievementService.CheckAchievement(userId, Achievement.FirstContract);

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

            await _discordEmbedService.SendEmbed(await _discordGuildService.GetSocketUser(userId), embed);
            await Task.CompletedTask;
        }
    }
}
