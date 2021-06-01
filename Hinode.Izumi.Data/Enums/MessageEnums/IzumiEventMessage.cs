using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
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
        LotteryWinner,
        EventJuneSkyLanternAnons,
        EventJuneSkyLanternBeginDesc,
        EventJuneSkyLanternBeginRewardFieldName,
        EventJuneSkyLanternBeginRewardFieldDesc,
        EventJuneSkyLanternBeginFooter,
        EventJuneSkyLanternEndDesc,
        EventJuneSkyLanternEndRewardDesc,
        EventJuneStartDesc,
        EventJuneEndDesc
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
                "{0} {1} репутации в **{2}**",

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

            IzumiEventMessage.EventJuneSkyLanternAnons =>
                "В **{0}** через {1} намечается запуск небесных фонариков!\n\nСамое время отправится туда, чтобы принять участие в таком захватывающем событии и отдохнуть, восстановив {2} энергию.\n\nА еще, **{3}**, угощает всех посетителей мероприятия, так что успейте получить свои {4} {5} {6}.",

            IzumiEventMessage.EventJuneSkyLanternBeginDesc =>
                "Мы закончили приготовления и подготовили для всех бесплатные небесные фонарики, которые вы можете запустить в небо!\n\nНажмите на реакцию {0} чтобы поучаствовать в запуске небесных фонариков.",

            IzumiEventMessage.EventJuneSkyLanternBeginRewardFieldName =>
                "Ожидаемая награда",

            IzumiEventMessage.EventJuneSkyLanternBeginRewardFieldDesc =>
                "Полное восстановление {0} энергии и {1} {2} {3}",

            IzumiEventMessage.EventJuneSkyLanternBeginFooter =>
                "У вас есть {0} на запуск небесного фонарика.",

            IzumiEventMessage.EventJuneSkyLanternEndDesc =>
                "Это было просто незабываемо, спасибо всем кто пришел поучаствовать.\nВ качестве подарка за вашу активность, возьмите эти {0} {1} {2}.",

            IzumiEventMessage.EventJuneSkyLanternEndRewardDesc =>
                "В **{0}** прошел запуск небесных фонариков, на котором любители приключений вместе с жителями отлично провели время, восстановили свою {1} энергию и получили {2} {3} {4} в качестве подарка.",

            IzumiEventMessage.EventJuneStartDesc =>
                "Сегодня, прогуливаясь по **цветущему саду**, вдали я увидела маленький светящийся комочек. Он казался таким крохотным, но таким мягким, мне очень захотелось его потрогать. Я шла за ним, пытаясь не спугнуть, и, когда он остановился, я увидела нечто **волшебное**.\n\n" +
                "Огромное количество маленьких комочков кружились будто в вальсе. После каждого существа оставался еле заметный шлейф, от которого казалось, что это маленькие светящиеся кометы.\n\n" +
                "После увиденного я вспомнила, что в детстве мне рассказывали о чудных и одновременно чудесных светлячках, которые прилетают оповестить о **начале лета**.\n\n" +
                "Как же хочется, что бы каждый их увидел! Если я правильно помню, то светлячки проведут здесь весь **последний день** весны.\n\n" +
                "Так-так-так, а раз завтра уже лето... Ой-ёй... Я ведь совсем забыла рассказать о том, что попросила передать **Нари**! Ну хотя бы тебе, дневник, расскажу.\n\n" +
                "После нападения монстров на сад было порушено очень много деревьев, в том числе и ее любимый {0} бамбук (который сейчас под ее надежной защитой).\n" +
                "И она совместно с жителями **сада** решила делать из него игрушки! Поэтому {0} бамбук, который можно найти исследуя сад, можно обменять на {1} игрушки! Вот же круто, можно собрать целую {2} {3} {4} {5} коллекцию!\n\n" +
                "Ну, а самое главное - вечером намечается запуск небесных фонариков!\n\nДневник, это считается, что я выполнила просьбу?",

            IzumiEventMessage.EventJuneEndDesc =>
                "Вот и подошла к концу первая неделя лета... Это было так быстро! Вот бы подольше задержаться в саду, но ноги уже тянут меня в новые путешествия!\n" +
                "За это время произошло столько-столько всего нового и невероятного, я сама сделала себе игрушку, и даже подарила одну маленькой девочке - Ханако. " +
                "Она была очень счастлива. Когда я спускалась по лестнице, стук камней в моей сумке был так громок что птицы взлетели ввысь, и устремились в небеса.\n" +
                "Может и мне уже пора? Ведь летний шелест ветерка такой заманчивый... Ай ладно! Нечего сидеть на месте, Дорогой дневник, до скорых встреч!",

            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
