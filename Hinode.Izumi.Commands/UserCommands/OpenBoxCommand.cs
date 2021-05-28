using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProductService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Box)]
    [IzumiRequireRegistry]
    public class OpenBoxCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        private Dictionary<string, EmoteRecord> _emotes;
        private readonly Random _random = new();

        public OpenBoxCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("открыть"), Alias("open")]
        [Summary("Открыть указанные коробки")]
        [CommandUsage("!открыть 1 сбережение", "!открыть 5 лотков")]
        public async Task OpenBoxTask(
            [Summary("Количество")] long amount,
            [Summary("Название коробки")] [Remainder]
            string boxName)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем локализацию этой коробки
            var localization = await _local.GetLocalizationByLocalizedWord(InventoryCategory.Box, boxName);
            // получаем коробку
            var box = (Box) localization.ItemId;
            // получаем коробки пользователя
            var userBox = await _mediator.Send(new GetUserBoxQuery((long) Context.User.Id, box));

            // проверяем достаточно ли у пользователя коробок
            if (userBox.Amount < amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.OpenBoxWrongAmount.Parse(
                    _emotes.GetEmoteOrBlank(box.Emote()), _local.Localize(box.ToString(), 5))));
            }
            else
            {
                // открываем коробки и получаем локализированную строку награды
                var reward = box switch
                {
                    Box.Capital => await OpenCapitalBox(amount),
                    Box.Garden => await OpenGardenBox(amount),
                    Box.Seaport => await OpenSeaportBox(amount),
                    Box.Castle => await OpenCastleBox(amount),
                    Box.Village => await OpenVillageBox(amount),
                    _ => throw new ArgumentOutOfRangeException()
                };

                // забираем коробки у пользователя
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Box, box.GetHashCode(), amount));

                var embed = new EmbedBuilder()
                    // подтверждаем открытие коробок
                    .WithDescription(IzumiReplyMessage.OpenBoxDesc.Parse(
                        _emotes.GetEmoteOrBlank(box.Emote()), amount, _local.Localize(box.ToString(), amount)))
                    // отображаем полученную награду
                    .AddField(IzumiReplyMessage.OpenBoxFieldName.Parse(),
                        reward.Length < 1024 ? reward : IzumiReplyMessage.OpenBoxOutOfLimit.Parse());

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                await Task.CompletedTask;
            }
        }

        private async Task<string> OpenCapitalBox(long amount)
        {
            // получаем минимальное количество валюты получаемое из коробки
            var minAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxCapitalMinAmount));
            // получаем максимальное количество валюты получаемое из коробки
            var maxAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxCapitalMaxAmount));

            // считаем финальное количество полученной валюты
            long finalAmount = 0;
            for (var i = 0; i < amount; i++) finalAmount += _random.Next(minAmount, maxAmount);

            // добавляем пользователю валюту
            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), finalAmount));

            // возвращаем локализированную строку награды
            return
                $"{_emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {finalAmount} {_local.Localize(Currency.Ien.ToString(), finalAmount)}";
        }

        private async Task<string> OpenGardenBox(long amount)
        {
            // получаем собирательские ресурсы
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(Location.Garden));

            // заполняем строку полученных ресурсов
            var finalString = string.Empty;

            foreach (var gathering in gatherings)
            {
                // считаем финальное количество полученного ресурса
                long finalAmount = 0;
                for (var i = 0; i < amount; i++)
                {
                    // получаем шанс сбора этого ресурса
                    var gatheringChance = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringChance)))
                        .MasteryMaxValue(0);
                    // получаем шанс удвоенного сбора ресурса
                    var gatheringDoubleChance = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringDoubleChance)))
                        .MasteryMaxValue(0);
                    // получаем количество собираемого ресурса
                    var gatheringAmount = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringAmount)))
                        .MasteryMaxValue(0);
                    // считаем финальное количество ресурсов
                    var successAmount = await _mediator.Send(
                        new GetSuccessAmountQuery(gatheringChance, gatheringDoubleChance, gatheringAmount));

                    // если пользователь не получил ресурс - пропускаем
                    if (successAmount <= 0) continue;

                    // добавляем в общее количество
                    finalAmount += successAmount;
                }

                // если пользователь не получил ресурс - пропускаем
                if (finalAmount <= 0) continue;

                // добавляем ресурс пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Gathering, gathering.Id, finalAmount));

                // добавляем локализированную строку
                finalString +=
                    $"{_emotes.GetEmoteOrBlank(gathering.Name)} {finalAmount} {_local.Localize(gathering.Name, finalAmount)}, ";
            }

            // возвращаем локализированную строку награды
            return finalString.Remove(finalString.Length - 2);
        }

        private async Task<string> OpenSeaportBox(long amount)
        {
            // получаем минимальное количество рыбы получаемое из коробки
            var minAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxSeaportMinAmount));
            // получаем максимальное количество рыбы получаемое из коробки
            var maxAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxSeaportMaxAmount));
            // получаем редкость рыбы получаемой из коробки
            var fishRarity = (FishRarity) await _mediator.Send(new GetPropertyValueQuery(Property.BoxSeaportRarity));

            // заполняем строку полученной рыбы
            var finalString = string.Empty;

            // создаем библиотеку куда будем добавлять полученные рыбы
            var fishes = new Dictionary<long, Dictionary<string, long>>();
            for (var i = 0; i < amount; i++)
            {
                // получаем случайную рыбу
                var randomFish = await _mediator.Send(new GetRandomFishWithRarityQuery(fishRarity));
                // получаем случайное количество рыбы
                var randomAmount = (long) _random.Next(minAmount, maxAmount);

                // если в библитете есть такая рыба - нужно просто добавить количество
                if (fishes.ContainsKey(randomFish.Id)) fishes[randomFish.Id][randomFish.Name] += randomAmount;
                // если нет - добавляем в библиотеку
                else fishes.Add(randomFish.Id, new Dictionary<string, long> {{randomFish.Name, randomAmount}});
            }

            // теперь для каждой рыбы в библиотеке
            foreach (var (fishId, fish) in fishes)
            {
                // можно не опасаясь использовать Keys.First() и Values.First() по отношению к библиотеке fish
                // потому что мы используем ее просто как удобную версию KeyValuePair и там всего одно значение где
                // Key - это название рыбы
                // Value - это количество рыбы

                // добавляем рыбу пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Fish, fishId, fish.Values.First()));

                // добавляем локализированную строку
                finalString +=
                    $"{_emotes.GetEmoteOrBlank(fish.Keys.First())} {fish.Values.First()} {_local.Localize(fish.Keys.First(), fish.Values.First())}, ";
            }

            // возвращаем локализированную строку награды
            return finalString.Remove(finalString.Length - 2);
        }

        private async Task<string> OpenCastleBox(long amount)
        {
            // получаем собирательские ресурсы
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(Location.Castle));

            // заполняем строку полученных ресурсов
            var finalString = string.Empty;

            foreach (var gathering in gatherings)
            {
                // считаем финальное количество полученного ресурса
                long finalAmount = 0;
                for (var i = 0; i < amount; i++)
                {
                    // получаем шанс сбора этого ресурса
                    var gatheringChance = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringChance)))
                        .MasteryMaxValue(0);
                    // получаем шанс удвоенного сбора ресурса
                    var gatheringDoubleChance = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringDoubleChance)))
                        .MasteryMaxValue(0);
                    // получаем количество собираемого ресурса
                    var gatheringAmount = (await _mediator.Send(
                            new GetGatheringPropertiesQuery(gathering.Id, GatheringProperty.GatheringAmount)))
                        .MasteryMaxValue(0);
                    // считаем финальное количество ресурсов
                    var successAmount = await _mediator.Send(
                        new GetSuccessAmountQuery(gatheringChance, gatheringDoubleChance, gatheringAmount));

                    // если пользователь не получил ресурс - пропускаем
                    if (successAmount <= 0) continue;

                    // добавляем в общее количество
                    finalAmount += successAmount;
                }

                // если пользователь не получил ресурс - пропускаем
                if (finalAmount < 1) continue;

                // добавляем ресурс пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Gathering, gathering.Id, finalAmount));

                // добавляем локализированную строку
                finalString +=
                    $"{_emotes.GetEmoteOrBlank(gathering.Name)} {finalAmount} {_local.Localize(gathering.Name, finalAmount)}, ";
            }

            // возвращаем локализированную строку награды
            return finalString.Remove(finalString.Length - 2);
        }

        private async Task<string> OpenVillageBox(long amount)
        {
            // получаем минимальное количество продукта получаемое из коробки
            var productMinAmount =
                (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxVillageProductMinAmount));
            // получаем максимальное количество продукта получаемое из коробки
            var productMaxAmount =
                (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxVillageProductMaxAmount));
            // получаем минимальное количество урожая получаемое из коробки
            var cropMinAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxVillageCropMinAmount));
            // получаем максимальное количество урожай получаемое из коробки
            var cropMaxAmount = (int) await _mediator.Send(new GetPropertyValueQuery(Property.BoxVillageCropMaxAmount));
            // получаем продукты
            var products = await _mediator.Send(new GetAllProductsQuery());

            // заполняем строку полученного урожая и продуктов
            var finalString = string.Empty;

            // для каждого продукта
            foreach (var product in products)
            {
                // считаем финальное количество полученного продукта
                long finalAmount = 0;
                for (var i = 0; i < amount; i++)
                {
                    // получаем случайное количество продукта
                    finalAmount += _random.Next(productMinAmount, productMaxAmount);
                }

                // если пользовать не получил продукт - пропускаем
                if (finalAmount < 1) continue;

                // добавляем продукт пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Product, product.Id, finalAmount));

                // добавляем локализированную строку
                finalString +=
                    $"{_emotes.GetEmoteOrBlank(product.Name)} {finalAmount} {_local.Localize(product.Name, finalAmount)}, ";
            }

            // создаем библиотеку куда будем добавлять полученный урожай
            var crops = new Dictionary<long, Dictionary<string, long>>();
            for (var i = 0; i < amount; i++)
            {
                // получаем случайный урожай
                var randomCrop = await _mediator.Send(new GetRandomCropQuery());
                // получаем случайное количество урожая
                var randomCropAmount = (long) _random.Next(cropMinAmount, cropMaxAmount);

                // если в библиотеке есть такой урожай - нужно просто добавить количество
                if (crops.ContainsKey(randomCrop.Id)) crops[randomCrop.Id][randomCrop.Name] += randomCropAmount;
                // если нет - добавляем в библиотеку
                else crops.Add(randomCrop.Id, new Dictionary<string, long> {{randomCrop.Name, randomCropAmount}});
            }

            // теперь для каждого урожая в библиотеке
            foreach (var (cropId, crop) in crops)
            {
                // можно не опасаясь использовать Keys.First() и Values.First() по отношению к библиотеке crop
                // потому что мы используем ее просто как удобную версию KeyValuePair и там всего одно значение где
                // Key - это название урожая
                // Value - это количество урожая

                // добавляем урожай пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Crop, cropId, crop.Values.First()));

                // добавляем локализированную строку
                finalString +=
                    $"{_emotes.GetEmoteOrBlank(crop.Keys.First())} {crop.Values.First()} {_local.Localize(crop.Keys.First(), crop.Values.First())}, ";
            }

            // возвращаем локализированную строку награды
            return finalString.Remove(finalString.Length - 2);
        }
    }
}
