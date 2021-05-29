using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.GameServices.CardService.Records
{
    public record CardRecord(long Id, CardRarity Rarity, Effect Effect, string Name, string Anime, string Url)
    {
        public CardRecord() : this(default, default, default, default, default, default)
        {
        }
    }
}
