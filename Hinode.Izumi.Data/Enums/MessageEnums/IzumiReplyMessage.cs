using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
    public enum IzumiReplyMessage
    {
        LogUserJoined,
        LogUserLeft,
        UsernameNotValid,
        UsernameTaken,
        UsernameNull,
        RegistrationAlready,
        RegistrationSuccessDesc,
        RegistrationSuccessGenderTitle,
        RegistrationSuccessGenderDesc,
        RegistrationSuccessBeginTitle,
        RegistrationSuccessBeginDesc,
        RegistrationSuccessReferralTitle,
        RegistrationSuccessReferralDesc,
        RegistrationSuccessButCantRename,
        ProfileTitle,
        ProfileGenderTitle,
        ProfileRegistrationDateTitle,
        ProfileRegistrationDateDesc,
        ProfileAboutMeTitle,
        ProfileAboutMeNull,
        ProfileCurrentLocationTitle,
        ProfileCurrentLocationInTransit,
        ProfileFamilyTitle,
        ProfileFamilyNull,
        ProfileFamilyDesc,
        ProfileClanTitle,
        ProfileClanNull,
        ReferralSetYourself,
        ReferralSetAlready,
        ReferralSetSuccess,
        ReferralSetNotifyPm,
        UpdateAboutCooldown,
        UpdateAboutNull,
        UpdateAboutMaxLimit,
        UpdateAboutSuccess,
        TransitListDesc,
        TransitCompleteNotify,
        TransitMakeAlready,
        TransitMakeNull,
        TransitMakeCurrent,
        TransitMakeNoCurrency,
        TransitMakeSuccess,
        InventoryDesc,
        InventorySeedOutOfLimit,
        InventoryCropOutOfLimit,
        InventoryFishOutOfLimit,
        InventoryNull,
        UserSeedsDesc,
        UserSeedsSpringFieldName,
        UserSeedsSummerFieldName,
        UserSeedsAutumnFieldName,
        UserCropsDesc,
        UserCropsSpringFieldName,
        UserCropsSummerFieldName,
        UserCropsAutumnFieldName,
        UserFishDesc,
        ExploreGardenBegin,
        ExploreCastleBegin,
        ExploreForestEmpty,
        ExploreCastleEmpty,
        ExploreForestSuccess,
        ExploreCastleSuccess,
        ExploreSuccessFieldName,
        FishingBegin,
        FishingEmpty,
        FishingSuccess,
        GamblingBetCooldown,
        GamblingBetNoAmount,
        GamblingBetNoCurrency,
        GamblingBetMinCurrency,
        GamblingBetMaxCurrency,
        GamblingBetCubeDrop,
        GamblingBetWon,
        GamblingBetLose,
        CapitalSeedShopDesc,
        CapitalSeedShopSeedFieldName,
        CapitalSeedShopSeedDesc,
        CapitalSeedShopSeedMultiple,
        CapitalSeedShopSeedReGrowth,
        MarketBuyDesc,
        MarketBuyFieldName,
        MarketBuyListDesc,
        MarketBuyListDescNull,
        MarketBuyDirectWrongAmount,
        MarketBuyDirectNoCurrency,
        MarketBuyDirectSuccess,
        MarketBuyRequestNoCurrency,
        MarketBuyRequestSuccess,
        MarketSellDesc,
        MarketSellFieldName,
        MarketSellListDesc,
        MarketSellListDescNull,
        MarketSellDirectWrongAmount,
        MarketSellDirectSuccess,
        MarketSellRequestNoCurrency,
        MarketSellRequestSuccess,
        MarketBuyYourself,
        MarketSellYourself,
        MarketBuyNotify,
        MarketSellNotify,
        MarketRequestAlready,
        MarketRequestListDesc,
        MarketRequestList,
        MarketRequestListNull,
        MarketRequestWrongUser,
        MarketRequestSellCancel,
        MarketRequestBuyCancel,
        ShopBuySeedWrongSeason,
        ShopBuyNoCurrency,
        ShopBuySeedSuccess,
        FisherShopDesc,
        FisherShopFishDesc,
        FisherSellWrongSeason,
        FisherSellNoFish,
        FisherSellSuccess,
        FisherMassSellNoFish,
        FisherMassSellSuccessDesc,
        FisherMassSellSuccessFieldName,
        FisherMassSellSuccessFieldDesc,
        FisherMassSellFishLine,
        FieldInfoNullDesc,
        FieldInfoNullFieldName,
        FieldInfoNullFieldDesc,
        FieldInfoHarvestingFieldName,
        FieldInfoHarvestingFieldDesc,
        FieldBuyAlready,
        FieldBuySuccess,
        FieldBuyNoCurrency,
        FieldInfoStateCompletedReGrowth,
        UserFieldNull,
        UserFieldEmpty,
        UserFieldCompleted,
        UserFieldPlantAlready,
        UserFieldPlantOnlyCurrentSeason,
        UserFieldPlantSuccess,
        SeedByLocalizedNameNull,
        UserFieldWaterStart,
        UserFieldWaterSuccess,
        UserFieldCollectNotReady,
        UserFieldCollectSuccess,
        UserFieldCollectSuccessReGrowth,
        UserFieldDigEmpty,
        UserFieldDigSuccess,
        CatchErrorHandle,
        WorldInfoTimeFieldName,
        WorldInfoTimeFieldDesc,
        WorldInfoWeatherTodayFieldName,
        WorldInfoWeatherTodayFieldDesc,
        WorldInfoWeatherTomorrowFieldName,
        WorldInfoWeatherTomorrowFieldDesc,
        WorldInfoSeasonFieldName,
        WorldInfoSeasonFieldDesc,
        WorldInfoStateFieldName,
        WorldInfoStateFieldDesc,
        UpdateAboutMinLimit,
        TemporarilyUnavailable,
        TitleAdded,
        FisherMassSellFishLineOutOfLimit,
        UserFieldWaterNull,
        LotteryWinnerPm,
        LotteryBuyAlready,
        LotteryBuyNoCurrency,
        LotteryBuySuccess,
        LotteryInfoDesc,
        LotteryInfoRulesFieldName,
        LotteryInfoRulesFieldDesc,
        LotteryInfoCurrentMembersFieldName,
        LotteryInfoCurrentMembersFieldDesc,
        LotteryGiftAlreadyHave,
        LotteryGiftNoCurrency,
        LotteryGiftSuccess,
        LotteryGiftSuccessPm,
        LotteryGiftYourself,
        ShopListDesc,
        MarketSelling,
        MarketBuying,
        UserDontHaveSeed,
        InventoryFoodOutOfLimit,
        UserFoodDesc,
        UserCollectionDesc,
        UserCollectionFieldName,
        UserFoodMastery0,
        UserFoodMastery50,
        UserFoodMastery100,
        UserFoodMastery150,
        UserFoodMastery200,
        UserFoodMastery250,
        ProductShopDesc,
        ProductShopFieldName,
        ProductShopBuySuccess,
        ResourceCraftWrongLocation,
        ResourceCraftNoCurrency,
        CraftingListFieldName,
        CraftingListFieldDesc,
        ShopRecipeDesc,
        ShopRecipeFieldNameMastery0,
        ShopRecipeFieldNameMastery50,
        ShopRecipeFieldNameMastery100,
        ShopRecipeFieldNameMastery150,
        ShopRecipeFieldNameMastery200,
        ShopRecipeFieldNameMastery250,
        ShopRecipeFieldDesc,
        MarketNotAllowedGroup,
        ResourceByLocalizedNameNull,
        CropByLocalizedNameNull,
        FoodByLocalizedNameNull,
        CookingNoCurrency,
        CookingListFieldName,
        CookingListFieldDesc,
        CookingListWrongMasteryBracket,
        CookingListNull,
        CookingListCategoryDesc,
        CookingListCategoryFieldName,
        CookingListCategoryFieldDesc,
        CookingListDesc,
        UserCollectionWrongGroup,
        RecipeBuyAlready,
        RecipeBuyNoCurrency,
        RecipeBuySuccess,
        UserEffectsDesc,
        UserEffectsHelpFieldName,
        UserEffectsHelpFieldDesc,
        EatFoodSuccess,
        EatFoodWrongAmount,
        UserEffectGroupAlready,
        ProfileReputationFieldName,
        ShopListFieldName,
        ShopListFieldDesc,
        WorldInfoDebuffFieldName,
        WorldInfoDebuffFieldDesc,
        BossDebuffActive,
        AchievementAdded,
        AchievementGroupsDesc,
        AchievementGroupsFieldName,
        UserFamilyNull,
        UserFamilyStatusRequireHead,
        UserNotInYourFamily,
        FamilyKickUserSuccess,
        FamilyKickUserSuccessNotify,
        FamilySetUserStatusCantBeHead,
        FamilySetUserStatusSuccess,
        FamilySetUserStatusSuccessNotify,
        FamilyInviteNull,
        UserFamilyNotNull,
        UserFamilyAlready,
        FamilyInviteAcceptSuccess,
        FamilyInviteAcceptSuccessNotify,
        FamilyInviteCancelSuccess,
        FamilyInviteCancelSuccessNotify,
        FamilyInviteListFamilyNullDesc,
        FamilyInviteListFamilyNotNullDesc,
        FamilyInviteListFieldName,
        FamilyInviteListFieldDescNull,
        FamilyInviteListFamilyNullFieldDesc,
        FamilyInviteListFamilyNotNullFieldDesc,
        FamilyInviteSendSuccess,
        FamilyInviteSendSuccessNotify,
        FamilyInviteDeclineSuccess,
        FamilyInviteDeclineSuccessNotify,
        FamilyDeleteSuccess,
        FamilyUpdateDescriptionMaxLength,
        FamilyUpdateDescriptionSuccess,
        FamilyStatusRegistration,
        FamilyKickYourself,
        FamilySetUserStatusYourself,
        FamilyInviteSendAlready,
        FamilyInfoUserFamilyNull,
        FamilyInfoDesc,
        FamilyInfoStatusRegistrationFieldName,
        FamilyInfoStatusRegistrationFieldDesc,
        FamilyInfoDescriptionFieldName,
        FamilyInfoMembersFieldName,
        RequireCert,
        FamilyNameNotValid,
        FamilyNameTaken,
        FamilyRegistrationSuccess,
        FamilyRegistrationCompleted,
        CapitalCertShopDesc,
        CapitalCertShopFieldName,
        CapitalCertShopFieldDesc,
        ShopBuyCertAlready,
        ShopBuyCertSuccess,
        RenameSuccess,
        CertRemoved,
        FamilyRenameSuccess,
        FamilyListDesc,
        FamilyListFieldName,
        FamilyListNull,
        MarketRequestGroupLimit,
        MarketRequestMinCost,
        FieldEmptyFieldName,
        FieldEmptyFieldDesc,
        FieldNeedWatering,
        FieldDontNeedWatering,
        FieldProgress,
        FieldCompletedFieldName,
        FieldCompletedFieldDesc,
        PresetGameRolesAuthor,
        PresetGameRolesDesc,
        PresetGameRolesFieldName,
        PresetGameRolesFieldDesc,
        PresetRolesFooter,
        PresetRegistryAnonsRolesFieldName,
        PresetRegistryAnonsRolesFieldDesc,
        ShopProjectDesc,
        ProjectPlanShopFieldDesc,
        ShopFieldDescNull,
        ShopBuyProjectSuccess,
        ShopBuyProjectAlreadyHaveProject,
        ShopBuyProjectAlreadyHaveBuilding,
        UserCertsDesc,
        UserCertsFooter,
        UserCertsFieldName,
        UserProjectsDesc,
        UserProjectsFooter,
        UserProjectsFieldName,
        UserProjectsFieldDesc,
        UserProjectsIngredientsNull,
        BuildingListDesc,
        BuildingListPersonalBuildingsNull,
        BuildingListFamilyBuildingsNull,
        BuildingListClanBuildingsNull,
        FamilyClanNull,
        BuildNoCurrency,
        BuildStartedSuccess,
        BuildRequirePersonalBuildingButNull,
        BuildPersonalBuildingAlready,
        BuildRequireUserFamilyButNull,
        BuildRequireUserFamilyStatusHeadButLower,
        BuildRequireFamilyBuildingButNull,
        BuildFamilyBuildingAlready,
        BuildRequireClanFamilyButNull,
        BuildRequireClanFamilyStatusOwnerButLower,
        BuildRequireClanBuildingButNull,
        BuildClanBuildingAlready,
        BuildCompleted,
        TransitListFieldName,
        TransitListFieldDesc,
        ContractListDesc,
        ContractListFooter,
        ContractListFieldDesc,
        UserProfileRepRatingFieldName,
        UserProfileRepRatingFieldDesc,
        UserProfileBirthdayFieldName,
        UserProfileBirthdayFieldDescNull,
        ContractAcceptDesc,
        ContractAcceptRewardFieldName,
        ContractRewardFieldDesc,
        TimeFieldName,
        ContractCompletedDesc,
        ContractCompletedRewardFieldName,
        ContractWrongLocation,
        ExploreRewardFieldName,
        ExploreRewardFishingFieldDesc,
        TransitCompleteInfoChannelsFieldName,
        PresetRegistryNicknameTitle,
        PresetRegistryNicknameDesc,
        PresetRegistryCommandTitle,
        PresetRegistryCommandDesc,
        PresetRegistryAnonsRolesTitle,
        PresetRegistryAnonsRolesDesc,
        UpdateGenderAlready,
        UpdateGenderDesc,
        UpdateGenderNotifyDesc,
        UpdateGenderNotifyFieldName,
        UpdateGenderNotifyFieldDesc,
        ModGenderDesc,
        ModGenderNotifyDesc,
        UserFieldWaterFamilyMemberOnly,
        NoRequiredIngredientAmount,
        CraftingAlcoholDesc,
        CraftingAlcoholInFamilyHouse,
        CraftingAlcoholExpectedFieldName,
        CraftingAlcoholCompleteDesc,
        CraftingAlcoholReceivedFieldName,
        CraftingResourceDesc,
        CraftingResourceInFamilyHouse,
        CraftingResourceExpectedFieldName,
        CraftingResourceCompleteDesc,
        CraftingResourceReceivedFieldName,
        CraftingFoodDesc,
        CraftingFoodInFamilyHouse,
        CraftingFoodExpectedFieldName,
        CraftingFoodReceivedFieldName,
        CraftingFoodCompleteDesc,
        IngredientsSpent,
        UserTitleListDesc,
        UserTitleListFieldName,
        UserUpdateTitleDontHave,
        UserUpdateTitleAlready,
        UserUpdateTitleSuccess,
        ShopBannerDesc,
        ShopBannerFieldDesc,
        DynamicShopDesc,
        DynamicShopFooter,
        ShopBuyBannerAlready,
        ShopBuyBannerSuccess,
        UserBannerListDesc,
        UserBannerListFieldName,
        UserBannerUpdateAlready,
        UserBannerUpdateSuccess,
        UserCardListDesc,
        UserCardListFooter,
        UserCardListLengthLessThen1FieldName,
        UserCardListLengthLessThen1FieldDesc,
        UserCardListLengthMoreThen15FieldName,
        UserCardListLengthMoreThen15FieldDesc,
        CardDetailedDesc,
        UserDeckListDesc,
        UserDeckListFooter,
        UserDeckListLengthLessThen1FieldName,
        UserDeckListLengthLessThen1FieldDesc,
        UserDeckRemoveNotInDeck,
        UserDeckRemoveSuccess,
        UserDeckAddAlreadyInDeck,
        UserDeckAddLengthMoreThen5,
        UserDeckAddSuccess,
        ProjectInfoDesc,
        ProjectInfoRequireFieldName,
        ProjectInfoIngredientsFieldName,
        ProjectInfoBuildingCostFieldName,
        ProjectInfoTitle,
        CardInfoIdFieldName,
        CardInfoRarityFieldName,
        CardInfoAnimeFieldName,
        CardInfoNameFieldName,
        MarketInfoDesc,
        MarketInfoBuyFieldName,
        MarketInfoBuyFieldDesc,
        MarketInfoSellFieldName,
        MarketInfoSellFieldDesc,
        MarketInfoRequestFieldName,
        MarketInfoRequestFieldDesc,
        MarketInfoGroupsFieldName,
        FamilyInviteListCantWatch,
        TransitCostFieldName,
        ReferralRewardFieldName,
        ReferralListDesc,
        ReferralListReferrerFieldName,
        ReferralListReferralsFieldName,
        ReferralListReferrerNull,
        ReferralListReferralsNull,
        ReferralListReferralsOutOfLimit,
        FamilyInfoDescriptionNull,
        FamilyInfoCurrencyFieldName,
        UserFamilyStatusRequireNotDefault,
        FamilyCurrencyAddUserNoCurrency,
        FamilyCurrencyTakeFamilyNoCurrency,
        FamilyCurrencyAddSuccess,
        FamilyCurrencyTakeSuccess,
        UserProfileEnergyFieldName,
        UserMasteryDesc,
        UserMasteryFieldName,
        MarketRequestInfo,
        MarketRequestListNullFieldName,
        MarketRequestListNullFieldDesc,
        MarketUserRequestListNullFieldName,
        MarketUserRequestListNullFieldDesc,
        MarketUserRequestFieldName,
        UserProjectNull,
        CraftingItemListDesc,
        CraftingAlcoholListDesc,
        CraftingAlcoholInfoDesc,
        IngredientsFieldName,
        CraftingPriceFieldName,
        LocationFieldName,
        CraftingItemInfoDesc
    }

    public static class IzumiReplyMessageHelper
    {
        public static string Parse(this IzumiReplyMessage message) => message.Localize();

        public static string Parse(this IzumiReplyMessage message, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(message), replacements);
            }
            catch
            {
                return "`Возникла ошибка вывода ответа. Пожалуйста, покажите это Холли.`";
            }
        }

        private static string Localize(this IzumiReplyMessage message) => message switch
        {
            IzumiReplyMessage.LogUserJoined =>
                "Пользователь {0} {1} присоединился к серверу.",

            IzumiReplyMessage.LogUserLeft =>
                "Пользователь {0} {1} покинул сервер.",

            IzumiReplyMessage.UsernameNull =>
                "Вы не указали игровое имя, попробуйте еще раз.",

            IzumiReplyMessage.UsernameNotValid =>
                "Игровое имя **{0}** не проходит проверку.",

            IzumiReplyMessage.UsernameTaken =>
                "Игровое имя **{0}** уже занято.",

            IzumiReplyMessage.RegistrationAlready =>
                "Вы ведь уже зарегистрированы в игровом мире.",

            IzumiReplyMessage.RegistrationSuccessDesc =>
                "Вы успешно зарегистрировались в игровом мире под именем **{0}**.\nЯ изменила ваш никнейм на сервере на **{0}** с эмоджи 🌺, отображающий что вы - зарегистрированный пользователь.\n\nТак же, вам явно пригодятся деньги на первое время, так что я выделила для вас {1} {2} иен.",

            IzumiReplyMessage.RegistrationSuccessGenderTitle =>
                "Подтверждение вашего гендера",

            IzumiReplyMessage.RegistrationSuccessGenderDesc =>
                "По-умолчанию в вашем профиле отображается что ваш пол - {0} не выбран.\nВы можете попросить модерацию сервера, **Родзю**, подтвердить его через команду `!подтвердить пол`, после чего вас пригласят в голосовой канал для быстрой беседы.\n\n*Данная процедура не является обязательной, вы можете скрывать свой пол при желании.*",

            IzumiReplyMessage.RegistrationSuccessBeginTitle =>
                "Начало",

            IzumiReplyMessage.RegistrationSuccessBeginDesc =>
                "Предлагаю вам пройти `!обучение`, где в коротком, но очень познавательном, путешевствии вы узнаете о мире **Hinode** и чем вы могли бы заняться.\n\nВ процессе обучения вам придется потратить {0} {1} иену, однако в процессе прохождения вы получите особую карточку «**Тони Тони Чоппер**» и {0} {2} иен!",

            IzumiReplyMessage.RegistrationSuccessReferralTitle =>
                "Реферальная система",

            IzumiReplyMessage.RegistrationSuccessReferralDesc =>
                "Если вас пригласил пользователь - вам определенно стоит написать\n`!пригласил [игровое имя]`, чтобы получить {0} бонусы.",

            IzumiReplyMessage.ProfileTitle =>
                "{0} {1} **{2}** `@{3}`",

            IzumiReplyMessage.ProfileGenderTitle =>
                "Пол",

            IzumiReplyMessage.ProfileRegistrationDateTitle =>
                "Дата регистрации",

            IzumiReplyMessage.ProfileAboutMeTitle =>
                "Информация",

            IzumiReplyMessage.ProfileAboutMeNull =>
                "На данный момент тут пусто ;c",

            IzumiReplyMessage.ReferralSetYourself =>
                "Вы не можете указать самого себя в качестве реферера.",

            IzumiReplyMessage.ReferralSetAlready =>
                "Вы уже указали {0} {1} **{2}** как своего реферера.",

            IzumiReplyMessage.ReferralSetSuccess =>
                "Отлично, {0} {1} **{2}** теперь ваш реферер.",

            IzumiReplyMessage.ReferralSetNotifyPm =>
                "{0} {1} **{2}** указал вас своим реферером.",

            IzumiReplyMessage.RegistrationSuccessButCantRename =>
                "@everyone, Я не смогла изменить имя на сервере пользователю {0}, однако его игровое имя теперь `{1}`.",

            IzumiReplyMessage.ProfileCurrentLocationTitle =>
                "Текущая локация",

            IzumiReplyMessage.ProfileRegistrationDateDesc =>
                "{0}, **{1}** в игровом мире",

            IzumiReplyMessage.UpdateAboutNull =>
                "Вы не указали новый текст информации о себе, попробуйте еще раз.",

            IzumiReplyMessage.UpdateAboutSuccess =>
                "Вы успешно обновили раздел информации о себе в профиле.",

            IzumiReplyMessage.UpdateAboutMaxLimit =>
                "Указанный вами текст превышает лимит в 1024 знака, попробуйте описать себя в более упрощенном виде ;)",

            IzumiReplyMessage.UpdateAboutCooldown =>
                "К сожалению, обновление раздела информации о себе возможно лишь раз в {0}. Попробуйте еще раз через {1}.",

            IzumiReplyMessage.TransitListDesc =>
                "Напишите `!отправиться [номер]` для перемещения в другую локацию.\n\n*Длительность перемещения зависит от вашей {0} энергии, а стоимость от мастерства {1} «{2}».*",

            IzumiReplyMessage.TransitCompleteNotify =>
                "Вы достигли точки прибытия, добро пожаловать в **{0}**!",

            IzumiReplyMessage.TransitMakeNull =>
                "Вы не указали куда хотите отправиться.\nЗагляните в `!отправления` и попробуйте еще раз.",

            IzumiReplyMessage.TransitMakeCurrent =>
                "Вы уже находитесь в данной локации, не нужно никуда отправляться.",

            IzumiReplyMessage.TransitMakeNoCurrency =>
                "Для оплаты этого транспорта необходимо {0} {1} иен, которых у вас нет.",

            IzumiReplyMessage.TransitMakeSuccess =>
                "Вы отправляетесь в **{2}**, хорошей дороги!",

            IzumiReplyMessage.TransitMakeAlready =>
                "Вы находитесь в пути и не можете отправиться в другую локацию до прибытия в **{0}**.",

            IzumiReplyMessage.ProfileCurrentLocationInTransit =>
                "В пути из **{0}** в **{1}**, до прибытия {2}",

            IzumiReplyMessage.InventoryDesc =>
                "Все имеющиеся вещи попадают сюда:",

            IzumiReplyMessage.InventoryNull =>
                "У вас нет ни одного предмета этого типа",

            IzumiReplyMessage.ProfileFamilyTitle =>
                "Семья",

            IzumiReplyMessage.ProfileFamilyNull =>
                "{0} Не состоит в семье",

            IzumiReplyMessage.ProfileFamilyDesc =>
                "{0} {1}",

            IzumiReplyMessage.ProfileClanTitle =>
                "Клан",

            IzumiReplyMessage.ProfileClanNull =>
                "{0} Семья не состоит в клане",

            IzumiReplyMessage.InventorySeedOutOfLimit =>
                "У вас слишком много семян для отображения в инвентаре, загляните в `!инвентарь семена` чтобы просмотреть их",

            IzumiReplyMessage.InventoryCropOutOfLimit =>
                "У вас слишком много собранного урожая для отображения в инвентаре, загляните в `!инвентарь урожай` чтобы просмотреть его",

            IzumiReplyMessage.UserSeedsDesc =>
                "Ваши семена:",

            IzumiReplyMessage.UserSeedsSpringFieldName =>
                "Весенние семена",

            IzumiReplyMessage.UserSeedsSummerFieldName =>
                "Летние семена",

            IzumiReplyMessage.UserSeedsAutumnFieldName =>
                "Осенние семена",

            IzumiReplyMessage.UserCropsDesc =>
                "Ваш собранный урожай:",

            IzumiReplyMessage.UserCropsSpringFieldName =>
                "Весенний урожай",

            IzumiReplyMessage.UserCropsSummerFieldName =>
                "Летний урожай",

            IzumiReplyMessage.UserCropsAutumnFieldName =>
                "Осенний урожай",

            IzumiReplyMessage.InventoryFishOutOfLimit =>
                "У вас слишком много рыбы для отображения в инвентаре, загляните в `!инвентарь рыба` чтобы просмотреть ее.",

            IzumiReplyMessage.ExploreGardenBegin =>
                "Вы отправились на исследования вглубь леса, который окружает **{0}**, нельзя сразу сказать что вас ожидает впереди.",

            IzumiReplyMessage.ExploreCastleBegin =>
                "Вы отправились на исследования пещер, которых хватает вокруг **{0}**. Мрачное дело, но ресурсы нужным всем, не могу осуждать.",

            IzumiReplyMessage.ExploreForestEmpty =>
                "Вы знаете это ощущение, когда приходишь вглубь леса и не помнишь зачем пришел? Именно такое ощущение пришло к вам когда вы добрались до места назначения, и только вернувшись обратно вы вспомнили что ходили то за ресурсами.",

            IzumiReplyMessage.ExploreCastleEmpty =>
                "Вы знаете это ощущение, когда приходишь вглубь шахт и не помнишь зачем пришел? Именно такое ощущение пришло к вам когда вы добрались до места назначения, и только вернувшись обратно вы вспомнили что ходили то за ресурсами.",

            IzumiReplyMessage.ExploreForestSuccess =>
                "Вы вернулись с исследования леса и были приятно удивлены тяжестью своей корзины.",

            IzumiReplyMessage.ExploreCastleSuccess =>
                "Вы вернулись с исследования шахт и были приятно удивлены тяжестью своей корзины.",

            IzumiReplyMessage.ExploreSuccessFieldName =>
                "Полученные ресурсы",

            IzumiReplyMessage.UserFishDesc =>
                "Ваша рыба:",

            IzumiReplyMessage.FishingBegin =>
                "**{0}** полон желающих поймать крутой улов и теперь вы одни из них. В надежде что богиня фортуны пошлет вам улов потяжелее вы отправляетесь на рыбалку, но даже самые опытные рыбаки никогда не могут знать заранее насколько удачно все пройдет.",

            IzumiReplyMessage.FishingEmpty =>
                "Сегодня явно не ваш день, ведь вернувшись вам совсем нечем похвастаться перед жителями города.\nВы почти поймали {0} {1}, однако хитрая рыба смогла сорваться с крючка. Но не расстраивайтесь, рыба в здешних водах никуда не денется, возвращайтесь и попытайте удачу еще раз!",

            IzumiReplyMessage.FishingSuccess =>
                "Вы возвращаетесь с улыбкой на лице и гордо демонстрируете жителям города {0} {1}.\nЕсть чем гордиться, понимаю, но рыбы в здешних водах еще полно, возвращайтесь за новым уловом поскорее!",

            IzumiReplyMessage.GamblingBetCooldown =>
                "Извините, но дилер раскрывает новую колоду. Попробуйте еще раз через {0}.",

            IzumiReplyMessage.GamblingBetNoCurrency =>
                "Прискорбно, но у вас иссякли {0} иены на подобную ставку. Не унывайте! Двери нашего казино всегда открыты, возвращайтесь в любое время!",

            IzumiReplyMessage.GamblingBetMinCurrency => "Извините, но мы играем по-крупному! Не меньше {0} {1} иен.",

            IzumiReplyMessage.GamblingBetMaxCurrency =>
                "Ничего себе, у вас крупная сумма! Но вы не могли бы разделить ставку по {0} {1} иен максимум? Иначе крупье может хватить удар от таких богатств.",

            IzumiReplyMessage.GamblingBetCubeDrop => "*На кубиках выпадает {0}.*\n\n",

            IzumiReplyMessage.GamblingBetWon =>
                "Прямо чувствуется, как повышается азарт от игры и выигранных {0} {1} иен! Главное, не теряйте свое чувство меры!",

            IzumiReplyMessage.GamblingBetLose =>
                "Неудача. Карта не пошла. Уверяем, колода не крапленая и {0} {1} иен честно проиграны!",

            IzumiReplyMessage.GamblingBetNoAmount =>
                "Вы забыли указать на какое количество {0} иен в этот раз мы будем делать ставку.",

            IzumiReplyMessage.CapitalSeedShopDesc =>
                "Для покупки напишите `!магазин купить семена [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            IzumiReplyMessage.CapitalSeedShopSeedFieldName =>
                "{0} `{1}` {2} {3} стоимостью {4} {5}",

            IzumiReplyMessage.CapitalSeedShopSeedDesc =>
                "Через {0} вырастет {1} {2} стоимостью {3} {4} {5}\n",

            IzumiReplyMessage.CapitalSeedShopSeedMultiple =>
                "*{0} Растет несколько шт. с одного семени*\n",

            IzumiReplyMessage.CapitalSeedShopSeedReGrowth =>
                "*{0} После первого сбора будет давать урожай каждые {1}*\n",

            IzumiReplyMessage.MarketBuyDesc =>
                "Для покупки напишите `!рынок купить [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            IzumiReplyMessage.MarketBuyFieldName =>
                "{0} `{1}` {2} {3} {4} продает {5} {6}",

            IzumiReplyMessage.MarketBuyListDesc =>
                "{0} `{1}` {2} {3} **{4}** продает {5} {6} по {7} {8} {9}, еще {10} шт.\n",

            IzumiReplyMessage.MarketBuyListDescNull =>
                "На рынке нет заявок на подходящие товары.",

            IzumiReplyMessage.MarketBuyDirectWrongAmount =>
                "Вы указали для покупки большее количество, чем выставлено на продажу.",

            IzumiReplyMessage.MarketBuyDirectNoCurrency =>
                "У вас недостаточно {0} {1} для оплаты этой покупки.",

            IzumiReplyMessage.MarketBuyDirectSuccess =>
                "Вы успешно купили {0} {1} {2} за {3} {4} {5}.",

            IzumiReplyMessage.MarketBuyRequestNoCurrency =>
                "У вас недостаточно {0} {1} чтобы оплатить эту заявку.",

            IzumiReplyMessage.MarketBuyRequestSuccess =>
                "Вы успешно создали заявку на покупку {0} {1} {2} по {3} {4} {5}.",

            IzumiReplyMessage.MarketSellDesc =>
                "Для продажи напишите `!рынок продать [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            IzumiReplyMessage.MarketSellFieldName =>
                "{0} `{1}` {2} {3} {4} покупает {5} {6}",

            IzumiReplyMessage.MarketSellListDesc =>
                "{0} `{1}` {2} {3} **{4}** покупает {5} {6} по {7} {8} {9}, еще {10} шт.\n",

            IzumiReplyMessage.MarketSellListDescNull =>
                "На рынке нет заявок на подходящие товары.",

            IzumiReplyMessage.MarketSellDirectWrongAmount =>
                "Вы указали для продажи большее количество, чем выставлено на покупку.",

            IzumiReplyMessage.MarketSellDirectSuccess =>
                "Вы успешно продали {0} {1} {2} за {3} {4} {5}.\n\n*Налог рынка составил {3} {6} {7}.*",

            IzumiReplyMessage.MarketSellRequestNoCurrency =>
                "У вас недостаточно {0} {1} чтобы создать эту заявку.",

            IzumiReplyMessage.MarketSellRequestSuccess =>
                "Вы успешно создали заявку на продажу {0} {1} {2} по {3} {4} {5}.",

            IzumiReplyMessage.MarketBuyYourself =>
                "Вы не можете покупать у самого себя, в этом ведь нет никакого смысла!",

            IzumiReplyMessage.MarketSellYourself =>
                "Вы не можете продавать самому себе, в этом ведь нет никакого смысла!",

            IzumiReplyMessage.MarketBuyNotify =>
                "{0} {1} **{2}** купил у вас {3} {4} {5} по заявке `#{6}`, вы получили {7} {8} {9}.\n\n*Налог рынка составил {7} {10} {11}*.",

            IzumiReplyMessage.MarketSellNotify =>
                "{0} {1} **{2}** продал вам {3} {4} {5} по заявке `#{6}`.",

            IzumiReplyMessage.MarketRequestAlready =>
                "Вы уже создали заявку `#{0}` на {1} {2}, если вам необходимо что-то изменить - необходимо сначала `!заявки отменить {0}`.",

            IzumiReplyMessage.MarketRequestListDesc =>
                "Для отмены заявки напишите `!рынок заявки отменить [номер]`.",

            IzumiReplyMessage.MarketRequestList =>
                "{0} `{1}` {2} {3} {4} по {5} {6} {7}, еще {8} шт.\n",

            IzumiReplyMessage.MarketRequestListNull =>
                "Вы не создавали заявок на покупку или продажу этой группы товаров.",

            IzumiReplyMessage.MarketRequestWrongUser =>
                "Вы не можете отменять чужие заявки, надеюсь что вы просто ошиблись номером.",

            IzumiReplyMessage.MarketRequestSellCancel =>
                "Вы успешно отменили свою заявку и получили {0} {1} {2} которые не успели продать.",

            IzumiReplyMessage.MarketRequestBuyCancel =>
                "Вы успешно отменили свою заявку на покупку {0} {1} {2} и вернули {3} {4} {5} которые не успели потратить.",

            IzumiReplyMessage.ShopBuySeedWrongSeason =>
                "Эти семена доступны для приобретения только в сезон «{0}».",

            IzumiReplyMessage.ShopBuyNoCurrency =>
                "У вас недостаточно {0} {1} для этой покупки.",

            IzumiReplyMessage.ShopBuySeedSuccess =>
                "Вы успешно купили {0} {1} {2} за {3} {4} {5}, отличного урожая вам!",

            IzumiReplyMessage.FisherShopDesc =>
                "Рыбак покупает только рыбу текущего сезона или не имеющую привязки к сезону, по ценам указанным ниже.\nДля продажи напишите `!рыбак продать [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.\n\nВы так же можете продать всю рыбу указав вместо количества слово `всю` или продать вообще всю подходяющую по сезону рыбу написав `!рыбак продать всю`.*",

            IzumiReplyMessage.FisherShopFishDesc =>
                "{0} `{1}` {2} {3} за {4} {5} {6}\n",

            IzumiReplyMessage.FisherSellWrongSeason =>
                "Рыбак покупает только рыбу текущего сезона или не имеющую привязки к сезону.",

            IzumiReplyMessage.FisherSellNoFish =>
                "У вас недостаточно {0} {1} для этой сделки.",

            IzumiReplyMessage.FisherSellSuccess =>
                "Вы успешно продали {0} {1} {2} и получили {3} {4} {5}.",

            IzumiReplyMessage.FieldInfoNullDesc =>
                "Вы еще не приобрели участок ;c",

            IzumiReplyMessage.FieldInfoNullFieldName =>
                "Приобретение участка земли",

            IzumiReplyMessage.FieldInfoNullFieldDesc =>
                "Напишите `!участок купить` чтобы приобрести участок земли за {0} {1} {2}. После приобретения вы получите 5 клеток земли на которых можно выращивать урожай.",

            IzumiReplyMessage.FieldInfoHarvestingFieldName =>
                "Выращивание урожая",

            IzumiReplyMessage.FieldInfoHarvestingFieldDesc =>
                "Для начала необходимо `!участок посадить [номер клетки] [название семян]` семена на ваши клетки земли.\n\nСемена необходимо поливать каждый день, кроме дождливых, для этого напишите `!участок полить`. Чем больше ячеек необходимо поливать - тем дольше это займет.\n\nПосле того как ваши семена созреют, вы можете\n`!участок собрать [номер клетки]` готовый урожай.\n\nЕсли вы передумали выращивать семена на этой клетке или хотите их заменить - `!участок выкопать [номер клетки]` и они будут удалены.",

            IzumiReplyMessage.FieldBuyAlready =>
                "Вы уже приобрели собственный участок земли.\nНапишите просто `!участок` чтобы просмотреть информацию о нем.",

            IzumiReplyMessage.FieldBuySuccess =>
                "Вы успешно приобрели собственный участок земли за {0} {1} {2}.\nНапишите `!участок` чтобы просмотреть информацию о нем.",

            IzumiReplyMessage.FieldBuyNoCurrency =>
                "У вас недостаточно {0} {1} для приобретения участка земли. Возвращайтесь как накопите нужную сумму, это того стоит!",

            IzumiReplyMessage.FieldInfoStateCompletedReGrowth =>
                "После сбора будет давать урожай каждые {0}.",

            IzumiReplyMessage.UserFieldNull =>
                "Прежде чем взаимодействовать с клетками земли, необходимо сперва обзавестись участком земли. Ознакомтесь с необходимой информацией через команду `!участок`.",

            IzumiReplyMessage.UserFieldEmpty =>
                "На этой ячейке ничего не растет, посадите сперва семена через команду `!участок посадить [номер клетки] [название семян]`.",

            IzumiReplyMessage.UserFieldCompleted =>
                "На этой ячейке земли находится готовый к сбору урожай, соберите сперва его при помощи команды `!участок собрать [номер клетки]`.",

            IzumiReplyMessage.UserFieldPlantAlready =>
                "На этой ячейке земли уже растет урожай, если вы хотите избавиться от него - напишите `!участок выкопать [номер клетки]`.",

            IzumiReplyMessage.UserFieldPlantOnlyCurrentSeason =>
                "Сажать можно только семена текущего сезона, иначе они не смогут расти!",

            IzumiReplyMessage.UserFieldPlantSuccess =>
                "Вы успешно посадили {0} {1}, поливайте их каждый день и уже через {2} дней вы будете наслаждаться урожаем!",

            IzumiReplyMessage.SeedByLocalizedNameNull =>
                "Я не нашла семян с таким названием, возможно вы ошиблись?",

            IzumiReplyMessage.UserFieldWaterStart =>
                "Вы отправились поливать нуждающиеся в этом семена, на это уйдет {0} минут.",

            IzumiReplyMessage.UserFieldWaterSuccess =>
                "Вы успешно полили семена, теперь можно быть уверенным в том, что они будут расти.",

            IzumiReplyMessage.UserFieldCollectNotReady =>
                "Семена на этой клетке еще не созрели для сбора, поливайте их каждый день и ожидайте своего урожая.",

            IzumiReplyMessage.UserFieldCollectSuccessReGrowth =>
                "Вы успешно собрали {0} {1} {2}. Новый урожай через {3} дней.",

            IzumiReplyMessage.UserFieldCollectSuccess =>
                "Вы успешно собрали {0} {1} {2}, ячейка земли теперь свободна.",

            IzumiReplyMessage.UserFieldDigEmpty =>
                "На этой клетке ничего не растет, нечего выкапывать.",

            IzumiReplyMessage.UserFieldDigSuccess =>
                "Вы успешно выкопали {0} {1} с этой клетки земли, теперь она свободна.",

            IzumiReplyMessage.CatchErrorHandle =>
                "Кажется что-то пошло не так, попробуйте еще раз проверив все внимательнее.",

            IzumiReplyMessage.WorldInfoTimeFieldName =>
                "{0} Текущее время",

            IzumiReplyMessage.WorldInfoTimeFieldDesc =>
                "*Время суток влияет на виды рыб, которые можно поймать.*\nСейчас {0}:{1}, **{2}**.",

            IzumiReplyMessage.WorldInfoWeatherTodayFieldName =>
                "{0} Погода сегодня",

            IzumiReplyMessage.WorldInfoWeatherTodayFieldDesc =>
                "*Ежедневная погода влияет на виды рыб, которые можно поймать, а так же в дождливую погоду не нужно поливать урожай.*\nСегодня погода будет **{0}**.",

            IzumiReplyMessage.WorldInfoWeatherTomorrowFieldName =>
                "{0} Предсказательница",

            IzumiReplyMessage.WorldInfoWeatherTomorrowFieldDesc =>
                "*C вами предсказетельница, ваш источник прогнозов погоды номер один. А сейчас - прогноз погоды на завтра...*\nПогода обещает быть **{0}**.",

            IzumiReplyMessage.WorldInfoSeasonFieldName =>
                "{0} Сезон",

            IzumiReplyMessage.WorldInfoSeasonFieldDesc =>
                "*Текущий сезон определяет ассортимент семян в магазине, ведь у каждого урожая есть свой сезон роста. Посаженные на ячейки семена умирают при смене сезона, поэтому будьте дальновидными. Так же влияет на виды рыб, которые можно поймать.*\nТекущий сезон - **{0}**.",

            IzumiReplyMessage.WorldInfoStateFieldName =>
                "{0} Состояние мира",

            IzumiReplyMessage.WorldInfoStateFieldDesc =>
                "Временно недоступно...",

            IzumiReplyMessage.UpdateAboutMinLimit =>
                "Указанный вами текст слишком короткий, попробуйте описать себя более подробно ;)",

            IzumiReplyMessage.TemporarilyUnavailable =>
                "Временно недоступно...",

            IzumiReplyMessage.TitleAdded =>
                "Вы получили новый титул «{0} {1}»!\nЗагляните в свои `!титулы` чтобы просмотреть доступные вам титулы.",

            IzumiReplyMessage.FisherMassSellNoFish =>
                "У вас нет подходящей по сезону рыбы для продажи, не стоит попросту беспокоить рыбака.",

            IzumiReplyMessage.FisherMassSellSuccessDesc =>
                "После достаточно быстрых торгов с рыбаком, вы успешно продали ему всю свою рыбу. Стоит просмотреть отчетность о продаже чтобы убедиться что вас нигде не обманули.",

            IzumiReplyMessage.FisherMassSellSuccessFieldName =>
                "Отчетность о продаже",

            IzumiReplyMessage.FisherMassSellSuccessFieldDesc =>
                "{0}\n\nИтоговая прибыль {1} {2} {3}",

            IzumiReplyMessage.FisherMassSellFishLine =>
                "{0} {1} {2} за {3} {4} {5}\n",

            IzumiReplyMessage.FisherMassSellFishLineOutOfLimit =>
                "Отчетность была такой длинной, что вы решили взглянуть сразу на самое важное",

            IzumiReplyMessage.UserFieldWaterNull =>
                "Трудиться это хорошо, жаль только поливать нечего.",

            IzumiReplyMessage.LotteryWinnerPm =>
                "Ваш {0} лотерейный билет оказался счастливым и приносит вам {1} {2} {3}, тратьте с умом!",

            IzumiReplyMessage.LotteryBuyAlready =>
                "У вас ведь уже есть {0} лотерейный билет, зачем вам еще один? Дождитесь розыгрыша а затем покупайте новый.",

            IzumiReplyMessage.LotteryBuyNoCurrency =>
                "Кажется у вас недостаточно {0} {1} для покупки {2} лотерейного билета, накопите нужную сумму и возвращайтесь.",

            IzumiReplyMessage.LotteryBuySuccess =>
                "Вы успешно приобрели {0} лотерейный билет за {1} {2} {3}, теперь осталось дождаться розыгрыша.",

            IzumiReplyMessage.LotteryInfoDesc =>
                "Приобретите {0} лотерейный билет через команду `!лотерея купить` за {1} {2} {3}, чтобы получить возможность выиграть {1} {4} {3}!\n\nВы так же можете `!лотерея подарить [игровое имя]` отправить {0} лотерейный билет в {5} подарок другому игроку, заплатив еще {1} {6} {3} за услуги курьера, который доставит подарок в любую точку мира.",

            IzumiReplyMessage.LotteryInfoRulesFieldName =>
                "Правила участия",

            IzumiReplyMessage.LotteryInfoRulesFieldDesc =>
                "Покупаете {0} лотерейный билет и ожидаете, когда наберется 10 участников. Затем случайным образом выбирается победитель, который получит {1} приз. Повторяете пока не станете местным миллионером ;)",

            IzumiReplyMessage.LotteryInfoCurrentMembersFieldName =>
                "Текущие участники",

            IzumiReplyMessage.LotteryInfoCurrentMembersFieldDesc =>
                "На данный момент {0} участников уже приобрели {1} лотерейный билет, до розыграша осталось еще {2}!\n\n{3}",

            IzumiReplyMessage.LotteryGiftAlreadyHave =>
                "У {0} {1} **{2}** уже есть {3} лотерейный билет, лучше подарить его кому-либо еще.",

            IzumiReplyMessage.LotteryGiftNoCurrency =>
                "У вас недостаточно {0} {1} для того чтобы оплатить этот подарок.",

            IzumiReplyMessage.LotteryGiftSuccess =>
                "Вы успешно отправили {0} лотерейный билет {1} {2} **{3}**. Наша курьерская служба доставит его сию секунду.",

            IzumiReplyMessage.LotteryGiftSuccessPm =>
                "Вы получили в подарок {0} лотерейный билет от {1} {2} **{3}**.",

            IzumiReplyMessage.LotteryGiftYourself =>
                "Вы конечно можете заплатить курьерской службе чтобы они доставили вам {0} лотерейный билет, но в этом нет никакого смысла, просто купите его.",

            IzumiReplyMessage.ShopListDesc =>
                "В этом мире множество магазинов, ознакомиться с их списком можно ниже:",

            IzumiReplyMessage.MarketSelling =>
                "Продаете",

            IzumiReplyMessage.MarketBuying =>
                "Покупаете",

            IzumiReplyMessage.UserDontHaveSeed =>
                "У вас нет {0} {1}, предлагаю для начала купить их в **{2}**!",

            IzumiReplyMessage.InventoryFoodOutOfLimit =>
                "У вас слишком много блюд для отображения в инвентаре, загляните в `!инвентарь блюда` чтобы просмотреть их",

            IzumiReplyMessage.UserFoodDesc =>
                "Ваши блюда:",

            IzumiReplyMessage.UserCollectionDesc =>
                "Когда вы впервые находите или создаете предмет, он попадает в вашу коллекцию. Вы можете просмотреть ее, указав необходимую категорию в команде `!коллекция [номер]`.",

            IzumiReplyMessage.UserCollectionFieldName =>
                "Категории",

            IzumiReplyMessage.UserFoodMastery0 =>
                "Блюда начинающего повара",

            IzumiReplyMessage.UserFoodMastery50 =>
                "Блюда повара-ученика",

            IzumiReplyMessage.UserFoodMastery100 =>
                "Блюда опытного повара",

            IzumiReplyMessage.UserFoodMastery150 =>
                "Блюда повара-профессионала",

            IzumiReplyMessage.UserFoodMastery200 =>
                "Блюда повара-эксперта",

            IzumiReplyMessage.UserFoodMastery250 =>
                "Блюда мастера-повара",

            IzumiReplyMessage.ProductShopDesc =>
                "Для покупки напишите `!магазин купить продукт [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            IzumiReplyMessage.ProductShopFieldName =>
                "{0} `{1}` {2} {3} стоимостью {4} {5} {6}",

            IzumiReplyMessage.ProductShopBuySuccess =>
                "Вы успешно купили {0} {1} {2} за {3} {4} {5}, удачного приготовления вам!",

            IzumiReplyMessage.ResourceCraftWrongLocation =>
                "Изготовить этот ресурс можно только в **{0}**.",

            IzumiReplyMessage.ResourceCraftNoCurrency =>
                "У вас недостаточно {0} {1} для оплаты инструментов.",

            IzumiReplyMessage.CraftingListFieldName =>
                "{0} `{1}` {2} {3} в {4}",

            IzumiReplyMessage.CraftingListFieldDesc =>
                "Ингредиенты:\n{0}Стоимость: {1} {2} {3}\nДлительность: {4}",

            IzumiReplyMessage.ShopRecipeDesc =>
                "Для покупки напишите `!магазин купить рецепт [номер]`.",

            IzumiReplyMessage.ShopRecipeFieldNameMastery0 =>
                "Рецепты начинающего повара",

            IzumiReplyMessage.ShopRecipeFieldNameMastery50 =>
                "Рецепты повара-ученика",

            IzumiReplyMessage.ShopRecipeFieldNameMastery100 =>
                "Рецепты опытного повара",

            IzumiReplyMessage.ShopRecipeFieldNameMastery150 =>
                "Рецепты повара-профессионала",

            IzumiReplyMessage.ShopRecipeFieldNameMastery200 =>
                "Рецепты повара-эксперта",

            IzumiReplyMessage.ShopRecipeFieldNameMastery250 =>
                "Рецепты мастера-повара",

            IzumiReplyMessage.ShopRecipeFieldDesc =>
                "`{0}` {1} {2} стоимостью {3} {4} {5}\n",

            IzumiReplyMessage.MarketNotAllowedGroup =>
                "На рынке не торгуют товарами этой группы.",

            IzumiReplyMessage.ResourceByLocalizedNameNull =>
                "Я не нашла ресурсов с таким названием, возможно вы ошиблись?",

            IzumiReplyMessage.CropByLocalizedNameNull =>
                "Я не нашла урожай с таким названием, возможно вы ошиблись?",

            IzumiReplyMessage.FoodByLocalizedNameNull =>
                "Я не нашла еду с таким названием, возможно вы ошиблись?",

            IzumiReplyMessage.CookingNoCurrency =>
                "У вас недостаточно {0} {1} для оплаты инструментов.",

            IzumiReplyMessage.CookingListFieldName =>
                "`{0}` {1} {2}",

            IzumiReplyMessage.CookingListFieldDesc =>
                "Ингредиенты: {0}\nСтоимость: {1} {2} {3}\nДлительность: {4}",

            IzumiReplyMessage.CookingListWrongMasteryBracket =>
                "Вы указали неправильную категорию рецептов, попробуйте свериться с `!приготовление` и попробовать еще раз.",

            IzumiReplyMessage.CookingListNull =>
                "У вас нет рецептов этой категории.\nВы всегда можете их приобрести заглянув в `!магазин рецептов`, который находится в **{0}**.",

            IzumiReplyMessage.CookingListCategoryDesc =>
                "Для просмотра рецептов необходимо уточнить необходимую вам категорю, написав команду `!приготовление [номер]`.",

            IzumiReplyMessage.CookingListCategoryFieldName =>
                "Категории рецептов",

            IzumiReplyMessage.CookingListCategoryFieldDesc =>
                "{0} `1` {1}\n{0} `2` {2}\n{0} `3` {3}\n{0} `4` {4}\n{0} `5` {5}\n{0} `6` {6}",

            IzumiReplyMessage.CookingListDesc =>
                "Напишите `!приготовить [номер] [количество]` для приготовления блюда.",

            IzumiReplyMessage.UserCollectionWrongGroup =>
                "В коллекции не сохраняются предметы этой категории.",

            IzumiReplyMessage.RecipeBuyAlready =>
                "У вас ведь уже есть такой {0} рецепт, не теряйте время на ерунду, вперед готовить!",

            IzumiReplyMessage.RecipeBuyNoCurrency =>
                "Кажется у вас недостаточно {0} {1} для приобретения этого {2} рецепта, возвращайтесь как накопите нужную сумму.",

            IzumiReplyMessage.RecipeBuySuccess =>
                "Вы успешно приобрели {0} {1}, самое время опробовать рецепт в деле!",

            IzumiReplyMessage.UserEffectsDesc =>
                "Тут отображается информация о текущих эффектах на вас:",

            IzumiReplyMessage.UserEffectsHelpFieldName =>
                "{0} Как получить эффект?",

            IzumiReplyMessage.UserEffectsHelpFieldDesc =>
                "Временные эффекты можно получить если `!съесть [название]` еду, а постоянные лишь активировав особые {0} карточки.",

            IzumiReplyMessage.EatFoodSuccess =>
                "Вы съели {0} {1} и получили {2} {3} {4}.",

            IzumiReplyMessage.EatFoodWrongAmount =>
                "У вас нет в наличии {0} {1}.",

            IzumiReplyMessage.UserEffectGroupAlready =>
                "У вас уже активен эффект из группы **{0}**, вы не можете получить еще один.",

            IzumiReplyMessage.ProfileReputationFieldName =>
                "Репутация в городах",

            IzumiReplyMessage.ShopListFieldName =>
                "Магазины",

            IzumiReplyMessage.ShopListFieldDesc =>
                "{0} `!магазин семян` в **{1}**, **{2}**\n{0} `!магазин сертификатов` в **{1}**, **{2}**\n{0} `!магазин баннеров` в **{1}**, **{2}**\n{0} `!магазин рецептов` в **{3}**\n{0} `!магазин продуктов` в **{4}**\n{0} `!магазин чертежей` в **{5}**",

            IzumiReplyMessage.WorldInfoDebuffFieldName =>
                "Последствия вторжения",

            IzumiReplyMessage.WorldInfoDebuffFieldDesc =>
                "*Если вторжение ежедневного босса не было остановлено, последствия не бывают хорошими.*\n{0}",

            IzumiReplyMessage.BossDebuffActive =>
                "Сегодня ежедневный босс побывал в **{0}** и людей было недостаточно, чтобы его остановить... ",

            IzumiReplyMessage.AchievementAdded =>
                "Поздравляю c выполнением достижения «{0} **{1}**» из категории «**{2}**», вы получаете {3}.",

            IzumiReplyMessage.AchievementGroupsDesc =>
                "Для просмотра достижений, напишите `!достижения [номер]`.",

            IzumiReplyMessage.AchievementGroupsFieldName =>
                "Категории достижений",

            IzumiReplyMessage.UserFamilyNull =>
                "Вы не можете воспользоваться этой командой потому что не состоите в семье.",

            IzumiReplyMessage.UserFamilyStatusRequireHead =>
                "Это может сделать только глава вашей семьи.",

            IzumiReplyMessage.UserNotInYourFamily =>
                "Этот пользователь не состоит в вашей семье.",

            IzumiReplyMessage.FamilyKickUserSuccess =>
                "Вы успешно выгнали пользователя {0} {1} **{2}** из своей семьи.",

            IzumiReplyMessage.FamilyKickUserSuccessNotify =>
                "Вас выгнали из **{0}**.",

            IzumiReplyMessage.FamilySetUserStatusCantBeHead =>
                "Вы не можете назначить другого пользователя главой семьи.",

            IzumiReplyMessage.FamilySetUserStatusSuccess =>
                "Вы успешно изменили статус этого члена семьи на **{0}**.",

            IzumiReplyMessage.FamilySetUserStatusSuccessNotify =>
                "Глава семьи изменил ваш статус на **{0}**.",

            IzumiReplyMessage.FamilyInviteNull =>
                "Я обыскала всю документацию, однако не смогла найти пригласительного письма с таким номером, возможно вы ошиблись?",

            IzumiReplyMessage.UserFamilyNotNull =>
                "Этот пользователь уже состоит в семье.",

            IzumiReplyMessage.UserFamilyAlready =>
                "Вы не можете этого сделать, ведь вы уже состоите в семье.",

            IzumiReplyMessage.FamilyInviteAcceptSuccess =>
                "Взвесив все \"за\" и \"против\" вы решили принять приглашение в семью **{0}**, добро пожаловать!",

            IzumiReplyMessage.FamilyInviteAcceptSuccessNotify =>
                "Пользователь {0} {1} **{2}** принял ваше приглашение на вступление в семью.",

            IzumiReplyMessage.FamilyInviteCancelSuccess =>
                "Вы успешно отменили пригласительное письмо под номером `{0}`.",

            IzumiReplyMessage.FamilyInviteCancelSuccessNotify =>
                "Семья **{0}** отменила действие пригласительного письма для вас.",

            IzumiReplyMessage.FamilyInviteListFamilyNullDesc =>
                "Тут собраны все приглашения в семью, которые вам прислали:\n\nВы можете `!семья приглашение принять [номер]`,\nлибо `!семья приглашение отказаться [номер]`.",

            IzumiReplyMessage.FamilyInviteListFamilyNotNullDesc =>
                "Тут собраны все приглашения в семью, которые вы отправили:\n\nВы можете `!семья приглашение отправить [игровое имя]`,\nлибо `!семья приглашение отменить [номер]`.",

            IzumiReplyMessage.FamilyInviteListFieldName =>
                "Приглашения",

            IzumiReplyMessage.FamilyInviteListFieldDescNull =>
                "Кажется, тут больше не осталось не рассмотренных приглашений.",

            IzumiReplyMessage.FamilyInviteListFamilyNullFieldDesc =>
                "{0} `{1}` Приглашение от **{2}**\n",

            IzumiReplyMessage.FamilyInviteListFamilyNotNullFieldDesc =>
                "{0} `{1}` Приглашен {2} {3} **{4}**\n",

            IzumiReplyMessage.FamilyInviteSendSuccess =>
                "Наша почтовая служба в мгновенье доставит пригласительное письмо для {0} {1} **{2}**.",

            IzumiReplyMessage.FamilyInviteSendSuccessNotify =>
                "Семья **{0}** прислала вам пригласительное письмо.\nЗагляните в `!семья приглашения` чтобы рассмотреть его.",

            IzumiReplyMessage.FamilyInviteDeclineSuccess =>
                "Взвесив все \"за\" и \"против\" вы решили отказать приглашению в семью **{0}**.",

            IzumiReplyMessage.FamilyInviteDeclineSuccessNotify =>
                "{0} {1} **{2}** отказался от вашего приглашения в семью.",

            IzumiReplyMessage.FamilyDeleteSuccess =>
                "Вы успешно расформировали свою семью. Надеюсь, вы останетесь друзьями после этого...",

            IzumiReplyMessage.FamilyUpdateDescriptionMaxLength =>
                "Максимальное количество символов для описания вашей семьи - 1024, постарайтесь вместить всю информацию в это количество.",

            IzumiReplyMessage.FamilyUpdateDescriptionSuccess =>
                "Вы успешно обновили описание вашей семьи, можете проверить его в `!семья`.",

            IzumiReplyMessage.FamilyStatusRegistration =>
                "Вы не можете этого сделать пока ваша семья находится на этапе регистрации.",

            IzumiReplyMessage.FamilyKickYourself =>
                "Вы не можете выгнать самого себя.",

            IzumiReplyMessage.FamilySetUserStatusYourself =>
                "Вы не можете изменить статус самому себе.",

            IzumiReplyMessage.FamilyInviteSendAlready =>
                "Вы уже отправили пригласительное письмо этому пользователю.",

            IzumiReplyMessage.FamilyInfoUserFamilyNull =>
                "Вы не состоите в семье на данный момент.\n\nЗагляните в `!семья приглашения` на наличие пригласительного письма в семью.\n\nВы так же можете `!семья регистрация [название]` основать свою семью, оплатив эту услугу в **{0}**.",

            IzumiReplyMessage.FamilyInfoDesc =>
                "Информация о семье **{0}**:",

            IzumiReplyMessage.FamilyInfoStatusRegistrationFieldName =>
                "Регистрация семьи",

            IzumiReplyMessage.FamilyInfoStatusRegistrationFieldDesc =>
                "Эта семья находится на этапе регистрации. Это означает что ей недоступно большинство услуг. Этап регистрации будет закончен когда она сможет собрать как минимум трех участников.\n\nЗагляните в `!семья приглашения` чтобы узнать как отправлять пригласительные письма другим пользователям.",

            IzumiReplyMessage.FamilyInfoDescriptionFieldName =>
                "Описание",

            IzumiReplyMessage.FamilyInfoMembersFieldName =>
                "Участники семьи",

            IzumiReplyMessage.RequireCert =>
                "Для начала необходимо приобрести сертификат {0} {1} в **{2}**.",

            IzumiReplyMessage.FamilyNameNotValid =>
                "К сожалению вы не можете создать семью с названием **{0}**, оно не проходит проверку.",

            IzumiReplyMessage.FamilyNameTaken =>
                "Семья **{0}** уже существует, подумайте над другим названием.",

            IzumiReplyMessage.FamilyRegistrationSuccess =>
                "Вы успешно подали заявление о регистрации семьи **{0}**! Теперь вы можете посмотреть всю информацию о ней в `!семья`.",

            IzumiReplyMessage.FamilyRegistrationCompleted =>
                "Поздравляю, ваша семья **{0}** завершила этап регистрации. Теперь вам доступны все услуги по управлению семьей.",

            IzumiReplyMessage.CapitalCertShopDesc =>
                "Для покупки напишите `!магазин купить сертификат [номер]`.",

            IzumiReplyMessage.CapitalCertShopFieldName =>
                "Сертификаты",

            IzumiReplyMessage.CapitalCertShopFieldDesc =>
                "{0} `{1}` {2} {3} стоимостью {4} {5} {6}\n",

            IzumiReplyMessage.ShopBuyCertAlready =>
                "У вас уже есть сертификат {0} {1}, используйте сперва его.",

            IzumiReplyMessage.ShopBuyCertSuccess =>
                "Вы успешно приобрели сертификат {0} {1}, будьте уверены в подлинности наших документов.\n\n*Загляните в `!сертификаты` чтобы узнать подробности.*",

            IzumiReplyMessage.RenameSuccess =>
                "Ваше игровое имя успешно изменено на **{0}**.",

            IzumiReplyMessage.CertRemoved =>
                "\n\nСертификат {0} {1} был изъят.",

            IzumiReplyMessage.FamilyRenameSuccess =>
                "Вы успешно изменили название семьи на **{0}**.",

            IzumiReplyMessage.FamilyListDesc =>
                "Тут собран список 10 лучших семей. Вы можете посмотреть подробную информацию о семье написав `!семья информация [название]`.",

            IzumiReplyMessage.FamilyListFieldName =>
                "Семьи",

            IzumiReplyMessage.FamilyListNull =>
                "Кажется сейчас нет ни одной семьи, самое время стать первыми!",

            IzumiReplyMessage.MarketRequestGroupLimit =>
                "Вы уже выставили 5 заявок в этой группе товаров. Сперва необходимо разобраться с ними, прежде чем выставлять новые.",

            IzumiReplyMessage.MarketRequestMinCost =>
                "Вы не можете указать цену товара меньше чем его базовая стоимость - {0} {1} {2}.",

            IzumiReplyMessage.FieldEmptyFieldName =>
                "Клетка земли пустая",

            IzumiReplyMessage.FieldEmptyFieldDesc =>
                "Засадите ее семенами чтобы начать выращивать урожай",

            IzumiReplyMessage.FieldNeedWatering =>
                "Не забудьте сегодня полить!",

            IzumiReplyMessage.FieldDontNeedWatering =>
                "Поливать сегодня уже не нужно.",

            IzumiReplyMessage.FieldProgress =>
                "еще {0} роста",

            IzumiReplyMessage.FieldCompletedFieldName =>
                "можно собирать",

            IzumiReplyMessage.FieldCompletedFieldDesc =>
                "Не забудьте посадить что-то на освободившееся место.",

            IzumiReplyMessage.PresetGameRolesAuthor =>
                "Игровые роли",

            IzumiReplyMessage.PresetGameRolesDesc =>
                "Вы можете получить игровые роли, которые можно **упоминать** в <#{0}> чтобы упростить процесс **поиска людей** для совместной игры, для этого нажмите на **реакцию** под этим сообщением.",

            IzumiReplyMessage.PresetGameRolesFieldName =>
                "Доступные для получения роли",

            IzumiReplyMessage.PresetGameRolesFieldDesc =>
                "{0} <@&{1}>\n{2} <@&{3}>\n{4} <@&{5}>\n{6} <@&{7}>\n{8} <@&{9}>\n{10} <@&{11}>\n{12} <@&{13}>\n{14} <@&{15}>\n{16} <@&{17}>",

            IzumiReplyMessage.PresetRolesFooter =>
                "При нажатии на реакцию, она будет снята и вы получите соответствующую роль. При необходимости роли можно снять, нажав на реакцию повторно.",

            IzumiReplyMessage.PresetRegistryAnonsRolesFieldName =>
                "Доступные для получения роли",

            IzumiReplyMessage.PresetRegistryAnonsRolesFieldDesc =>
                "{0} <@&{1}>\n\n{2} <@&{3}>\n{4} <@&{5}>\n{6} <@&{7}>\n{8} <@&{9}>\n{10} <@&{11}>",

            IzumiReplyMessage.ShopProjectDesc =>
                "Для покупки напишите `!магазин купить чертеж [номер]`.\nНапишите `!чертеж [номер]` для просмотра информации о чертеже.",

            IzumiReplyMessage.ProjectPlanShopFieldDesc =>
                "{0} `{1}` {2} {3} стоимостью {4} {5} {6}\n",

            IzumiReplyMessage.ShopFieldDescNull =>
                "Полки с этим товаром сейчас пустуют, ожидаем поставок в ближайшее время!",

            IzumiReplyMessage.ShopBuyProjectSuccess =>
                "Вы успешно приобрели чертеж {0} {1} за {2} {3} {4}, поскорее бы приступить к постройке.\n\n*Загляните в `!чертежи` чтобы узнать подробности.*",

            IzumiReplyMessage.ShopBuyProjectAlreadyHaveProject =>
                "Вы уже владеете чертежом {0} {1}, зачем вам еще один?",

            IzumiReplyMessage.ShopBuyProjectAlreadyHaveBuilding =>
                "Вы уже построили **{0}**, этот {1} чертеж вам больше не понадобится.",

            IzumiReplyMessage.UserCertsDesc =>
                "Тут собраны ваши сертификаты и инструкции по их применению:",

            IzumiReplyMessage.UserCertsFooter =>
                "После использования сертификат будет изъят.",

            IzumiReplyMessage.UserCertsFieldName =>
                "{0} Сертификат {1} {2}",

            IzumiReplyMessage.UserProjectsDesc =>
                "Напишите `!чертеж [номер]` чтобы посмотреть информацию о чертеже, не обязательном том, который у вас есть.\nНапишите `!построить [номер]` чтобы начать строительство.",

            IzumiReplyMessage.UserProjectsFooter =>
                "При постройке чертеж будет изъят.",

            IzumiReplyMessage.UserProjectsFieldName =>
                "{0} `{1}` {2} чертеж {3} {4}",

            IzumiReplyMessage.UserProjectsFieldDesc =>
                "После строительства вы получите {0} **{1}**.\n*{2}*",

            IzumiReplyMessage.UserProjectsIngredientsNull =>
                "Ресурсы не требуются",

            IzumiReplyMessage.BuildingListDesc =>
                "Все возведенные вами, вашей семьей или кланом постройки:",

            IzumiReplyMessage.BuildingListPersonalBuildingsNull =>
                "У вас нет персональных построек.\n\nВы можете заглянуть в `!магазин чертежей`, который находится в **{0}**, и приобрести там необходимый вам {1} чертеж.",

            IzumiReplyMessage.BuildingListFamilyBuildingsNull =>
                "У вашей семьи нет семейных построек.\n\nГлава семьи может заглянуть в `!магазин чертежей`, который находится в **{0}** и приобрести там необходимый вашей семье {1} чертеж.",

            IzumiReplyMessage.BuildingListClanBuildingsNull =>
                "У вашего клана нет семейныйх построек.\n\nГлава клана может заглянуть в `!магазин чертежей`, который находится в **{0}** и приобрести там необходимый вашему клану {1} чертеж.",

            IzumiReplyMessage.FamilyClanNull =>
                "Ваша семья не состоит в клане.",

            IzumiReplyMessage.BuildNoCurrency =>
                "У вас недостаточно {0} {1} для оплаты рабочих, загляните в `!чертежи` чтобы сверить всю информацию.",

            IzumiReplyMessage.BuildStartedSuccess =>
                "Заплатив {0} {1} {2} и отдав все необходимые ресурсы рабочим - они уверили вас что закончат постройку через {3}. Уверяю вас, этим ребятам можно доверять.",

            IzumiReplyMessage.BuildRequirePersonalBuildingButNull =>
                "Строительство по этому {0} чертежу требует постройку {1} **{2}**, которой у вас нет.",

            IzumiReplyMessage.BuildPersonalBuildingAlready =>
                "Вы уже владеете постройкой {0} **{1}**, этот {2} чертеж вам больше не пригодится.",

            IzumiReplyMessage.BuildRequireUserFamilyButNull =>
                "Вы не состоите в семье, а соответственно и не можете воздвигать для нее постройки, оставьте этот {0} чертеж на будущее.",

            IzumiReplyMessage.BuildRequireUserFamilyStatusHeadButLower =>
                "Воздвигать семейные постройки может только глава семьи, оставьте этот {0} чертеж на будущее.",

            IzumiReplyMessage.BuildRequireFamilyBuildingButNull =>
                "Строительство по этому {0} чертежу требует постройку {1} **{2}**, которой у вашей семьи нет.",

            IzumiReplyMessage.BuildFamilyBuildingAlready =>
                "Ваша семья уже владеет постройкой {0} **{1}**, этот {2} чертеж сейчас не нужен.",

            IzumiReplyMessage.BuildRequireClanFamilyButNull =>
                "Ваша семья не состоит в клане, а соответственно вы не можете воздвигать для него постройки, оставьте этот {0} чертеж на будущее.",

            IzumiReplyMessage.BuildRequireClanFamilyStatusOwnerButLower =>
                "Воздвигать клановые постройки может только глава семьи-основателя этого клана, оставьте этот {0} чертеж на будущее.",

            IzumiReplyMessage.BuildRequireClanBuildingButNull =>
                "Строительство по этому {0} чертежу требует постройку {1} **{2}**, которой у вашего клана нет.",

            IzumiReplyMessage.BuildClanBuildingAlready =>
                "Ваш клан уже владеет постройкой {0} **{1}**, этот {2} чертеж сейчас не нужен.",

            IzumiReplyMessage.BuildCompleted =>
                "Постройка {0} **{1}** завершена в сроки.\nЗагляните в `!постройки` чтобы убедиться о факте выполнения работ.",

            IzumiReplyMessage.TransitListFieldName =>
                "{0} `{1}` Отправиться в {2}",

            IzumiReplyMessage.TransitListFieldDesc =>
                "Длительность: {0}\nСтоимость: {1}",

            IzumiReplyMessage.ContractListDesc =>
                "Решились помочь жителям и взяться за выполнение одного из рабочих контрактов?\nТогда вам стоит написать `!контракт принять [номер]`.\nВы не сможете отказаться от работы после того как примете контракт.",

            IzumiReplyMessage.ContractListFooter =>
                "Во время выполнения рабочего контракта вы не сможете отвлекаться на другие дела до его завершения.",

            IzumiReplyMessage.ContractListFieldDesc =>
                "{0}\nОплата: {1} {2} {3}\nРепутация: {4} {5} репутации в **{6}**\nДлительность: {7}",

            IzumiReplyMessage.UserProfileRepRatingFieldName =>
                "Репутационный рейтинг",

            IzumiReplyMessage.UserProfileRepRatingFieldDesc =>
                "У вас {0} {1} рейтинга\nСтатус в обществе: **{2}**",

            IzumiReplyMessage.UserProfileBirthdayFieldName =>
                "День рождения",

            IzumiReplyMessage.UserProfileBirthdayFieldDescNull =>
                "{0} Не указан",

            IzumiReplyMessage.ContractAcceptDesc =>
                "Вы взялись помогать городу выполняя рабочий контракт, это очень здорово. Желаю вам отлично поработать, не подведите!",

            IzumiReplyMessage.ContractAcceptRewardFieldName =>
                "Ожидаемая награда",

            IzumiReplyMessage.ContractRewardFieldDesc =>
                "{0} {1} {2} и {3} {4} репутации в **{5}**",

            IzumiReplyMessage.TimeFieldName =>
                "Длительность",

            IzumiReplyMessage.ContractCompletedDesc =>
                "С возвращением, жители говорят что вы отлично потрудились и заслуживаете положенной награды, она уже ожидает вас в вашем инвентаре. Про репутацию я тоже не забыла, не переживайте!",

            IzumiReplyMessage.ContractCompletedRewardFieldName =>
                "Награда",

            IzumiReplyMessage.ContractWrongLocation =>
                "Работать по этому контракту необходимо в **{0}**, а вы немного не там.",

            IzumiReplyMessage.ExploreRewardFieldName =>
                "Возможная добыча",

            IzumiReplyMessage.ExploreRewardFishingFieldDesc =>
                "{0} случайная рыба",

            IzumiReplyMessage.TransitCompleteInfoChannelsFieldName =>
                "Информационные каналы локации",

            IzumiReplyMessage.PresetRegistryNicknameTitle =>
                "Шаг 1: Выбор игрового имени",

            IzumiReplyMessage.PresetRegistryNicknameDesc =>
                "Как и всегда, все начинается с самой сложной части - придумать игровое имя.\nУ нас есть несколько правил, которые сузят круг ваших вариантов и помогут подобрать отличное имя.\n\nИтак, игровое имя:\n<:List:750651302920585245> может состоять как из русских так и английских букв;\n<:List:750651302920585245> должно начинаться с заглавной буквы;\n<:List:750651302920585245> не может содержать других заглавных букв;\n<:List:750651302920585245> не может содержать символы кроме пробела;\n<:List:750651302920585245> не может нарушать правил сервера.",

            IzumiReplyMessage.PresetRegistryCommandTitle =>
                "Шаг 2: Регистрация",

            IzumiReplyMessage.PresetRegistryCommandDesc =>
                "После того как вы определились с игровым именем - смело приступайте к регистрации.\n\nНапишите в **личные сообщения** <@750617055992217620> команду\n`!регистрация [игровое имя]`.",

            IzumiReplyMessage.PresetRegistryAnonsRolesTitle =>
                "Шаг 3: Получение ролей оповещений",

            IzumiReplyMessage.PresetRegistryAnonsRolesDesc =>
                "Вам определенно стоит получить роли для **оповещения** о различных **событиях**.\nХотите получать все уведомления? А может только о недельных событиях? Выбирайте только то, что вас **интересует**.",

            IzumiReplyMessage.UpdateGenderAlready =>
                "Вам уже подтвердили ваш пол на {0} **{1}**.\n\nЕсли по какой-то причине **{2}** ошиблись, свяжитесь с ними напрямую.",

            IzumiReplyMessage.UpdateGenderDesc =>
                "Я передала все необходимые бумаги нашей службе доставки, они доставят их **{0}** и они обязательно свяжуться с вами для подтверждения вашего {1} пола.",

            IzumiReplyMessage.UpdateGenderNotifyDesc =>
                "Пользователь {0} просит подтвердить ему {1} пол.\nПригласите его в **голосовой канал** для быстрой беседы.",

            IzumiReplyMessage.UpdateGenderNotifyFieldName =>
                "Обновление пола пользователя",

            IzumiReplyMessage.UpdateGenderNotifyFieldDesc =>
                "{0} **{1}** `!mod gender {2} 1`\n{3} **{4}** `!mod gender {2} 2`",

            IzumiReplyMessage.ModGenderDesc =>
                "Пол пользователя {0} был успешно обновлен на {1} **{2}**.",

            IzumiReplyMessage.ModGenderNotifyDesc =>
                "Ваш пол был обновлен на {0} **{1}**.",

            IzumiReplyMessage.UserFieldWaterFamilyMemberOnly =>
                "Поливать участки других пользователей можно лишь в том случае, если вы состоите в одной семье.",

            IzumiReplyMessage.NoRequiredIngredientAmount =>
                "Сверяясь со списком ингредиентов, я заметила что в вашем инвентаре недостаточно {0} {1}.",

            IzumiReplyMessage.CraftingAlcoholDesc =>
                "Собрав все необходимые ингредиенты и свершившись с инструкцией, вы начали изготовление {0} {1}.",

            IzumiReplyMessage.CraftingAlcoholExpectedFieldName =>
                "Ожидаемый изготовленный алкоголь",

            IzumiReplyMessage.CraftingAlcoholCompleteDesc =>
                "После рабочего процесса, вы с улыбкой смотрите на идеальное {0} {1} изготовленное собственными руками.",

            IzumiReplyMessage.CraftingAlcoholReceivedFieldName =>
                "Полученный алкоголь",

            IzumiReplyMessage.CraftingResourceDesc =>
                "Собрав все необходимые ингредиенты и свершившись с инструкцией, вы начали изготовление {0} {1}.",

            IzumiReplyMessage.CraftingResourceExpectedFieldName =>
                "Ожидаемые изготовленные предметы",

            IzumiReplyMessage.CraftingResourceCompleteDesc =>
                "После рабочего процесса, вы с улыбкой смотрите на идеальные {0} {1} изготовленные собственными руками.",

            IzumiReplyMessage.CraftingResourceReceivedFieldName =>
                "Полученные предметы",

            IzumiReplyMessage.CraftingFoodDesc =>
                "Собрав все необходимые ингредиенты и сверившись с рецептом, вы начали приготовление {0} {1}.",

            IzumiReplyMessage.CraftingFoodExpectedFieldName =>
                "Ожидаемые блюда",

            IzumiReplyMessage.CraftingFoodReceivedFieldName =>
                "Полученные блюда",

            IzumiReplyMessage.CraftingFoodCompleteDesc =>
                "После рабочего процесса, вы с улыбкой смотрите на идеальный {0} {1} приготовленный собственными руками.",

            IzumiReplyMessage.IngredientsSpent =>
                "Затраченные ингредиенты",

            IzumiReplyMessage.CraftingAlcoholInFamilyHouse =>
                "К счастью, в семейной {0} **{1}** есть все необходимые инструменты, что ускорит работу.",

            IzumiReplyMessage.CraftingResourceInFamilyHouse =>
                "\nК счастью, в семейной {0} **{1}** есть все необходимые инструменты, что ускорит работу.",

            IzumiReplyMessage.CraftingFoodInFamilyHouse =>
                "К счастью, в семейной {0} **{1}** есть все необходимые приборы, что ускорит работу.",

            IzumiReplyMessage.UserTitleListDesc =>
                "Для смены текущего титула напишите `!титул [номер]`.",

            IzumiReplyMessage.UserTitleListFieldName =>
                "Доступные вам титулы",

            IzumiReplyMessage.UserUpdateTitleDontHave =>
                "У вас нет титула {0} {1}.",

            IzumiReplyMessage.UserUpdateTitleAlready =>
                "Титул {0} {1} уже установлен как текущий.",

            IzumiReplyMessage.UserUpdateTitleSuccess =>
                "Вы успешно обновили свой текущий титул на {0} {1}.",

            IzumiReplyMessage.ShopBannerDesc =>
                "Для покупки напишите `!магазин купить баннер [номер]`.",

            IzumiReplyMessage.ShopBannerFieldDesc =>
                "Название тайтла: {0}\nСтоимость: {1} {2} {3}\n[Нажмите сюда]({4}) чтобы посмотреть как он выглядит.",

            IzumiReplyMessage.DynamicShopDesc =>
                "\n\n*Товары этого магазина обновляются каждый день, не пропускайте новые партии!*",

            IzumiReplyMessage.DynamicShopFooter =>
                "Следующее обновление магазина через {0}.",

            IzumiReplyMessage.ShopBuyBannerAlready =>
                "У вас уже есть {0} **«{1}»**.",

            IzumiReplyMessage.ShopBuyBannerSuccess =>
                "Вы успешно купили {0} **«{1}»** за {2} {3} {4}.\n\nСамое время заглянуть в `!баннеры`.",

            IzumiReplyMessage.UserBannerListDesc =>
                "Для смены текущего баннера напишите `!баннер [номер]`.",

            IzumiReplyMessage.UserBannerListFieldName =>
                "Доступные вам баннеры",

            IzumiReplyMessage.UserBannerUpdateAlready =>
                "У вас уже активен {0} **«{1}»**.",

            IzumiReplyMessage.UserBannerUpdateSuccess =>
                "Вы успешно обновили свой баннер на {0} **«{1}»**",

            IzumiReplyMessage.UserCardListDesc =>
                "Тут собрана ваша коллекция карточек. Если же вас интересуют карточки, которые вы выбрали для своей колоды - стоит заглянуть в `!колода`.\n\nНапишите `!карточка [номер]` чтобы просмотреть подробную информацию о карточке, не обязательно той, которая у вас есть.\n\nНапишите `!колода добавить [номер]` чтобы добавить карточку в вашу колоду.\nИли `!колода убрать [номер]` чтобы убрать ее из кололы.",

            IzumiReplyMessage.UserCardListFooter =>
                "В вашей коллекции {0} из {1} доступных карточек.",

            IzumiReplyMessage.UserCardListLengthLessThen1FieldName =>
                "У вас нет ни одной карточки",

            IzumiReplyMessage.UserCardListLengthLessThen1FieldDesc =>
                "Свою первую карточку вы можете получить пройдя `!обучение`, которое начинается в **{0}**.",

            IzumiReplyMessage.UserCardListLengthMoreThen15FieldName =>
                "У вас слишком много карточек",

            IzumiReplyMessage.UserCardListLengthMoreThen15FieldDesc =>
                "По техническим причинам я не могу отобразить тут больше 15 карточек.",

            IzumiReplyMessage.CardDetailedDesc =>
                "Название тайтла: {0}.\nЭффект: {1}.\n[Нажмите сюда]({2}) чтобы посмотреть как она выглядит.",

            IzumiReplyMessage.UserDeckListDesc =>
                "Тут отображается ваша {0} колода карточек.\n\nНапишите `!карточка [номер]` чтобы просмотреть подробную информацию о карточке, не обязательно той, которая у вас есть.\n\nНапишите `!колода добавить [номер]` чтобы добавить карточку в вашу колоду.\nИли `!колода убрать [номер]` чтобы убрать ее из кололы.",

            IzumiReplyMessage.UserDeckListFooter =>
                "В вашей колоде {0} из 5 карточек.",

            IzumiReplyMessage.UserDeckListLengthLessThen1FieldName =>
                "В вашей колоде нет ни одной карточки",

            IzumiReplyMessage.UserDeckListLengthLessThen1FieldDesc =>
                "Добавьте карты из своей коллекции в {0} колоду чтобы активировать их эффекты.",

            IzumiReplyMessage.UserDeckRemoveNotInDeck =>
                "{0} «**{1}**» не находится в вашей {2} колоде.",

            IzumiReplyMessage.UserDeckRemoveSuccess =>
                "Вы убрали {0} «**{1}**» из своей {2} колоды и сразу ощутили как ее эффект перестал действовать.",

            IzumiReplyMessage.UserDeckAddAlreadyInDeck =>
                "{0} «**{1}**» уже находится в вашей {2} колоде.",

            IzumiReplyMessage.UserDeckAddLengthMoreThen5 =>
                "В вашей {0} колоде уже максимальное количество карточек.",

            IzumiReplyMessage.UserDeckAddSuccess =>
                "Вы добавили {0} «**{1}**» в свою {2} колоду и сразу же ощутили как ее эффект начал действовать.",

            IzumiReplyMessage.ProjectInfoDesc =>
                "После строительства вы получите {0} **{1}**.\n*{2}*",

            IzumiReplyMessage.ProjectInfoRequireFieldName =>
                "Требование",

            IzumiReplyMessage.ProjectInfoIngredientsFieldName =>
                "Необходимые ресурсы",

            IzumiReplyMessage.ProjectInfoBuildingCostFieldName =>
                "Стоимость постройки",

            IzumiReplyMessage.ProjectInfoTitle =>
                "`{0}` {1} чертеж {2} {3}",

            IzumiReplyMessage.CardInfoIdFieldName =>
                "Номер",

            IzumiReplyMessage.CardInfoRarityFieldName =>
                "Редкость",

            IzumiReplyMessage.CardInfoAnimeFieldName =>
                "Название тайтла",

            IzumiReplyMessage.CardInfoNameFieldName =>
                "Название",

            IzumiReplyMessage.MarketInfoDesc =>
                "На рынке можно продавать и покупать различные товары.",

            IzumiReplyMessage.MarketInfoBuyFieldName =>
                "Покупка",

            IzumiReplyMessage.MarketInfoBuyFieldDesc =>
                "Для просмотра заявок `!рынок купить [номер категории]` или `!рынок купить [номер категории] [название]` если вас интересует конкретный товар.\n\nЧтобы выставить заявку на покупку предмета, напишите `!рынок купить [номер категории] [название] [цена] [количество]`.\n*Название товара нужно указать одним словом.\nКоличество можно не указывать, по-умолчанию это 1.*",

            IzumiReplyMessage.MarketInfoSellFieldName =>
                "Продажа",

            IzumiReplyMessage.MarketInfoSellFieldDesc =>
                "Для просмотра заявок `!рынок продать [номер категории]` или `!рынок продать [номер категории] [название]` если вас интересует конкретный товар.\n\nЧтобы выставить заявку на продажу предмета, напишите `!рынок продать [номер категории] [название] [цена] [количество]`.\n*Название товара нужно указать одним словом.\nКоличество можно не указывать, по-умолчанию это 1.*",

            IzumiReplyMessage.MarketInfoRequestFieldName =>
                "Заявки",

            IzumiReplyMessage.MarketInfoRequestFieldDesc =>
                "Вы можете просмотреть выставленные вами заявки на рынке через команду `!рынок заявки`.",

            IzumiReplyMessage.MarketInfoGroupsFieldName =>
                "Категории товаров",

            IzumiReplyMessage.FamilyInviteListCantWatch =>
                "Вы не можете просматривать отправленные вашей семьей приглашения.",

            IzumiReplyMessage.TransitCostFieldName =>
                "Стоимость перемещения",

            IzumiReplyMessage.ReferralRewardFieldName =>
                "Бонус реферальной системы",

            IzumiReplyMessage.ReferralListDesc =>
                "Тут собранна информация о вашем участии в реферальной системе:",

            IzumiReplyMessage.ReferralListReferrerFieldName =>
                "Ваш реферер",

            IzumiReplyMessage.ReferralListReferralsFieldName =>
                "Приглашенные пользователи",

            IzumiReplyMessage.ReferralListReferrerNull =>
                "Вы не указали пригласившего вас пользователя.\n\n*Напишите `!пригласил [игровое имя]` чтобы указать его и получить {0} {1} {2}.*",

            IzumiReplyMessage.ReferralListReferralsNull =>
                "У вас так много приглашенных пользователей, что мне трудно назвать их всех! Но их точно **{0}**.",

            IzumiReplyMessage.ReferralListReferralsOutOfLimit =>
                "Вы еще не пригласили ни одного пользователя. Приглашайте своих друзей и получайте {0} {1} бонусы реферальной системы вместе.",

            IzumiReplyMessage.FamilyInfoDescriptionNull =>
                "У вашей семьи нет описания. Глава семьи может добавить его написав `!семья описание [текст]`.",

            IzumiReplyMessage.FamilyInfoCurrencyFieldName =>
                "Казна семьи",

            IzumiReplyMessage.UserFamilyStatusRequireNotDefault =>
                "Это может сделать только **глава** или **заместитель** семьи.",

            IzumiReplyMessage.FamilyCurrencyAddUserNoCurrency =>
                "У вас недостаточно {0} {1} чтобы добавить такое количество в казну семьи.",

            IzumiReplyMessage.FamilyCurrencyTakeFamilyNoCurrency =>
                "В казне семьи нет столько {0} {1} сколько вы пытаетесь взять.",

            IzumiReplyMessage.FamilyCurrencyAddSuccess =>
                "Вы успешно добавили {0} {1} {2} в казну семьи.",

            IzumiReplyMessage.FamilyCurrencyTakeSuccess =>
                "Вы успешно взяли {0} {1} {2} из казны семьи.",

            IzumiReplyMessage.UserProfileEnergyFieldName =>
                "Энергия",

            IzumiReplyMessage.UserMasteryDesc =>
                "Каждые 50 мастерства улучшают ваш навык, открывая новые возможности или улучшая уже доступные.\n\nМаксимальное мастерство при вашем **статусе**\\* - {0}.\n**Репутационный статус это среднее количество всех репутаций.*",

            IzumiReplyMessage.UserMasteryFieldName =>
                "{0} {1} мастерства «{2}»",

            IzumiReplyMessage.MarketRequestInfo =>
                "Цена за единицу товара: {0} {1} {2}\nОставшееся количество: {3} шт.",

            IzumiReplyMessage.MarketRequestListNullFieldName =>
                "{0} На рынке нет заявок в этой категории",

            IzumiReplyMessage.MarketRequestListNullFieldDesc =>
                "Это отличная возможность создать заявку самостоятельно!",

            IzumiReplyMessage.MarketUserRequestListNullFieldName =>
                "{0} У вас нет созданных заявок на рынке",

            IzumiReplyMessage.MarketUserRequestListNullFieldDesc =>
                "Самое время создать несколько!",

            IzumiReplyMessage.MarketUserRequestFieldName =>
                "{0} `{1}` {2} {3} {4}",

            IzumiReplyMessage.UserProjectNull =>
                "У вас нет этого {0} чертежа.",

            IzumiReplyMessage.CraftingItemListDesc =>
                "Напишите `!изготовление предмета [номер]` для просмотра рецепта изготавливаемого предмета.",

            IzumiReplyMessage.CraftingAlcoholListDesc =>
                "Напишите `!изготовление алкоголя [номер]` для просмотра рецепта алкоголя.",

            IzumiReplyMessage.CraftingAlcoholInfoDesc =>
                "Для изготовления отправляйтесь в указанную локацию и напишите `!изготовить алкоголь [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            IzumiReplyMessage.IngredientsFieldName =>
                "Необходимые ингредиенты",

            IzumiReplyMessage.CraftingPriceFieldName =>
                "Стоимость изготовления",

            IzumiReplyMessage.LocationFieldName =>
                "Локация",

            IzumiReplyMessage.CraftingItemInfoDesc =>
                "Для изготовления отправляйтесь в указанную локацию и напишите `!изготовить предмет [номер] [количество]`.\n*Количество указывать не обязательно, по-умолчанию это 1.*",

            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
