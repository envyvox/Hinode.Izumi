using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.BackgroundJobs.BuildingJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProjectService.Commands;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.GameServices.ProjectService.Records;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.BuildingCommands
{
    [CommandCategory(CommandCategory.Building)]
    [IzumiRequireRegistry]
    public class BuildingStartCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public BuildingStartCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("построить"), Alias("build")]
        [Summary("Начать строительство по указанному чертежу")]
        [CommandUsage("!построить 1", "!построить 3")]
        public async Task BuildingStartTask(
            [Summary("Номер чертежа")] long projectId)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем чертеж
            var project = await _mediator.Send(new GetProjectQuery(projectId));
            // проверяем есть ли упользователя чертеж
            var hasProject = await _mediator.Send(new CheckUserHasProjectQuery((long) Context.User.Id, project.Id));

            if (!hasProject)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserProjectNull.Parse(
                    _emotes.GetEmoteOrBlank("Project"))));
            }
            else
            {
                // проверяем проходит ли пользователь все требования к постройке
                await CheckBuildingRequirements(project);
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) Context.User.Id, Currency.Ien));
                // получаем стоимость одного часа строительства
                var buildingHourPrice = await _mediator.Send(new GetPropertyValueQuery(Property.BuildingHourPrice));
                // считаем стоимость строительства
                var buildingPrice = project.Time * buildingHourPrice;

                // проверяем хватает ли пользователю на оплату строительства
                if (userCurrency.Amount < buildingPrice)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.BuildNoCurrency.Parse(
                        _emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // проверяем есть ли у пользователя все необходимые ресурсы для строительства
                    await _mediator.Send(new CheckProjectIngredientsCommand((long) Context.User.Id, project.Id));
                    // забираем у пользователя все ресурсы для строительства
                    await _mediator.Send(new RemoveProjectIngredientsCommand((long) Context.User.Id, project.Id));
                    // забираем у пользователя валюту для оплаты строительства
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), buildingPrice));
                    // забираем у пользователя чертеж
                    await _mediator.Send(new RemoveProjectFromUserCommand((long) Context.User.Id, project.Id));

                    // запускаем джобу с окончанием строительства
                    BackgroundJob.Schedule<IBuildingJob>(
                        x => x.CompleteBuilding((long) Context.User.Id, project.Id),
                        TimeSpan.FromHours(project.Time));

                    var embed = new EmbedBuilder()
                        // подтверждаем что строительство успешно начато
                        .WithDescription(IzumiReplyMessage.BuildStartedSuccess.Parse(
                            _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), buildingPrice,
                            _local.Localize(Currency.Ien.ToString(), buildingPrice),
                            project.Time.Hours().Humanize(1, new CultureInfo("ru-RU"))));

                    await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }

        private async Task CheckBuildingRequirements(ProjectRecord project)
        {
            // получаем постройку из этого чертежа
            var building = await _mediator.Send(new GetBuildingByProjectIdQuery(project.Id));

            // проверка зависит от категории постройки
            switch (building.Category)
            {
                case BuildingCategory.Personal:

                    // проверяем есть ли у пользователя уже эта постройка
                    var userHasBuilding = await _mediator.Send(new CheckBuildingInUserQuery(
                        (long) Context.User.Id, building.Type));

                    // если у пользователя уже есть эта постройка - выводим ошибку
                    if (userHasBuilding)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.BuildPersonalBuildingAlready.Parse(
                            _emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name,
                            _emotes.GetEmoteOrBlank("Project"))));
                    }
                    else
                    {
                        // если постройка не требует наличия другой постройки - пропускаем
                        if (project.ReqBuildingId is null) return;

                        // получаем необходимую постройку
                        var reqBuilding = await _mediator.Send(new GetBuildingByIdQuery((long) project.ReqBuildingId));
                        // проверяем есть ли у пользователя необходимая постройка
                        var userHasReqBuilding = await _mediator.Send(new CheckBuildingInUserQuery(
                            (long) Context.User.Id, reqBuilding.Type));

                        // если нет - выводим ошибку
                        if (!userHasReqBuilding)
                        {
                            await Task.FromException(new Exception(
                                IzumiReplyMessage.BuildRequirePersonalBuildingButNull.Parse(
                                    _emotes.GetEmoteOrBlank("Project"),
                                    _emotes.GetEmoteOrBlank(reqBuilding.Type.ToString()), reqBuilding.Name)));
                        }
                    }

                    break;
                case BuildingCategory.Family:

                    // проверяем состоит ли пользователь в семье
                    var userHasFamily = await _mediator.Send(new CheckUserHasFamilyQuery((long) Context.User.Id));
                    // если нет - то он не может строить семейные постройки
                    if (!userHasFamily)
                    {
                        await Task.FromException(new Exception(
                            IzumiReplyMessage.BuildRequireUserFamilyButNull.Parse(
                                _emotes.GetEmoteOrBlank("Project"))));
                    }
                    else
                    {
                        // получаем пользователя в семье
                        var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) Context.User.Id));

                        // если пользователь не глава семьи - то он не может строить семейные постройки
                        if (userFamily.Status != UserInFamilyStatus.Head)
                        {
                            await Task.FromException(new Exception(
                                IzumiReplyMessage.BuildRequireUserFamilyStatusHeadButLower.Parse(
                                    _emotes.GetEmoteOrBlank("Project"))));
                        }
                        else
                        {
                            // проверяем есть ли у семьи уже эта постройка
                            var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                                userFamily.FamilyId, building.Type));

                            // если есть - выводим ошибку
                            if (familyHasBuilding)
                            {
                                await Task.FromException(new Exception(
                                    IzumiReplyMessage.BuildFamilyBuildingAlready.Parse(
                                        _emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name,
                                        _emotes.GetEmoteOrBlank("Project"))));
                            }
                            else
                            {
                                // если постройка не требует наличия другой постройки - пропускаем
                                if (project.ReqBuildingId is null) return;

                                // получаем требуемую постройку
                                var reqBuilding = await _mediator.Send(
                                    new GetBuildingByIdQuery((long) project.ReqBuildingId));
                                // проверяем есть ли у семьи требуемая постройка
                                var familyHasReqBuilding = await _mediator.Send(
                                    new CheckBuildingInFamilyQuery(userFamily.FamilyId, reqBuilding.Type));

                                // если нет - выводим ошибку
                                if (!familyHasReqBuilding)
                                {
                                    await Task.FromException(new Exception(
                                        IzumiReplyMessage.BuildRequireFamilyBuildingButNull.Parse(
                                            _emotes.GetEmoteOrBlank("Project"),
                                            _emotes.GetEmoteOrBlank(reqBuilding.Type.ToString()), reqBuilding.Name)));
                                }
                            }
                        }
                    }

                    break;
                case BuildingCategory.Clan:
                    // TODO добавить проверку когда появятся кланы
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
