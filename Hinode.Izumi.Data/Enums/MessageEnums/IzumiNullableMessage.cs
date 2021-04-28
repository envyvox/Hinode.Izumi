using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
    /// <summary>
    /// Сообщение с ошибкой о том что ожидаемый ответ оказался null.
    /// </summary>
    public enum IzumiNullableMessage
    {
        UserWithId,
        UserWithName,
        UserInventory,
        LocalizationByKeyword,
        LocalizationByLocalizedWordGathering,
        LocalizationByLocalizedWordProduct,
        LocalizationByLocalizedWordCrafting,
        LocalizationByLocalizedWordAlcohol,
        LocalizationByLocalizedWordDrink,
        LocalizationByLocalizedWordSeed,
        LocalizationByLocalizedWordCrop,
        LocalizationByLocalizedWordFish,
        LocalizationByLocalizedWordFood,
        LocalizationByLocalizedWordCurrency,
        UserBanner,
        DynamicShopBanner,
        CardById,
        UserCard,
        UserCertificate,
        ContractById,
        ContractByLocation,
        Transit,
        FamilyByName,
        UserFamily,
        Certificate,
        Product,
        MarketRequest,
        Project,
        Building,
        FamilyInvite,
        LocalizationByLocalizedWordBar,
        LocalizationByLocalizedWordBox
    }

    public static class IzumiNullableMessageHelper
    {
        /// <summary>
        /// Возвращает локализированный текст сообщение об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        public static string Parse(this IzumiNullableMessage message) => message.Localize();

        /// <summary>
        /// Возвращает локализированный текст сообщение об ошибке поставляя указанные значения.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="replacements">Значения которые необходимо подставить в текст.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        public static string Parse(this IzumiNullableMessage message, params object[] replacements)
        {
            try
            {
                // пытаемся подставить указанные значения
                return string.Format(Parse(message), replacements);
            }
            catch (FormatException)
            {
                // выводим текст с ошибкой, если подставить не получилось
                return "`Возникла ошибка вывода ответа. Пожалуйста, покажите это Холли.`";
            }
        }

        /// <summary>
        /// Возвращает локализированный текст сообщения об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        private static string Localize(this IzumiNullableMessage message) => message switch
        {
            IzumiNullableMessage.UserWithId =>
                "Я не смогла найти пользователя с таким Id.",

            IzumiNullableMessage.UserWithName =>
                "Я не смогла найти пользователя с таким игровым именем.",

            IzumiNullableMessage.UserInventory =>
                "По всей видимости в вашем инвентаре нет {0} {1}, для начала нужно разобраться с этим.",

            IzumiNullableMessage.LocalizationByKeyword =>
                "Я не знаю что вы имеете ввиду под этим названием, это какое-то иностранное слово?",

            IzumiNullableMessage.LocalizationByLocalizedWordGathering =>
                "Я не слышала о собирательском предмете с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordProduct =>
                "Я не слышала о продукте с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordCrafting =>
                "Я не слышала о изготавливаемом предмете с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordAlcohol =>
                "Я не слышала о алкоголе с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordDrink =>
                "Я не слышала о напитке с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordSeed =>
                "Я не слышала о семенах с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordCrop =>
                "Я не слышала о урожае с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordFish =>
                "Я не слышала о рыбе с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordFood =>
                "Я не слышала о блюде с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordCurrency =>
                "Я не слышала о валюте с таким названием.",

            IzumiNullableMessage.UserBanner =>
                "У вас нет баннера с таким номером.",

            IzumiNullableMessage.DynamicShopBanner =>
                "Среди товаров магазина сейчас нет баннера с таким номером.",

            IzumiNullableMessage.CardById =>
                "Никогда не слышала о карточке с таким номером.",

            IzumiNullableMessage.UserCard =>
                "У вас нет карточки с таким номером.",

            IzumiNullableMessage.UserCertificate =>
                "У вас нет сертификата с таким номером.",

            IzumiNullableMessage.ContractById =>
                "Никогда не слышала о рабочем контракте с таким номером.",

            IzumiNullableMessage.ContractByLocation =>
                "Контракты это серьезное занятие, которое требует полной сосредоточенности на работе. Сейчас вам кажется не до этого, приходите в любой город когда будете свободны!",

            IzumiNullableMessage.Transit =>
                "Никогда не слышала о маршруте с таким номером.",

            IzumiNullableMessage.FamilyByName =>
                "Никогда не слышала о семье с таким названием.",

            IzumiNullableMessage.UserFamily =>
                "Вы не состоите в семье.",

            IzumiNullableMessage.Certificate =>
                "Никогда не слышала о сертификате с таким номером.",

            IzumiNullableMessage.Product =>
                "Никогда не слышала о продукте с таким номером.",

            IzumiNullableMessage.MarketRequest =>
                "На рынке нет заявки с таким номером.",

            IzumiNullableMessage.Project =>
                "Никогда не слышала о чертеже с таким номером.",

            IzumiNullableMessage.Building =>
                "Никогда не слышала о постройке с таким номером.",

            IzumiNullableMessage.FamilyInvite =>
                "Я обыскала всю документацию, однако не смогла найти пригласительного письма с таким номером.",

            IzumiNullableMessage.LocalizationByLocalizedWordBar =>
                "Никогда не слышала о шкале с таким названием.",

            IzumiNullableMessage.LocalizationByLocalizedWordBox =>
                "Никогда не слышала о коробке с таким названием.",

            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
