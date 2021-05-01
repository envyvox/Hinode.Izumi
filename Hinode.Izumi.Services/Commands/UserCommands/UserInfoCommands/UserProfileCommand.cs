using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BannerService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ContractService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Hinode.Izumi.Services.RpgServices.UserService.Models;
using Humanizer;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserProfileCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IBannerService _bannerService;
        private readonly IReputationService _reputationService;
        private readonly ITrainingService _trainingService;
        private readonly ILocationService _locationService;
        private readonly ILocalizationService _local;
        private readonly ICalculationService _calc;
        private readonly IContractService _contractService;
        private readonly IFamilyService _familyService;

        public UserProfileCommand(IDiscordEmbedService discordEmbedService, IUserService userService,
            IEmoteService emoteService, IDiscordGuildService discordGuildService, IBannerService bannerService,
            IReputationService reputationService, ITrainingService trainingService, ILocationService locationService,
            ILocalizationService local, ICalculationService calc, IContractService contractService,
            IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _userService = userService;
            _emoteService = emoteService;
            _discordGuildService = discordGuildService;
            _bannerService = bannerService;
            _reputationService = reputationService;
            _trainingService = trainingService;
            _locationService = locationService;
            _local = local;
            _calc = calc;
            _contractService = contractService;
            _familyService = familyService;
        }

        [Command("профиль"), Alias("profile")]
        public async Task UserProfileTask([Remainder] string name = null!)
        {
            var embed = await GetProfileEmbed(name != null
                // если пользователь указал игровое имя в команде, нужно вывести профиль желаемого пользователя
                ? await _userService.GetUser(name)
                // если нет - его собственный профиль
                : await _userService.GetUser((long) Context.User.Id));

            await _discordEmbedService.SendEmbed(Context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.CheckProfile);
            await Task.CompletedTask;
        }

        private async Task<EmbedBuilder> GetProfileEmbed(UserModel user)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем репутации пользователя
            var userReputation = await _reputationService.GetUserReputation(user.Id);
            // получаем среднее значение репутаций пользователя
            var userAverageReputation = userReputation.Count > 0
                ? userReputation.Values.Average(x => x.Amount)
                : 0;
            // определяем репутационный статус по среднему значению репутаций пользователя
            var userReputationStatus = ReputationStatusHelper.GetReputationStatus(userAverageReputation);
            // получаем информацию о передвижениях пользователя
            var userMovement = await _locationService.GetUserMovement(user.Id);
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily(user.Id);
            // получаем активный баннер пользователя
            var userBanner = await _bannerService.GetUserActiveBanner(user.Id);
            // получаем пользователя в дискорде для того чтобы взять его аватарку и имя аккаунта
            var socketUser = await _discordGuildService.GetSocketUser(user.Id);
            // заполняем дату регистрации
            var registrationDate = user.CreatedAt.ToString("dd MMMM yyyy", new CultureInfo("ru-ru"));
            // заполняем количество дней в игровом мире
            var daysInGame = (timeNow - user.CreatedAt).TotalDays.Days().Humanize(1, new CultureInfo("ru-RU"));

            // заполняем строку о текущей локации в зависимости от того, чем пользователь сейчас заниматся
            string locationString;
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (user.Location)
            {
                case Location.InTransit:
                    locationString = IzumiReplyMessage.ProfileCurrentLocationInTransit.Parse(
                        userMovement.Departure.Localize(), userMovement.Destination.Localize(),
                        TimeSpan.FromMinutes(userMovement.Arrival.Subtract(timeNow).TotalMinutes)
                            .Humanize(2, new CultureInfo("ru-RU")));
                    break;
                case Location.WorkOnContract:
                    var userContract = await _contractService.GetUserContract(user.Id);
                    locationString =
                        $@"**{userContract.Name}** в **{userContract.Location.Localize(true)}**, еще {
                            TimeSpan.FromMinutes(userMovement.Arrival.Subtract(timeNow).TotalMinutes)
                                .Humanize(2, new CultureInfo("ru-RU"))}";
                    break;
                case Location.ExploreGarden:
                case Location.ExploreCastle:
                case Location.Fishing:
                case Location.FieldWatering:
                case Location.MakingCrafting:
                case Location.MakingAlcohol:
                case Location.MakingFood:
                case Location.MakingDrink:
                    locationString =
                        $@"**{user.Location.Localize()}**, еще {
                            TimeSpan.FromSeconds(userMovement.Arrival.Subtract(timeNow).TotalSeconds)
                                .Humanize(2, new CultureInfo("ru-RU"))}";
                    break;
                default:
                    locationString = $"**{user.Location.Localize()}**";
                    break;
            }

            return new EmbedBuilder()
                // аватарка пользователя
                .WithThumbnailUrl(socketUser.GetAvatarUrl())
                // титул пользователя, игровое имя и имя аккаунта дискорда
                .WithDescription(IzumiReplyMessage.ProfileTitle.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(),
                    user.Name, socketUser.Username))
                // пол пользователя
                .AddField(IzumiReplyMessage.ProfileGenderTitle.Parse(),
                    $"{emotes.GetEmoteOrBlank(user.Gender.Emote())} {user.Gender.Localize()}" +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}", true)
                // день рождения
                .AddField(IzumiReplyMessage.UserProfileBirthdayFieldName.Parse(),
                    IzumiReplyMessage.UserProfileBirthdayFieldDescNull.Parse(
                        emotes.GetEmoteOrBlank("Blank")), true)
                // энергия пользователя
                .AddField(IzumiReplyMessage.UserProfileEnergyFieldName.Parse(),
                    $"{await _calc.DisplayProgressBar(user.Energy)} {emotes.GetEmoteOrBlank("Energy")} {user.Energy} {_local.Localize("Energy", user.Energy)}")
                // текущая локация
                .AddField(IzumiReplyMessage.ProfileCurrentLocationTitle.Parse(),
                    locationString + $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // семья
                .AddField(IzumiReplyMessage.ProfileFamilyTitle.Parse(),
                    hasFamily
                        // если пользователь состоит в семье - выводим строку с названием семьи и его статусом в ней
                        ? IzumiReplyMessage.ProfileFamilyDesc.Parse(
                            emotes.GetEmoteOrBlank("Blank"), await DisplayUserFamily(user.Id))
                        // если пользователь не состоит в семье - выводим строку о том, что у него нет семьи
                        : IzumiReplyMessage.ProfileFamilyNull.Parse(emotes.GetEmoteOrBlank("Blank")))
                // клан
                .AddField(IzumiReplyMessage.ProfileClanTitle.Parse(),
                    IzumiReplyMessage.ProfileClanNull.Parse(emotes.GetEmoteOrBlank("Blank")) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // репутационный рейтинг
                .AddField(IzumiReplyMessage.UserProfileRepRatingFieldName.Parse(),
                    IzumiReplyMessage.UserProfileRepRatingFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(ReputationStatusHelper.Emote(userAverageReputation)),
                        userAverageReputation, userReputationStatus.Localize()))
                // репутация пользователя
                .AddField(IzumiReplyMessage.ProfileReputationFieldName.Parse(),
                    Enum.GetValues(typeof(Reputation))
                        .Cast<Reputation>()
                        .Aggregate(string.Empty, (current, reputationType) =>
                            current +
                            $"{emotes.GetEmoteOrBlank(reputationType.Emote(userReputation.ContainsKey(reputationType) ? userReputation[reputationType].Amount : 0))} {(userReputation.ContainsKey(reputationType) ? $"{userReputation[reputationType].Amount}" : "0")} в **{reputationType.Location().Localize(true)}**\n") +
                    $"{emotes.GetEmoteOrBlank("Blank")}")
                // дата регистрации и количество дней в игровом мире
                .AddField(IzumiReplyMessage.ProfileRegistrationDateTitle.Parse(),
                    IzumiReplyMessage.ProfileRegistrationDateDesc.Parse(
                        registrationDate, daysInGame))
                // информация пользователя
                .AddField(IzumiReplyMessage.ProfileAboutMeTitle.Parse(),
                    user.About ?? IzumiReplyMessage.ProfileAboutMeNull.Parse())
                // активный баннер пользователя
                .WithImageUrl(userBanner.Url);
        }

        /// <summary>
        /// Возвращает локалазированную строку названия семьи и статуса пользователя в ней.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Локалазированную строка названия семьи и статуса пользователя в ней.</returns>
        private async Task<string> DisplayUserFamily(long userId)
        {
            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily(userId);
            // получаем семью
            var family = await _familyService.GetFamily(userFamily.FamilyId);
            // возвращаем локалазированную строку
            return $"{family.Name}, {userFamily.Status.Localize()}";
        }
    }
}
