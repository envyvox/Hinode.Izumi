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
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldPlantCommand
{
    [InjectableService]
    public class FieldPlantCommand : IFieldPlantCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldPlantCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long fieldId, string seedNamePattern)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем клетку участка пользователя
            var userField = await _mediator.Send(new GetUserFieldQuery((long) context.User.Id, fieldId));

            switch (userField.State)
            {
                case FieldState.Planted:
                case FieldState.Watered:

                    // если клетка не пустая - на нее нельзя ничего посадить
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldPlantAlready.Parse()));

                    break;
                case FieldState.Completed:

                    // если на клетке уже вырос урожай - его нужно сперва собрать
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldCompleted.Parse()));

                    break;
                case FieldState.Empty:

                    var localization = await _local.GetLocalizationByLocalizedWord(
                        LocalizationCategory.Seed, seedNamePattern);
                    // получаем семена по локализированному названию
                    var seed = await _mediator.Send(new GetSeedQuery(localization.ItemId));
                    // получаем эти семена у пользователя
                    var userSeed = await _mediator.Send(new GetUserSeedQuery((long) context.User.Id, seed.Id));
                    // получаем текущий сезон в мире
                    var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));

                    // проверяем есть ли у пользователя необходимые семена
                    if (userSeed.Amount < 1)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UserDontHaveSeed.Parse(
                            emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name),
                            Location.Capital.Localize(true))));
                    }
                    // проверяем подходит ли сезон семян для посадки
                    else if (seed.Season != season)
                    {
                        await Task.FromException(
                            new Exception(IzumiReplyMessage.UserFieldPlantOnlyCurrentSeason.Parse()));
                    }
                    else
                    {
                        // садим семена на эту клетку участка
                        await _mediator.Send(new PlantUserFieldCommand((long) context.User.Id, fieldId, seed.Id));

                        // забираем у пользователя посаженные семена
                        await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                            (long) context.User.Id, InventoryCategory.Seed, seed.Id));
                        // добавляем пользователю статистку посаженных семян
                        await _mediator.Send(new AddStatisticToUserCommand(
                            (long) context.User.Id, Statistic.SeedPlanted));
                        // проверяем не выполнил ли пользователь достижения
                        await _mediator.Send(new CheckAchievementsInUserCommand(
                            (long) context.User.Id, new[]
                            {
                                Achievement.FirstPlant,
                                Achievement.Plant25Seed,
                                Achievement.Plant150Seed
                            }));
                        // отнимаем энергию у пользователя
                        await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                            // получаем количество энергии
                            await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostFieldPlant))));

                        var embed = new EmbedBuilder()
                            // баннер участка
                            .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)))
                            // подверждаем успешную посадку семян
                            .WithDescription(IzumiReplyMessage.UserFieldPlantSuccess.Parse(
                                emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name), seed.Growth));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                        await Task.CompletedTask;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
