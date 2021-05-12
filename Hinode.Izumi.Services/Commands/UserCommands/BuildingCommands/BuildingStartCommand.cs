using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.BackgroundJobs.BuildingJob;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProjectService;
using Hinode.Izumi.Services.RpgServices.ProjectService.Models;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Humanizer;

namespace Hinode.Izumi.Services.Commands.UserCommands.BuildingCommands
{
    [CommandCategory(CommandCategory.Building)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class BuildingStartCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IProjectService _projectService;
        private readonly IIngredientService _ingredientService;
        private readonly IInventoryService _inventoryService;
        private readonly IFamilyService _familyService;
        private readonly IPropertyService _propertyService;
        private readonly ILocalizationService _local;
        private readonly IBuildingService _buildingService;

        private Dictionary<string, EmoteModel> _emotes;

        public BuildingStartCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IProjectService projectService, IIngredientService ingredientService, IInventoryService inventoryService,
            IFamilyService familyService, IPropertyService propertyService, ILocalizationService local,
            IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _projectService = projectService;
            _ingredientService = ingredientService;
            _inventoryService = inventoryService;
            _familyService = familyService;
            _propertyService = propertyService;
            _local = local;
            _buildingService = buildingService;
        }

        [Command("построить"), Alias("build")]
        [Summary("Начать строительство по указанному чертежу")]
        [CommandUsage("!построить 1", "!построить 3")]
        public async Task BuildingStartTask(
            [Summary("Номер чертежа")] long projectId)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем чертеж
            var project = await _projectService.GetProject(projectId);
            // проверяем есть ли упользователя чертеж
            var hasProject = await _projectService.CheckUserHasProject((long) Context.User.Id, project.Id);

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
                var userCurrency = await _inventoryService.GetUserCurrency((long) Context.User.Id, Currency.Ien);
                // получаем стоимость одного часа строительства
                var buildingHourPrice = await _propertyService.GetPropertyValue(Property.BuildingHourPrice);
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
                    await _ingredientService.CheckProjectIngredients((long) Context.User.Id, project.Id);
                    // забираем у пользователя все ресурсы для строительства
                    await _ingredientService.RemoveProjectIngredients((long) Context.User.Id, project.Id);
                    // забираем у пользователя валюту для оплаты строительства
                    await _inventoryService.RemoveItemFromUser(
                        (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), buildingPrice);
                    // забираем у пользователя чертеж
                    await _projectService.RemoveProjectFromUser((long) Context.User.Id, project.Id);

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

                    await _discordEmbedService.SendEmbed(Context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }

        private async Task CheckBuildingRequirements(ProjectModel project)
        {
            // получаем постройку из этого чертежа
            var building = await _buildingService.GetBuildingByProjectId(project.Id);

            // проверка зависит от категории постройки
            switch (building.Category)
            {
                case BuildingCategory.Personal:

                    // проверяем есть ли у пользователя уже эта постройка
                    var userHasBuilding = await _buildingService.CheckBuildingInUser(
                        (long) Context.User.Id, building.Type);

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
                        if (project.ReqBuildingId == null) return;

                        // получаем необходимую постройку
                        var reqBuilding = await _buildingService.GetBuilding((long) project.ReqBuildingId);
                        // проверяем есть ли у пользователя необходимая постройка
                        var userHasReqBuilding = await _buildingService.CheckBuildingInUser(
                            (long) Context.User.Id, reqBuilding.Type);

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
                    var userHasFamily = await _familyService.CheckUserHasFamily((long) Context.User.Id);
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
                        var userFamily = await _familyService.GetUserFamily((long) Context.User.Id);

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
                            var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                                userFamily.FamilyId, building.Type);

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
                                if (project.ReqBuildingId == null) return;

                                // получаем требуемую постройку
                                var reqBuilding = await _buildingService.GetBuilding((long) project.ReqBuildingId);
                                // проверяем есть ли у семьи требуемая постройка
                                var familyHasReqBuilding = await _buildingService.CheckBuildingInFamily(
                                    userFamily.FamilyId, reqBuilding.Type);

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
