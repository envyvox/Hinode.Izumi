﻿using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.MasteryPropertyWebService.Models
{
    /// <summary>
    /// Свойства (настройки) мастерства.
    /// </summary>
    public class MasteryPropertyWebModel : EntityBaseModel
    {
        /// <summary>
        /// Категория свойства мастерства.
        /// </summary>
        public MasteryPropertyCategory PropertyCategory { get; set; }

        /// <summary>
        /// Свойство мастерства.
        /// </summary>
        public MasteryProperty Property { get; set; }

        /// <summary>
        /// Значение при 0 мастерства.
        /// </summary>
        public long Mastery0 { get; set; }

        /// <summary>
        /// Значение при 50 мастерства.
        /// </summary>
        public long Mastery50 { get; set; }

        /// <summary>
        /// Значение при 100 мастерства.
        /// </summary>
        public long Mastery100 { get; set; }

        /// <summary>
        /// Значение при 150 мастерства.
        /// </summary>
        public long Mastery150 { get; set; }

        /// <summary>
        /// Значение при 200 мастерства.
        /// </summary>
        public long Mastery200 { get; set; }

        /// <summary>
        /// Значение при 250 мастерства.
        /// </summary>
        public long Mastery250 { get; set; }
    }
}
