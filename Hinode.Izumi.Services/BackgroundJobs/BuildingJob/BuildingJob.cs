using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.FieldService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.BackgroundJobs.BuildingJob
{
    [InjectableService]
    public class BuildingJob : IBuildingJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IBuildingService _buildingService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IFieldService _fieldService;
        private readonly IPropertyService _propertyService;
        private readonly IFamilyService _familyService;

        public BuildingJob(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IBuildingService buildingService, IDiscordGuildService discordGuildService, IFieldService fieldService,
            IPropertyService propertyService, IFamilyService familyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _buildingService = buildingService;
            _discordGuildService = discordGuildService;
            _fieldService = fieldService;
            _propertyService = propertyService;
            _familyService = familyService;
        }

        public async Task CompleteBuilding(long userId, long projectId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем постройку
            var building = await _buildingService.GetBuildingByProjectId(projectId);

            // действие зависит от категории постройки
            switch (building.Category)
            {
                case BuildingCategory.Personal:

                    // добавляем постройку пользователю
                    await _buildingService.AddBuildingToUser(userId, building.Type);
                    // если постройка это расшение участка, то нужно добавить пользователю новые клетки участка
                    if (building.Type == Building.HarvestFieldExpansionL1)
                        await _fieldService.AddFieldToUser(userId, new long[] {6, 7});
                    if (building.Type == Building.HarvestFieldExpansionL2)
                        await _fieldService.AddFieldToUser(userId, new long[] {8, 9, 10});

                    break;
                case BuildingCategory.Family:

                    // получаем пользователя в семье
                    var userFamily = await _familyService.GetUserFamily(userId);
                    // добавляем постройку семье
                    await _buildingService.AddBuildingToFamily(userFamily.FamilyId, building.Type);

                    break;
                case BuildingCategory.Clan:

                    // TODO добавление постройки когда появятся кланы

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var embed = new EmbedBuilder()
                // Оповещаем о том, что постройка успешно завершена
                .WithDescription(IzumiReplyMessage.BuildCompleted.Parse(
                    emotes.GetEmoteOrBlank(building.Type.ToString()), building.Name));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
            await Task.CompletedTask;
        }
    }
}
