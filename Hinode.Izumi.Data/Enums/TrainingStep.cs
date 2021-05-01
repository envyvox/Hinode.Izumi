﻿using System;

namespace Hinode.Izumi.Data.Enums
{
    public enum TrainingStep
    {
        None = 0,
        CheckProfile = 1,
        CheckWorldInfo = 2,
        CheckTransits = 3,
        TransitToSeaport = 4,
        CompleteFishing = 5,
        CheckInventory = 6,
        TransitToGarden = 7,
        CompleteExploreGarden = 8,
        CheckCookingList = 9,
        CookFriedEgg = 10,
        EatFriedEgg = 11,
        TransitToCastle = 12,
        CompleteExploreCastle = 13,
        TransitToVillage = 14,
        CheckHarvestingField = 15,
        TransitToCapital = 16,
        TransitToCapitalShop = 17,
        CheckCapitalSeedShop = 18,
        TransitToCapitalAfterSeedShop = 19,
        TransitToCapitalMarket = 20,
        CheckCapitalMarket = 21,
        TransitToCapitalAfterMarket = 22,
        TransitToCapitalCasino = 23,
        TransitToCapitalAfterCasino = 24,
        CheckContracts = 25,
        CheckCards = 26,
        AddCardToDeck = 27,
        CheckEffects = 28,
        Completed = 29
    }

    public static class TrainingStepHelper
    {
        /// <summary>
        /// Возвращает локализирированное название шага обучения.
        /// </summary>
        /// <param name="step">Шаг обучения.</param>
        /// <returns>Локализирированное название шага обучения.</returns>
        public static string Name(this TrainingStep step) => step switch
        {
            TrainingStep.None => "",
            TrainingStep.CheckProfile => "Приветствие",
            TrainingStep.CheckWorldInfo => "Знакомство с миром",
            TrainingStep.CheckTransits => "Время путешествовать",
            TrainingStep.TransitToSeaport => "В путь",
            TrainingStep.CompleteFishing => "Отличный день для рыбалки",
            TrainingStep.CheckInventory => "Рюкзак путешественника",
            TrainingStep.TransitToGarden => "Отправляемся в cад",
            TrainingStep.CompleteExploreGarden => "Прогулка по cаду",
            TrainingStep.CheckCookingList => "Восстановить силы",
            TrainingStep.CookFriedEgg => "Кулинарный вызов",
            TrainingStep.EatFriedEgg => "Вкусный перекус",
            TrainingStep.TransitToCastle => "Дальнее путешествие",
            TrainingStep.CompleteExploreCastle => "Вглубь шахт",
            TrainingStep.TransitToVillage => "Отправляемся в деревню",
            TrainingStep.CheckHarvestingField => "Безграничные поля",
            TrainingStep.TransitToCapital => "Возвращение в столицу",
            TrainingStep.TransitToCapitalShop => "Кто этот ваш Торедо?",
            TrainingStep.CheckCapitalSeedShop => "Шоппинг",
            TrainingStep.TransitToCapitalAfterSeedShop => "Изу, хватит все скупать!",
            TrainingStep.TransitToCapitalMarket => "Куда на этот раз?",
            TrainingStep.CheckCapitalMarket => "Торговые ряды",
            TrainingStep.TransitToCapitalAfterMarket => "Где тут выход?",
            TrainingStep.TransitToCapitalCasino => "Коун Джо",
            TrainingStep.TransitToCapitalAfterCasino => "Давай еще разочек?",
            TrainingStep.CheckContracts => "Опять работать?",
            TrainingStep.CheckCards => "Первая карточка",
            TrainingStep.AddCardToDeck => "Чоппер, я выбираю тебя!",
            TrainingStep.CheckEffects => "Я чувствую эту силу",
            TrainingStep.Completed => "До новых встреч!",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
        };

