using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserProfileCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserProfileCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("профиль"), Alias("profile")]
        [Summary("Посмотреть свой профиль или профиль пользователя с указанным именем")]
        [CommandUsage("!профиль", "!профиль Холли")]
        public async Task UserProfileTask(
            [Summary("Игровое имя")] [Remainder] string name = null) =>
            await SendProfileEmbed(name is not null
                // если пользователь указал игровое имя в команде, нужно вывести профиль желаемого пользователя
                ? await _mediator.Send(new GetUserWithRowNumberByNamePatternQuery(name))
                // если нет - его собственный профиль
                : await _mediator.Send(new GetUserWithRowNumberByIdQuery((long) Context.User.Id)));

        [Command("профиль"), Alias("profile")]
        [Summary("Посмотреть профиль пользователя с указанным ID")]
        [CommandUsage("!профиль 550493599629049858")]
        public async Task UserProfileTask(
            [Summary("ID пользователя")] long userId) =>
            await SendProfileEmbed(
                await _mediator.Send(new GetUserWithRowNumberByIdQuery(userId)));

        private async Task SendProfileEmbed(UserWithRowNumberRecord user)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем репутации пользователя
            var userReputations = await _mediator.Send(new GetUserReputationsQuery(user.Id));
            // получаем массив доступных репутаций
            var reputations = Enum.GetValues(typeof(Reputation)).Cast<Reputation>().ToArray();
            // получаем среднее значение репутаций пользователя
            var userAverageReputation =
                reputations.Sum(reputation =>
                    userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0) /
                reputations.Length;
            // определяем репутационный статус по среднему значению репутаций пользователя
            var userReputationStatus = ReputationStatusHelper.GetReputationStatus(userAverageReputation);
            // получаем информацию о передвижениях пользователя
            var userMovement = await _mediator.Send(new GetUserMovementQuery(user.Id));
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(user.Id));
            // получаем активный баннер пользователя
            var userBanner = await _mediator.Send(new GetUserActiveBannerQuery(user.Id));
            // получаем пользователя в дискорде для того чтобы взять его аватарку и имя аккаунта
            var socketUser = await _mediator.Send(new GetDiscordSocketUserQuery(user.Id));
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
                    var userContract = await _mediator.Send(new GetUserContractQuery(user.Id));
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

            var embed = new EmbedBuilder()
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
                    $"{await _mediator.Send(new DisplayProgressBarCommand(user.Energy))} {emotes.GetEmoteOrBlank("Energy")} {user.Energy} {_local.Localize("Energy", user.Energy)}")

                // текущая локация
                .AddField(IzumiReplyMessage.ProfileCurrentLocationTitle.Parse(),
                    locationString + $"\n{emotes.GetEmoteOrBlank("Blank")}")

                // рейтинг приключений
                .AddField(IzumiReplyMessage.UserProfileRatingFieldName.Parse(),
                    IzumiReplyMessage.UserProfileRatingFieldDesc.Parse(
                        await _mediator.Send(new DisplayRowNumberQuery(user.RowNumber)), user.RowNumber, user.Points,
                        _local.Localize("AdventurePoints", user.Points)))

                // репутационный рейтинг
                .AddField(IzumiReplyMessage.UserProfileRepRatingFieldName.Parse(),
                    IzumiReplyMessage.UserProfileRepRatingFieldDesc.Parse(
                        userReputationStatus.Localize(),
                        emotes.GetEmoteOrBlank(ReputationStatusHelper.Emote(userAverageReputation)),
                        userAverageReputation) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")

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

                // дата регистрации и количество дней в игровом мире
                .AddField(IzumiReplyMessage.ProfileRegistrationDateTitle.Parse(),
                    IzumiReplyMessage.ProfileRegistrationDateDesc.Parse(
                        registrationDate, daysInGame))

                // информация пользователя
                .AddField(IzumiReplyMessage.ProfileAboutMeTitle.Parse(),
                    user.About ?? IzumiReplyMessage.ProfileAboutMeNull.Parse())

                // активный баннер пользователя
                .WithImageUrl(userBanner.Url);

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand((long) Context.User.Id, TutorialStep.CheckProfile));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локалазированную строку названия семьи и статуса пользователя в ней.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Локалазированную строка названия семьи и статуса пользователя в ней.</returns>
        private async Task<string> DisplayUserFamily(long userId)
        {
            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
            // получаем семью
            var family = await _mediator.Send(new GetFamilyByIdQuery(userFamily.FamilyId));
            // возвращаем локалазированную строку
            return $"{family.Name}, {userFamily.Status.Localize()}";
        }
    }
}
