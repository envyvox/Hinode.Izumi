namespace Hinode.Izumi.Data.Enums
{
    public enum Box : byte
    {
        Capital = 1,
        Garden = 2,
        Seaport = 3,
        Castle = 4,
        Village = 5
    }

    public static class BoxHelper
    {
        public static string Emote(this Box box) => "Box" + box;
    }
}
