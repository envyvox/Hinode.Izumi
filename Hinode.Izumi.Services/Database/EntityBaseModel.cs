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
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Время последнего обновления записи.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
