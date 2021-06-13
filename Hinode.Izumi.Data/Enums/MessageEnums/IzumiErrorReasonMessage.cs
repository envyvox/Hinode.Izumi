using System;

namespace Hinode.Izumi.Data.Enums.MessageEnums
{
    public enum IzumiErrorReasonMessage : byte
    {
        SomethingGoneWrong = 0,
        UnknownCommand = 1,
        ParseFailed = 2,
        BadArgCount = 3,
        ObjectNotFound = 4,
        MultipleMatches = 5
    }

    public static class IzumiErrorReasonMessageHelper
    {
        public static string Localize(this IzumiErrorReasonMessage errorReasonMessage) => errorReasonMessage switch
        {
            IzumiErrorReasonMessage.SomethingGoneWrong =>
                "Ой, кажется что-то пошло не так...",

            IzumiErrorReasonMessage.UnknownCommand =>
                "Я не уверена что вы хотите сделать, советую заглянуть в `!помощь` чтобы узнать о доступных командах.",

            IzumiErrorReasonMessage.ParseFailed =>
                "После сложных математических вычеслений я пришла к ошибке. Какой? Не знаю, просто ошибке. Это вы виноваты!",

            IzumiErrorReasonMessage.BadArgCount =>
                "Кажется вы неправильно указали аргументы после команды, советую заглянуть в `!помощь` чтобы узнать о доступных командах и как их правильно использовать.",

            IzumiErrorReasonMessage.ObjectNotFound =>
                "После сложных математических вычеслений я пришла к выводу что такого объекта не существует. Какого объекта? Не знаю, это ведь вы искали его...",

            IzumiErrorReasonMessage.MultipleMatches =>
                "Уф, кажется тут подходит несколько вариантов, я за вас решать не собираюсь!",

            _ => throw new ArgumentOutOfRangeException(nameof(errorReasonMessage), errorReasonMessage, null)
        };
    }
}