        /// <summary>
        /// Возвращает локализированное описание шага обучения.
        /// </summary>
        /// <param name="step">Шаг обучения.</param>
        /// <returns>Локализированное описание шага обучения.</returns>
        public static string Description(this TrainingStep step) => step switch
        {
            TrainingStep.None => "",

            TrainingStep.CheckProfile =>
                "Я тебя раньше тут не видела. Ох, где мои манеры. Привет! Меня зовут Изуми, я путешествую по миру и завожу новых друзей. Какое-то время назад я остановилась здесь, уж очень мне тут понравилось. Расскажешь о себе?\n\n" +
                "Напиши `!профиль` для его просмотра, а если хочешь добавить немного информации о себе, то напиши\n`!информация [текст]`.",

            TrainingStep.CheckWorldInfo =>
                "Приятно познакомиться! Я всегда готова помочь своим новым знакомым, поэтому проведу тебе небольшую экскурсию!\n\n" +
                "Сейчас мы находимся в **столице «Эдо»**, можешь почитать об ее истории в канале <#750624440391434321>. Но начнем мы не с неё. Для начала дам тебе один совет.\n\n" +
                "Напиши `!мир`, чтобы узнать информацию о погоде, времени суток и прочее. Это пригодится тебе в дальнейшем.",

            TrainingStep.CheckTransits =>
                "Отлично, а теперь за дело!\n\n" +
                "Если ты тут хочешь задержаться, то тебе понадобятся <:Ien:813145486428995635> деньги. Нужно что-то придумать...\n\n" +
                "Напиши `!отправления`.",

            TrainingStep.TransitToSeaport =>
                "О, кажется у меня есть идея!\n" +
                "Поехали в **портовый город «Нагоя»**!\n\n" +
                "Напиши `!отправиться 3`.",

            TrainingStep.CompleteFishing =>
                "Ох, обязательно прочитай историю об этом городе в канале <#750624448004227113>, она такая захватывающая!\n\n" +
                "В **портовом городе «Нагоя»** кланы и семьи отправляются в исследования на своих кораблях, ну, а нам новичкам, чтобы заработать <:Ien:813145486428995635> денег остается только `!рыбачить`.",

            TrainingStep.CheckInventory =>
                "По рассказам жителей тут водится огромное количество видов рыб, и от ее редкости зависит цена, которую дает местный рыбак.\n\n" +
                "Но это еще не все, рыбалка не такое простое дело. Некоторых рыб нужно вылавливать в определенную погоду и время суток, вся информацию можно найти в `!помощь рыба`.\n\n" +
                "А ты сколько поймал? Загляни в `!инвентарь`, а если же хочешь узнать более подробную информацию то напиши `!инвентарь рыба`.",

            TrainingStep.TransitToGarden =>
                "Да уж, что-то мне сегодня не везет с уловом. Ну, а ты если что-то поймал, то можешь продать рыбаку.\n\n" +
                "Цены на рыбку ты можешь посмотреть с помощью команды `!рыбак`, там же он тебе расскажет как ее продать.\n\n" +
                "Такс, куда мы отправимся теперь... Точно! Я была в **цветущем саду «Кайраку-Эн»**, там так красиво! Тебе обязательно нужно там побывать.\n\n" +
                "Напиши `!отправиться 2`.",

            TrainingStep.CompleteExploreGarden =>
                "Помимо красоты тут есть еще и полезные ресурсы, тебе же надо будет возводить собственные постройки или ты настолько любишь природу?\n\n" +
                "Кстати, на эти ресурсы в столице бывает спрос, так что стоит собрать побольше.\n\n" +
                "Напиши `!исследовать сад`.\n\n" +
                "А пока прочитай <#750624444078227497> **цветущего сада «Кайраку-Эн»**, оно просто волшебное!",

            TrainingStep.CheckCookingList =>
                "После тяжелой работы необходимо восстановить <:Energy:835868948561002546> энергию, ведь именно от ее количества зависит как быстро ты будешь действовать. " +
                "Чем меньше <:Energy:835868948561002546> энергии - тем больше времени тебе понадобиться чтобы выполнить действие.\n\n" +
                "Напиши `!приготовление`.",

            TrainingStep.CookFriedEgg =>
                "Тут будут собраны все твои рецепты блюд, а сейчас самое время приготовить <:FriedEgg:813146110612340747> яичницу!\n\n" +
                "На этот раз я подарю тебе <:Recipe:813150911530401844> рецепт из категории начинающего повара и <:Egg:813145655316971581> необходимые ингредиенты, однако в будущем тебе придется покупать <:Recipe:813150911530401844> рецепты и добывать все ингредиенты самостоятельно.\n\n" +
                "А теперь напиши `!приготовить 4 1`.\n\n" +
                "*4 это номер <:FriedEgg:813146110612340747> яичницы, а 1 это количество.*",

            TrainingStep.EatFriedEgg =>
                "А у тебя неплохо получилось для первого раза.\n" +
                "Чем больше ты будешь практиковаться - тем больше наберешь мастерства <:Cooking:813150094362804285> «Кулинария» и тем сложнее и вкуснее блюда будут доступны для приготовления.\n\n" +
                "Время перекусить и восстановить <:Energy:835868948561002546> энергию.\n" +
                "Напиши `!съесть яичница`.",

            TrainingStep.TransitToCastle =>
                "Отлично, держи еще <:FriedEgg:813146110612340747> 20 яичниц на первое время.\n" +
                "После **цветущего сада «Кайраку-Эн»** мы с тобой отправимся в не менее загадочное место.\n\n" +
                "Я говорю о **древнем замке «Химэдзи»**.\n" +
                "Ох, мне уже жутко!\n\n" +
                "Напиши `!отправиться 4`.",

            TrainingStep.CompleteExploreCastle =>
                "Запомни, мы сюда приехали только за ресурсами!\n\n" +
                "Напиши `!исследовать шахту`.\n\n" +
                "А пока можешь почитать <#750624451640557638> **древнего замка «Химэдзи»**, лично у меня аж мурашки пробежали!",

            TrainingStep.TransitToVillage =>
                "*Устало села на пенёк*\n" +
                "А ты неплохо справился. Ну у меня просто совершенно другие таланты! Я... Неплохо танцую, например!\n\n" +
                "О чем это я, ах да, теперь нам нужно отправиться в живописную деревушку Мура. Поехали?\n\n" +
                "Напиши `!отправиться 5`.",

            TrainingStep.CheckHarvestingField =>
                "Давай немного расскажу зачем тебе вообще нужны ресурсы.\n\n" +
                "Во-первых, это еще один небольшой способ подзаработать, но об этом позже.\n\n" +
                "Во-вторых, ресурсы понадобятся тебе для различных построек, но сначала их нужно переработать и...\n\n" +
                "Ой, смотри какие пейзажи! Это значит, что мы уже на месте. Кстати, наверняка ты уже понял, где можно почитать про деревню. В **деревне «Мура»** есть много свободных участков, один их них ты можешь купить и выращивать урожай!\n\n" +
                "Напиши `!участок`.",

            TrainingStep.TransitToCapital =>
                "Так-с, здесь мы были, там побывали.\n\n" +
                "Ну что же, пора возвращаться в **столицу «Эдо»** - самый людный город, не зря же это столица.\n\n" +
                "Напиши `!отправиться 1`.",

            TrainingStep.TransitToCapitalShop =>
                "Давай пройдемся по городу.\n" +
                "Для начала мы заглянем к **Торедо**.\n\n" +
                "Напиши `!отправиться 12`.",

            TrainingStep.CheckCapitalSeedShop =>
                "Здесь продаются семена посезонно, но всегда есть из чего выбрать. Можешь сам убедиться.\n\n" +
                "Напиши `!магазин семян`.",

            TrainingStep.TransitToCapitalAfterSeedShop =>
                "Глаза разбегаются, пошли скорее отсюда, пока я все <:Ien:813145486428995635> деньги тут не оставила!\n\n" +
                "Напиши `!отправиться 1`.",

            TrainingStep.TransitToCapitalMarket =>
                "Ну люблю я шоппинг, ничего не могу поделать.\n\n" +
                "Хм, теперь давай отправимся на **рынок**.\n\n" +
                "Напиши `!отправиться 11`.",

            TrainingStep.CheckCapitalMarket =>
                "На рынке ты можешь как купить нужные ресурсы и урожай, так и продать ненужные.\n\n" +
                "Подробнее почитай в `!рынок`.",

            TrainingStep.TransitToCapitalAfterMarket =>
                "Ну вроде здесь тоже осмотрелись, можно уходить.\n\n" +
                "Напиши `!отправиться 1`.",

            TrainingStep.TransitToCapitalCasino =>
                "А теперь я покажу тебе самое любимое место для людей, которые любят азарт, в том числе и меня!\n\n" +
                "Уже догадался? Да, это **казино**. Пошли же скорей!\n\n" +
                "Напиши `!отправиться 10`.",

            TrainingStep.TransitToCapitalAfterCasino =>
                "Здесь проводится <:LotteryTicket:813150225597202474> `!лотерея`, где у тебя есть шанс\nсорвать <:Ien:813145486428995635> куш.\n\n" +
                "А если хочешь испытать свою удачу прямо сейчас,\nто напиши `!ставка [сумма]`.\n\nНу все, пойдем скорее на воздух, а то у меня начинает кружиться голова.\n\nНапиши `!отправиться 1`.",

            TrainingStep.CheckContracts =>
                "Если у тебя наступили <:Ien:813145486428995635> финансовые трудности, то в любом городе жители могут предложить тебе небольшую работу с соответствующей оплатой.\n\n" +
                "Напиши `!контракты`.",

            TrainingStep.CheckCards =>
                "А теперь время поговорить о самом главном - карточках!\n\n" +
                "Я подготовила для тебя небольшой подарок на эту тему, особую карточку «**Тони Тони Чоппер**».\n\n" +
                "Загляни в свои `!карточки`.",

            TrainingStep.AddCardToDeck =>
                "Помимо своей декоративной составляющей - все карточки обладают особыми эффектами.\n\n" +
                "Однако получить эффект от карточки можно только если добавить ее в свою <:CardDeck:813150225237016636> колоду.\n\n" +
                "Напиши `!колода добавить 10`.",

            TrainingStep.CheckEffects =>
                "Теперь, когда карточка находится в вашей колоде, вы чувствуете как ее эффект воздействует на вас.\n\n" +
                "Однако, стоить помнить что в колоду можно добавить лишь 5 карточек одновременно, а значит что в будущем придется выбирать.\n\n" +
                "Давайте посмотрим ваши `!эффекты`.",

            TrainingStep.Completed =>
                "Эффекты бывают двух типов - временные, которые можно получить съев еду и постоянные, получаемые от карточек.\n" +
                "Подробнее об этом всем лучше прочитать в `!помощь эффекты`, ведь там много различных нюансов.\n\n" +
                "Ну, что же, друг, я показала тебе все что нужно, чтобы ты смог обустроиться здесь. Держи <:Ien:813145486428995635> 1000 иен, потрать их с умом!\n\n" +
                "Далее дело за тобой, но я всегда буду на связи. Буду рада тебе помочь, стоит тебе только написать `!помощь`.\n\n" +
                "И еще кое-что! У меня есть <#750624434179801108>, где я буду рассказывать обо всем интересном, что будет происходить в мире **Hinode**, не забывай читать!",

            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null)
        };
    }
}
