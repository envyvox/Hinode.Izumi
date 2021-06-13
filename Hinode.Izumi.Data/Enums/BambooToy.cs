namespace Hinode.Izumi.Data.Enums
{
    public enum BambooToy : byte
    {
        Dragon = 1,
        Whale = 2,
        MiniPig = 3,
        Panda = 4,
        Elephant = 5
    }

    public static class BambooToyHelper
    {
        public static string Emote(this BambooToy bambooToy) => "Bamboo" + bambooToy;
    }
}
