using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Commands;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.BuildingJob
{
    [InjectableService]
    public class BuildingJob : IBuildingJob
    {
        private readonly IMediator _mediator;

        public BuildingJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CompleteBuilding(long userId, long projectId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем постройку
            var building = await _mediator.Send(new GetBuildingByProjectIdQuery(projectId));

            // действие зависит от категории постройки
            switch (building.Category)
            {
                case BuildingCategory.Personal:

                    // добавляем постройку пользователю
                    await _mediator.Send(new AddBuildingToUserCommand(userId, building.Type));
                    // если постройка это расшение участка, то нужно добавить пользователю новые клетки участка
                    if (building.Type == Building.HarvestFieldExpansionL1)
                        await _mediator.Send(new CreateUserFieldsCommand(userId, new long[] {6, 7}));
                    if (building.Type == Building.HarvestFieldExpansionL2)
                        await _mediator.Send(new CreateUserFieldsCommand(userId, new long[] {8, 9, 10}));

                    break;
                case BuildingCategory.Family:

                    // получаем пользователя в семье
                    var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
                    // добавляем постройку семье
                    await _mediator.Send(new AddBuildingToFamilyCommand(userFamily.FamilyId, building.Type));

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

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Building));
            await Task.CompletedTask;
        }
    }
}
