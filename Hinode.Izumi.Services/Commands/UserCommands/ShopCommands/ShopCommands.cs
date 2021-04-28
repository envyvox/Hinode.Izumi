using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands;
using Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands
{
    [Group("магазин"), Alias("shop")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ShopCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IShopBannerCommand _shopBannerCommand;
        private readonly IShopCertificateCommand _shopCertificateCommand;
        private readonly IShopProductCommand _shopProductCommand;
        private readonly IShopRecipeCommand _shopRecipeCommand;
        private readonly IShopSeedCommand _shopSeedCommand;
        private readonly IShopProjectCommand _shopProjectCommand;

        public ShopCommands(IShopBannerCommand shopBannerCommand, IShopCertificateCommand shopCertificateCommand,
            IShopProductCommand shopProductCommand, IShopRecipeCommand shopRecipeCommand,
            IShopSeedCommand shopSeedCommand, IShopProjectCommand shopProjectCommand)
        {
            _shopBannerCommand = shopBannerCommand;
            _shopCertificateCommand = shopCertificateCommand;
            _shopProductCommand = shopProductCommand;
            _shopRecipeCommand = shopRecipeCommand;
            _shopSeedCommand = shopSeedCommand;
            _shopProjectCommand = shopProjectCommand;
        }

        [Command("семян"), Alias("seeds")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopSeedTask() =>
            await _shopSeedCommand.Execute(Context);

        [Command("сертификатов"), Alias("certificates")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopCertificateTask() =>
            await _shopCertificateCommand.Execute(Context);

        [Command("баннеров"), Alias("banners")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopBannerTask() =>
            await _shopBannerCommand.Execute(Context);

        [Command("рецептов"), Alias("recipes")]
        [IzumiRequireLocation(Location.Garden), IzumiRequireNoDebuff(BossDebuff.GardenStop)]
        public async Task ShopRecipeTask() =>
            await _shopRecipeCommand.Execute(Context);

        [Command("продуктов"), Alias("products")]
        [IzumiRequireLocation(Location.Village), IzumiRequireNoDebuff(BossDebuff.VillageStop)]
        public async Task ShopProductTask() =>
            await _shopProductCommand.Execute(Context);

        [Command("чертежей"), Alias("projects")]
        [IzumiRequireLocation(Location.Seaport), IzumiRequireNoDebuff(BossDebuff.SeaportStop)]
        public async Task ShopProjectsTask() =>
            await _shopProjectCommand.Execute(Context);

        [Group("купить"), Alias("buy")]
        public class ShopBuyCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IShopBuyBannerCommand _shopBuyBannerCommand;
            private readonly IShopBuyProductCommand _shopBuyProductCommand;
            private readonly IShopBuyCertificateCommand _shopBuyCertificateCommand;
            private readonly IShopBuyRecipeCommand _shopBuyRecipeCommand;
            private readonly IShopBuySeedCommand _shopBuySeedCommand;
            private readonly IShopBuyProjectCommand _shopBuyProjectCommand;

            public ShopBuyCommands(IShopBuyBannerCommand shopBuyBannerCommand,
                IShopBuyProductCommand shopBuyProductCommand, IShopBuyCertificateCommand shopBuyCertificateCommand,
                IShopBuyRecipeCommand shopBuyRecipeCommand, IShopBuySeedCommand shopBuySeedCommand,
                IShopBuyProjectCommand shopBuyProjectCommand)
            {
                _shopBuyBannerCommand = shopBuyBannerCommand;
                _shopBuyProductCommand = shopBuyProductCommand;
                _shopBuyCertificateCommand = shopBuyCertificateCommand;
                _shopBuyRecipeCommand = shopBuyRecipeCommand;
                _shopBuySeedCommand = shopBuySeedCommand;
                _shopBuyProjectCommand = shopBuyProjectCommand;
            }

            [Command("семена"), Alias("seed")]
            public async Task ShopBuySeedTask(long seedId, long amount = 1) =>
                await _shopBuySeedCommand.Execute(Context, seedId, amount);

            [Command("сертификат"), Alias("certificate")]
            public async Task ShopBuyCertTask(long certificateId) =>
                await _shopBuyCertificateCommand.Execute(Context, certificateId);

            [Command("баннер"), Alias("banner")]
            public async Task ShopBuyBannerTask(long bannerId) =>
                await _shopBuyBannerCommand.Execute(Context, bannerId);

            [Command("рецепт"), Alias("recipe")]
            public async Task ShopBuyRecipeTask(long foodId) =>
                await _shopBuyRecipeCommand.Execute(Context, foodId);

            [Command("продукт"), Alias("product")]
            public async Task ShopBuyProductTask(long productId, long amount = 1) =>
                await _shopBuyProductCommand.Execute(Context, productId, amount);

            [Command("чертеж"), Alias("project")]
            public async Task ShopBuyProjectTask(long projectId) =>
                await _shopBuyProjectCommand.Execute(Context, projectId);
        }
    }
}
