﻿using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Сертификат.
    /// </summary>
    public enum Certificate
    {
        Rename = 1,
        FamilyRegistration = 2,
        FamilyRename = 3
    }

    public static class CertificateHelper
    {
        /// <summary>
        /// Возвращает локализированное название сертификата.
        /// </summary>
        /// <param name="certificate">Сертификат.</param>
        /// <returns>Локализированное название сертификата.</returns>
        public static string Localize(this Certificate certificate) => certificate switch
        {
            Certificate.Rename => "смена имени",
            Certificate.FamilyRegistration => "регистрация семьи",
            Certificate.FamilyRename => "смена названия семьи",
            _ => throw new ArgumentOutOfRangeException(nameof(certificate), certificate, null)
        };

        /// <summary>
        /// Возвращает локализированное описание сертификата.
        /// </summary>
        /// <param name="certificate">Сертификат.</param>
        /// <returns>Локализированное описание сертификата.</returns>
        public static string Description(this Certificate certificate) => certificate switch
        {
            Certificate.Rename =>
                "Позволяет сменить игровое имя на новое,\nдля этого напишите `!переименоваться [новое имя]`.",
            Certificate.FamilyRegistration =>
                "Позволяет зарегистрировать новую семью,\nдля этого напишите `!семья регистрация [новое имя]`.",
            Certificate.FamilyRename =>
                "Позволяет сменить название вашей семьи,\nдля этого напишите `!семья переименовать [новое имя]`.",
            _ => throw new ArgumentOutOfRangeException(nameof(certificate), certificate, null)
        };
    }
}
