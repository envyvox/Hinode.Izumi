using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
    /// <summary>
    /// Сообщение с ошибкой о том что не выполнены необходимые требования.
    /// </summary>
    public enum IzumiPreconditionErrorMessage
    {
        RequireRegistry = 1,
        RequireRole = 2,
        RequireSeason = 3,
        RequireWeather = 4,
        RequireLocationButYouInTransit = 5,
        RequireLocationButYouInAnother = 6,
        RequireLocationButYouExploringGarden = 7,
        RequireLocationButYouExploringCastle = 8,
        RequireLocationButYouFishing = 9,
        RequireLocationButYouFieldWatering = 10,
        RequireCasinoOpen = 11,
        RequireNoDebuff = 12,
        RequireEvent = 13,
        RequirePremium = 14
    }

    public static class IzumiPreconditionErrorMessageHelper
    {
        /// <summary>
        /// Возвращает локализированный текст сообщения об ошибке.
        /// </summary>
        /// <param name="ibPreconditionErrorMessage">Сообщение об ошибке.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        public static string Parse(this IzumiPreconditionErrorMessage ibPreconditionErrorMessage) =>
            ibPreconditionErrorMessage.Localize();

        /// <summary>
        /// Возвращает локализированный текст сообщения об ошибке подставляя указанные значения.
        /// </summary>
        /// <param name="ibPreconditionErrorMessage">Сообщение об ошибке.</param>
        /// <param name="replacements">Значения которые необходимо подставить в текст.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        public static string Parse(this IzumiPreconditionErrorMessage ibPreconditionErrorMessage,
            params object[] replacements)
        {
            try
            {
                // пытаемся подставить указанные значения
                return string.Format(Parse(ibPreconditionErrorMessage), replacements);
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
        /// <param name="ibPreconditionErrorMessage">Сообщение об ошибке.</param>
        /// <returns>Локализированный текст сообщения об ошибке.</returns>
        private static string Localize(this IzumiPreconditionErrorMessage ibPreconditionErrorMessage) =>
            ibPreconditionErrorMessage switch
            {
                IzumiPreconditionErrorMessage.RequireRegistry =>
                    "Данная команда доступна только после регистрации в игровом мире. Напишите `!регистрация [игровое имя]` чтобы зарегестрироваться.",

                IzumiPreconditionErrorMessage.RequireRole =>
                    "Данная команда доступна только для пользователей с ролью «**{0}**».",

                IzumiPreconditionErrorMessage.RequireSeason =>
                    "Использовать данную команду можно лишь когда наступит сезон «**{0}**».",

                IzumiPreconditionErrorMessage.RequireWeather =>
                    "Использовать данную команду можно лишь когда погода станет **{0}**.",

                IzumiPreconditionErrorMessage.RequireLocationButYouInTransit =>
                    "Достаточно трудно делать что-то находясь внутри телеги. Советую сначала дождаться прибытия и лишь потом дать волю своим желаниям действовать.",

                IzumiPreconditionErrorMessage.RequireLocationButYouInAnother =>
                    "Так это ведь доступно только в **{0}**, а вы немного в другом городе... Кажется пора собирать вещи перед отправлением.",

                IzumiPreconditionErrorMessage.RequireLocationButYouExploringGarden =>
                    "Если прямо сейчас бросить исследование сада, то что будет потом? Пора уже научиться заканчивать начатое.",

                IzumiPreconditionErrorMessage.RequireLocationButYouExploringCastle =>
                    "Что-то мне подсказывает, что бригадир шахты которого мы уговорили пустить нас сюда для исследований - не очень то обрадуется если мы так быстро вернемся обратно, возможно стоит закончить тут, а потом пойти по остальным делам?",

                IzumiPreconditionErrorMessage.RequireLocationButYouFishing =>
                    "Тише-тише, сейчас нужно сосредочиться только на рыбалке, а после вкусного улова можно будет и другими делами заняться.",

                IzumiPreconditionErrorMessage.RequireLocationButYouFieldWatering =>
                    "Хэй, а как же семена? Вы ведь уже решили что их пора поливать, так давайте же закончим сначала это дело, а потом будем заниматься всем остальным.",

                IzumiPreconditionErrorMessage.RequireCasinoOpen =>
                    "Казино работает только с `18:00` до `06:00`, приходите в это время чтобы испытать свою удачу!",

                IzumiPreconditionErrorMessage.RequireNoDebuff =>
                    "Вы не можете это сделать, ведь из-за последствий вторжения ежедневного босса {0}",

                IzumiPreconditionErrorMessage.RequireEvent =>
                    "Эта команда доступна только во время события **«{0}»**.",

                IzumiPreconditionErrorMessage.RequirePremium =>
                    "Эта команда доступна только пользователям с премиум-статусом.",

                _ => throw new ArgumentOutOfRangeException(nameof(ibPreconditionErrorMessage),
                    ibPreconditionErrorMessage, null)
            };
    }
}
