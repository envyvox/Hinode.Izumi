using System;
using Hinode.Izumi.Framework.EF;

namespace Hinode.Izumi.Data.Models
{
    public class EntityBase: IEntityBase
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
