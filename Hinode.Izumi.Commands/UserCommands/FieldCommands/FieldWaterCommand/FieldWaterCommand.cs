using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.FieldJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.FieldService.Records;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldWaterCommand
{
    [InjectableService]
    public class FieldWaterCommand : IFieldWaterCommand
    {
        private readonly IMediator _mediator;

        public FieldWaterCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string namePattern = null)
        {
            // если пользователь не ввел игровое имя в команде, значит он хочет полить свой участок
            if (namePattern is null)
            {
                // получаем клетки земли участка пользователя
                var userFields = await _mediator.Send(new GetUserFieldsQuery((long) context.User.Id));
                // поливаем участок пользователя
                await WaterFieldTask(context, userFields);
            }
            else
            {
                // получаем цель пользователя
                var tUser = await _mediator.Send(new GetUserByNamePatternQuery(namePattern));
                // получаем семью пользователя
                var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));
                // получаем семью цели пользователя
                var tUserFamily = await _mediator.Send(new GetUserFamilyQuery(tUser.Id));

                // проверяем в одной ли они семье
                if (userFamily is null ||
                    tUserFamily is null ||
                    userFamily.FamilyId != tUserFamily.FamilyId)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldWaterFamilyMemberOnly.Parse()));
                }
                else
                {
                    // получаем клетки земли участка пользователя
                    var userFields = await _mediator.Send(new GetUserFieldsQuery(tUser.Id));
                    // поливаем участок цели пользователя
                    await WaterFieldTask(context, userFields);
                }
            }
        }

        private async Task WaterFieldTask(SocketCommandContext context, IReadOnlyCollection<UserFieldRecord> userFields)
        {
            // проверяем есть ли у пользователя участок
            if (userFields.Count < 1)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFieldNull.Parse()));
            }
            else
            {
                // считаем сколько клеток нуждаются в поливке
                var fieldToWater = userFields.Count(field => field.State == FieldState.Planted);

                // проверяем нужно ли вообще поливать участок
                if (fieldToWater < 1)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFieldWaterNull.Parse()));
                }
                else
                {
                    // получаем пользователя который поливает участок
                    var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                    // получаем текущее время
                    var timeNow = DateTimeOffset.Now;
                    // определяем длительность поливки участка
                    var wateringTime =
                        await _mediator.Send(new GetActionTimeQuery(
                            await _mediator.Send(new GetPropertyValueQuery(Property.ActionTimeFieldWater)),
                            user.Energy)) * fieldToWater;

                    // обновляем текущую локацию пользователя
                    await _mediator.Send(new UpdateUserLocationCommand((long) context.User.Id, Location.FieldWatering));
                    // добавляем информацию о перемещении
                    await _mediator.Send(new CreateUserMovementCommand(
                        (long) context.User.Id, Location.FieldWatering, Location.Village,
                        timeNow.AddMinutes(wateringTime)));
                    // отнимаем энергию у пользователя
                    await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                        // получаем количество энергии
                        await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostFieldWater))));

                    // запускаем джобу для окончания поливки участка
                    BackgroundJob.Schedule<IFieldJob>(x =>
                            x.CompleteWatering((long) context.User.Id, userFields.First().UserId),
                        TimeSpan.FromMinutes(wateringTime));

                    var embed = new EmbedBuilder()
                        // баннер участка
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)))
                        // подверждаем успешное начало поливки участка
                        .WithDescription(IzumiReplyMessage.UserFieldWaterStart.Parse(wateringTime));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
