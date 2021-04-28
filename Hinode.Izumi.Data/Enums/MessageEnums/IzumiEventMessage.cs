using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
    /// <summary>
    /// Сообщения о событиях.
    /// </summary>
    public enum IzumiEventMessage
    {
        DiaryAuthorField,
        SpringComing,
        SummerComing,
        AutumnComing,
        WinterComing,
        BossNotify,
        BossHere,
        BossHereFooter,
        BossNotKilled,
        ReputationAdded,
        BossKilled,
        BossRewardNotify,
        BossRewardFieldName,
        BossRewardReputation,
        CasinoOpen,
        CasinoClosed
    }

    public static class IzumiEventMessageHelper
    {
        public static string Parse(this IzumiEventMessage message) => message.Localize();

        public static string Parse(this IzumiEventMessage message, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(message), replacements);
            }
            catch (FormatException)
            {
                return "`Возникла ошибка вывода ответа. Пожалуйста, покажите это Холли.`";
            }
        }

        /// <summary>
        /// Возвращает локалазированную строку сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Локализированная строка сообщения.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static string Localize(this IzumiEventMessage message) => message switch
        {
            IzumiEventMessage.DiaryAuthorField =>
                "Дорогой дневник,",

            IzumiEventMessage.SpringComing =>
                "Весна наступает.",

            IzumiEventMessage.SummerComing =>
                "Лето наступает.",

            IzumiEventMessage.AutumnComing =>
                "Осень наступает.",

            IzumiEventMessage.WinterComing =>
                "Зима наступает.",

            IzumiEventMessage.BossNotify =>
                "Я только что слышала жуткие толчки, будто землятресение или чьи-то гигантские шаги...\n\nЧто бы это ни было, через полчаса оно уже будет в **{0}**.",

            IzumiEventMessage.BossHere =>
                "Ну что? Все готовы? Босс уже здесь!\n**{0}** нуждается в вашей защите.\n\nПокажем этому монстру где сакура цветет!",

            IzumiEventMessage.BossHereFooter =>
                "У вас есть {0} минут на убийство босса.",

            IzumiEventMessage.BossNotKilled =>
                "Это ужасно, но нам нехватает людей чтобы отразить его нападение, будет разумнее на сегодня отступить...\n\n*{0}*",

            IzumiEventMessage.ReputationAdded =>
                "{0} {1} репутации в **{2}**, ",

            IzumiEventMessage.BossKilled =>
                "Мы отлично справились, у босса не было ни шанса, но не стоит расслабляться. Завтра он наверняка захочет отомстить и вновь нападет на один из городов.",

            IzumiEventMessage.BossRewardNotify =>
                "Сегодня в **{0}** появился босс, который угрожал спокойной жизни местных жителей.\n\nОднако искатели приключений, показав свою храбрость, помогли одолеть его.\n\nВ благодарность за помощь, жители собрали для них небольшую награду:\n{1}.",

            IzumiEventMessage.BossRewardFieldName =>
                "Ожидаемая награда за помощь",

            IzumiEventMessage.BossRewardReputation =>
                "{0} {1} репутации в **{2}**\n",

            IzumiEventMessage.CasinoOpen =>
                "Ну наконец-то на часах 18:00, надеюсь вы подкопили достаточно деньжат для меня.\n\nОх, вижу-вижу, вы хотите поскорее испытать свою удачу, тогда вперед, двери моего казино открыты. Готовьтесь попрощаться со своими сбережениями.",

            IzumiEventMessage.CasinoClosed =>
                "Пришла напомнить, что время 6 утра и пора закрываться. Поторопитесь, мне еще все пересчитывать и подготавливать казино к вечеру.\n\nВам же рекомендую выспаться, подзаработать денег, а уже вечером вернуться ко мне. Уверяю, что сегодня удача будет на вашей стороне ;)",

            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
