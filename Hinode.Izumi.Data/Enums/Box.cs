namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Коробка с подарком внутри.
    /// </summary>
    public enum Box
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class BoxHelper
    {
        /// <summary>
        /// Возвращает название иконки коробки.
        /// </summary>
        /// <param name="box">Коробка.</param>
        /// <returns>Название иконки коробки.</returns>
        public static string Emote(this Box box) => "Box" + box;
    }
}
