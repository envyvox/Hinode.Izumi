using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.CollectionService.Commands;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldCollectCommand
{
    [InjectableService]
    public class FieldCollectCommand : IFieldCollectCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldCollectCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long fieldId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем клетку земли участка пользователя
            var userField = await _mediator.Send(new GetUserFieldQuery((long) context.User.Id, fieldId));

            switch (userField.State)
            {
                case FieldState.Empty:

                    // нельзя собрать с пустой клетки участка
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldEmpty.Parse()));

                    break;
                case FieldState.Planted:
                case FieldState.Watered:

                    // нельзя собрать с еще не готовой к сбору клетки участка
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldCollectNotReady.Parse()));

                    break;
                case FieldState.Completed:

                    // получаем посаженные семена
                    var seed = await _mediator.Send(new GetSeedQuery(userField.SeedId));
                    // получаем урожай из этих семян
                    var crop = await _mediator.Send(new GetCropBySeedIdQuery(seed.Id));

                    var embed = new EmbedBuilder()
                        // баннер участка
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)));

                    // определяем количество собранного урожая
                    var amount = seed.Multiply ? new Random().Next(2, 4) : 1;

                    // если семяна имеют повторный рост, нужно запустить рост повторно
                    if (seed.ReGrowth > 0)
                    {
                        // запускаем повторный рост семян
                        await _mediator.Send(new StartReGrowthOnUserFieldCommand(
                            (long) context.User.Id, userField.FieldId));

                        embed.WithDescription(IzumiReplyMessage.UserFieldCollectSuccessReGrowth.Parse(
                            emotes.GetEmoteOrBlank(crop.Name), amount, _local.Localize(crop.Name, amount),
                            seed.ReGrowth));
                    }
                    else
                    {
                        // сбрасываем состоение клетки участка на пустое
                        await _mediator.Send(new ResetUserFieldCommand((long) context.User.Id, userField.FieldId));

                        embed.WithDescription(IzumiReplyMessage.UserFieldCollectSuccess.Parse(
                            emotes.GetEmoteOrBlank(crop.Name), amount, _local.Localize(crop.Name, amount)));
                    }

                    // добавляем пользователю собранный урожай
                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Crop, crop.Id, amount));
                    // добавляем в коллекцию пользователя урожай
                    await _mediator.Send(new AddCollectionToUserCommand(
                        (long) context.User.Id, CollectionCategory.Crop, crop.Id));
                    // добавляем пользователю статистику собранного урожая
                    await _mediator.Send(new AddStatisticToUserCommand(
                        (long) context.User.Id, Statistic.CropHarvested));
                    // проверяем не выполнил ли пользователь достижения
                    await _mediator.Send(new CheckAchievementsInUserCommand(
                        (long) context.User.Id, new[]
                        {
                            Achievement.Collect50Crop,
                            Achievement.Collect300Crop,
                            Achievement.CompleteCollectionCrop
                        }));
                    // отнимаем энергию у пользователя
                    await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                        // получаем количество энергии
                        await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostFieldCollect))));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
