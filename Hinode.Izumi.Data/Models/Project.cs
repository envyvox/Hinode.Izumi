namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Чертеж.
    /// </summary>
    public class Project : EntityBase
    {
        /// <summary>
        /// Название чертежа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Стоимость чертежа.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Длительность строительства (в часах).
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Id требуемой постройки (не обязательно).
        /// </summary>
        public long? ReqBuildingId { get; set; }

        /// <summary>
        /// Требуемая постройка.
        /// </summary>
        public virtual Building Building { get; set; }
    }
}
