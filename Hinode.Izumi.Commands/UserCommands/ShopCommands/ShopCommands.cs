using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands;
using Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands
{
    [CommandCategory(CommandCategory.Shop)]
    [Group("магазин"), Alias("shop")]
    [IzumiRequireRegistry]
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
        [Summary("Посмотреть ассортимент магазина семян")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopSeedTask() =>
            await _shopSeedCommand.Execute(Context);

        [Command("сертификатов"), Alias("certificates")]
        [Summary("Посмотреть ассортимент магазина сертификатов")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopCertificateTask() =>
            await _shopCertificateCommand.Execute(Context);

        [Command("баннеров"), Alias("banners")]
        [Summary("Посмотреть ассортимент магазина баннеров")]
        [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
        public async Task ShopBannerTask() =>
            await _shopBannerCommand.Execute(Context);

        [Command("рецептов"), Alias("recipes")]
        [Summary("Посмотреть ассортимент магазина рецептов")]
        [IzumiRequireLocation(Location.Garden), IzumiRequireNoDebuff(BossDebuff.GardenStop)]
        public async Task ShopRecipeTask() =>
            await _shopRecipeCommand.Execute(Context);

        [Command("продуктов"), Alias("products")]
        [Summary("Посмотреть ассортимент магазина продуктов")]
        [IzumiRequireLocation(Location.Village), IzumiRequireNoDebuff(BossDebuff.VillageStop)]
        public async Task ShopProductTask() =>
            await _shopProductCommand.Execute(Context);

        [Command("чертежей"), Alias("projects")]
        [Summary("Посмотреть ассортимент магазина чертежей")]
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
            [Summary("Приобрести указанные семена")]
            [CommandUsage("!магазин купить семена 21 5", "!магазин купить семена 2")]
            [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
            public async Task ShopBuySeedTask(
                [Summary("Номер семян")] long seedId,
                [Summary("Количество")] long amount = 1) =>
                await _shopBuySeedCommand.Execute(Context, seedId, amount);

            [Command("сертификат"), Alias("certificate")]
            [Summary("Приобрести указанный сертификат")]
            [CommandUsage("!магазин купить сертификат 1", "!магазин купить сертификат 3")]
            [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
            public async Task ShopBuyCertTask(
                [Summary("Номер сертификата")] long certificateId) =>
                await _shopBuyCertificateCommand.Execute(Context, certificateId);

            [Command("баннер"), Alias("banner")]
            [Summary("Приобрести указанный баннер")]
            [CommandUsage("!магазин купить баннер 1", "!магазин купить баннер 5")]
            [IzumiRequireLocation(Location.CapitalShop), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
            public async Task ShopBuyBannerTask(
                [Summary("Номер баннера")] long bannerId) =>
                await _shopBuyBannerCommand.Execute(Context, bannerId);

            [Command("рецепт"), Alias("recipe")]
            [Summary("Приобрести указанный рецепт")]
            [CommandUsage("!магазин купить рецепт 1", "!магазин купить рецепт 5")]
            [IzumiRequireLocation(Location.Garden), IzumiRequireNoDebuff(BossDebuff.GardenStop)]
            public async Task ShopBuyRecipeTask(
                [Summary("Номер рецепта")] long foodId) =>
                await _shopBuyRecipeCommand.Execute(Context, foodId);

            [Command("продукт"), Alias("product")]
            [Summary("Приобрести указанные продукты")]
            [CommandUsage("!магазин купить продукт 1 5", "!магазин купить продукт 2")]
            [IzumiRequireLocation(Location.Village), IzumiRequireNoDebuff(BossDebuff.VillageStop)]
            public async Task ShopBuyProductTask(
                [Summary("Номер продукта")] long productId,
                [Summary("Количество")] long amount = 1) =>
                await _shopBuyProductCommand.Execute(Context, productId, amount);

            [Command("чертеж"), Alias("project")]
            [Summary("Приобрести указанный чертеж")]
            [CommandUsage("!магазин купить чертеж 1", "!магазин купить чертеж 5")]
            [IzumiRequireLocation(Location.Seaport), IzumiRequireNoDebuff(BossDebuff.SeaportStop)]
            public async Task ShopBuyProjectTask(
                [Summary("Номер чертежа")] long projectId) =>
                await _shopBuyProjectCommand.Execute(Context, projectId);
        }

        [Group("события"), Alias("event")]
        public class ShopEventCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IShopEventCommand _shopEventCommand;
            private readonly IShopBuyEventCommand _shopBuyEventCommand;

            public ShopEventCommands(IShopEventCommand shopEventCommand, IShopBuyEventCommand shopBuyEventCommand)
            {
                _shopEventCommand = shopEventCommand;
                _shopBuyEventCommand = shopBuyEventCommand;
            }

            [Command]
            [Summary("Посмотреть ассортимент магазина события")]
            public async Task ShopEventTask() =>
                await _shopEventCommand.Execute(Context);

            [Command("купить"), Alias("buy")]
            [Summary("Приобрести указанный товар")]
            [CommandUsage("!магазин события купить 1", "!магазин события купить 2 Холли")]
            public async Task ShopEventBuyTask(
                [Summary("Номер предмета")] long itemId,
                [Summary("Игровое имя")] string namePattern = null) =>
                await _shopBuyEventCommand.Execute(Context, itemId, namePattern);
        }
    }
}
