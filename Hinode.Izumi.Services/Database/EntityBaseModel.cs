using System;

namespace Hinode.Izumi.Services.Database
{
    public class EntityBaseModel
    {
        /// <summary>
        /// Id записи.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Время создания записи.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Время последнего обновления записи.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
