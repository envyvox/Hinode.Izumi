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
        CasinoClosed,
        EventMayPicnicAnonsDesc,
        EventMayPicnicSpawnDesc,
        EventMayPicnicSpawnRewardFieldName,
        EventMayPicnicSpawnRewardFieldDesc,
        EventMayPicnicSpawnFooter,
        EventMayPicnicEndDesc,
        EventMayPicnicEndRewardDesc,
        EventMayStartDesc,
        EventMayStartPicnicFieldName,
        EventMayStartPicnicFieldDesc,
        EventTimeReduceTransitFieldName,
        EventTimeReduceTransitFieldDesc,
        EventMayStartFooter,
        EventMayEndDesc,
        LotteryWinner
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
                "Осталась всего **неделя** до наступления лета, а солнце уже начинает нещадно припекать.\n\n" +
                "Жарища иногда просто сводит с ума, птицы начинают щебетать все раньше, а спасительная прохлада вечерней тишины наступает все позже.\n\n" +
                "Но это из неприятного. В конце-концов, у каждой поры есть свои минусы и плюсы. Из крайне хорошего - наконец-то можно дать волю планам, которые я так долго строила на это лето! Теперь надо сделать так, чтобы ничего мне не помешало их осуществить. Хотя что-то мне подсказывает, что все равно что-то да пойдет не так...\n\n" +
                "И хотя планы планами, надо бы не забыть пару дел, с которыми побыстрее бы разобраться и со спокойной душой идти доставать из закромов свой купальник. На очереди срочным делом был вроде {0} **весенний урожай**, если мне память не изменяет. Надо бы его **собрать**, он же у нас важный, урожай бумажный, расти отказывается летом.\n\n" +
                "Хотя еще **неделя**, успеется. Воздух такой приятный, слабенький ветерок освежает. Так можно и усн...\n\n*Длинный росчерк в конце страницы намекает, что так и произошло.*",

            IzumiEventMessage.AutumnComing =>
                "Осень наступает.",

            IzumiEventMessage.WinterComing =>
                "Зима наступает.",

            IzumiEventMessage.BossNotify =>
                "Я только что слышала жуткие толчки, будто землятресение или чьи-то гигантские шаги...\n\n" +
                "Что бы это ни было, через полчаса оно уже будет в **{0}**.",

            IzumiEventMessage.BossHere =>
                "Ну что? Все готовы? Босс уже здесь!\n" +
                "**{0}** нуждается в вашей защите.\n\n" +
                "Нажмите на реакцию {1} и покажите этому монстру где сакура цветет!",

            IzumiEventMessage.BossHereFooter =>
                "У вас есть {0} минут на убийство босса.",

            IzumiEventMessage.BossNotKilled =>
                "Это ужасно, но нам нехватает людей чтобы отразить его нападение, будет разумнее на сегодня отступить...\n\n" +
                "*{0}*",

            IzumiEventMessage.ReputationAdded =>
                "{0} {1} репутации в **{2}**, ",

            IzumiEventMessage.BossKilled =>
                "Мы отлично справились, у босса не было ни шанса, но не стоит расслабляться. Завтра он наверняка захочет отомстить и вновь нападет на один из городов.",

            IzumiEventMessage.BossRewardNotify =>
                "Сегодня в **{0}** появился босс, который угрожал спокойной жизни местных жителей.\n\n" +
                "Однако искатели приключений, показав свою храбрость, помогли одолеть его.\n\n" +
                "В благодарность за помощь, жители собрали для них небольшую награду:\n{1}.",

            IzumiEventMessage.BossRewardFieldName =>
                "Ожидаемая награда за помощь",

            IzumiEventMessage.BossRewardReputation =>
                "{0} {1} репутации в **{2}**\n",

            IzumiEventMessage.CasinoOpen =>
                "Ну наконец-то на часах 18:00, надеюсь вы подкопили достаточно деньжат для меня.\n\n" +
                "Ох, вижу-вижу, вы хотите поскорее испытать свою удачу, тогда вперед, двери моего казино открыты. Готовьтесь попрощаться со своими сбережениями.",

            IzumiEventMessage.CasinoClosed =>
                "Пришла напомнить, что время 6 утра и пора закрываться. Поторопитесь, мне еще все пересчитывать и подготавливать казино к вечеру.\n\n" +
                "Вам же рекомендую выспаться, подзаработать денег, а уже вечером вернуться ко мне. Уверяю, что сегодня удача будет на вашей стороне ;)",

            IzumiEventMessage.EventMayPicnicAnonsDesc =>
                "В **{0}** через {1} намечается пикник! Самое время отправится туда, чтобы попробовать приготовленные жителями блюда, восстанавливающие {2} энергию.",

            IzumiEventMessage.EventMayPicnicSpawnDesc =>
                "Приветствую всех любителей приключений на нашем ежегодном майском пикнике! В этом году у нас хороший урожай, и мы решили угостить всех желающих.\n\n" +
                "Нажмите на реакцию {0} чтобы поучаствовать в пикнике.",

            IzumiEventMessage.EventMayPicnicSpawnRewardFieldName =>
                "Ожидаемая награда",

            IzumiEventMessage.EventMayPicnicSpawnRewardFieldDesc =>
                "Полное восстановление {0} энергии и {1} {2} {3}",

            IzumiEventMessage.EventMayPicnicSpawnFooter =>
                "У вас есть {0}, чтобы принять участие.",

            IzumiEventMessage.EventMayPicnicEndDesc =>
                "Ох, спасибо что приехали к нам сегодня, мы отлично провели время и набили свои животы. У нас осталось ещё много урожая, так что загляните к нам завтра в это же время." +
                "\n\nВ качестве подарка можете взять с собой {0} {1} {2}.",

            IzumiEventMessage.EventMayPicnicEndRewardDesc =>
                "В **{0}** прошел пикник, на котором любители приключений вместе с жителями отлично провели время, восстановили свою {1} энергию и получили {2} {3} {4} в качестве подарка.",

            IzumiEventMessage.EventMayStartDesc =>
                "Вот и наступил **месяц Май**, а значит пришло время для отличного отдыха на свежем воздухе.",

            IzumiEventMessage.EventMayStartPicnicFieldName =>
                "Время пикника",

            IzumiEventMessage.EventMayStartPicnicFieldDesc =>
                "{0} из **деревни «Мура»**, приглашает всех любителей приключений на ежедневный пикник, который он будет проводить по вечерам. Следите за обновлениями в <#{1}>, чтобы не пропустить!",

            IzumiEventMessage.EventTimeReduceTransitFieldName =>
                "Ускоренное перемещение",

            IzumiEventMessage.EventTimeReduceTransitFieldDesc =>
                "В течение события жители городов уделяют особенное внимание качеству дорог, поэтому в это время вы сможете перемещаться между локациями на **{0}%** быстрее!",

            IzumiEventMessage.EventMayStartFooter =>
                "Событие закончится в 00:00 часов, 10 мая.",

            IzumiEventMessage.EventMayEndDesc =>
                "Вот и подошло к концу **майское событие**, надеюсь, что все любители приключений успели насытиться угощениями жителей. Ну, а теперь нужно возвращаться к ежедневной рутине...\n\n" +
                "К счастью ненадолго, ведь уже в следующем месяце жители планируют провести еще одно событие, там и встретимся!",

            IzumiEventMessage.LotteryWinner =>
                "Все жители **столицы** только и говорят о везунчике {0} {1} **{2}**, который победил в {3} лотерее и получил {4} {5} {6}, вот бы и мне так везло...",

            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
