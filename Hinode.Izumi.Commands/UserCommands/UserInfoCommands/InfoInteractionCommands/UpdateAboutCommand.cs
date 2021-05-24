using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.CooldownService.Commands;
using Hinode.Izumi.Services.GameServices.CooldownService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireRegistry]
    public class UpdateAboutCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UpdateAboutCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("информация"), Alias("about")]
        [Summary("Обновить информацию в профиле")]
        [CommandUsage("!информация Люблю мороженое и приключения")]
        public async Task UpdateAboutTask(
            [Summary("Новая информация")] [Remainder]
            string about = null)
        {
            // получаем кулдаун на смену информации пользователя
            var cooldown = await _mediator.Send(new GetUserCooldownQuery((long) Context.User.Id, Cooldown.UpdateAbout));
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;

            // если смена информации еще на кулдауне - выводим ошибку и пишем сколько осталось до отката
            if (cooldown.Expiration > timeNow)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UpdateAboutCooldown.Parse(
                    (await _mediator.Send(new GetPropertyValueQuery(Property.CooldownUpdateAbout)))
                    .Days()
                    .Humanize(1, new CultureInfo("ru-RU")),
                    cooldown.Expiration.Subtract(timeNow).TotalMinutes
                        .Minutes()
                        .Humanize(2, new CultureInfo("ru-RU")))));
            }
            // новая информация не может быть пустой
            else if (about is null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UpdateAboutNull.Parse()));
            }
            // новая информация не может быть длинее чем 1024 символа
            else if (about.Length > 1024)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UpdateAboutMaxLimit.Parse()));
            }
            // новая информация не может быть короче чем 2 символа
            else if (string.Concat(about.Where(c => !char.IsWhiteSpace(c))).Length < 2)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UpdateAboutMinLimit.Parse()));
            }
            else
            {
                // обновляем информацию пользователя
                await _mediator.Send(new UpdateUserAboutCommand((long) Context.User.Id, about));
                // добавляем кулдаун на смену информации
                await _mediator.Send(new AddCooldownToUserCommand((long) Context.User.Id,
                    Cooldown.UpdateAbout, timeNow.AddDays(
                        await _mediator.Send(new GetPropertyValueQuery(Property.CooldownUpdateAbout)))));

                var embed = new EmbedBuilder()
                    // подверждаем что смена информации прошла успешно
                    .WithDescription(IzumiReplyMessage.UpdateAboutSuccess.Parse());

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
