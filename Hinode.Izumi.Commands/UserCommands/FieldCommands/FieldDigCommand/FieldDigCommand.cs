using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FieldService.Commands;
using Hinode.Izumi.Services.GameServices.FieldService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldDigCommand
{
    [InjectableService]
    public class FieldDigCommand : IFieldDigCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FieldDigCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long fieldId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем клетку участка пользователя
            var userField = await _mediator.Send(new GetUserFieldQuery((long) context.User.Id, fieldId));

            // проверяем не пустая ли она
            if (userField.State == FieldState.Empty)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFieldDigEmpty.Parse()));
            }
            else
            {
                // сбрасываем состояние клетки участка на пустое
                await _mediator.Send(new ResetUserFieldCommand((long) context.User.Id, fieldId));
                // отнимаем энергию у пользователя
                await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                    // получаем количество энергии
                    await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostFieldDig))));

                // получаем посажанные в этой клетке участка семена
                var seed = await _mediator.Send(new GetSeedQuery(userField.SeedId));

                var embed = new EmbedBuilder()
                    // баннер участка
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Field)))
                    // подтверждаем что семена выкопаны с клетки участка
                    .WithDescription(IzumiReplyMessage.UserFieldDigSuccess.Parse(
                        emotes.GetEmoteOrBlank(seed.Name), _local.Localize(seed.Name)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
