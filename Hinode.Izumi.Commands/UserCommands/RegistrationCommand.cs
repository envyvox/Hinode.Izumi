using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Registration)]
    [IzumiRequireContext(DiscordContext.DirectMessage)]
    public class RegistrationCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public RegistrationCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("регистрация"), Alias("registration")]
        [Summary("Зарегистрироваться в игровом мире")]
        [CommandUsage("!регистрация Вино из бананов")]
        public async Task RegistrationTask(
            [Summary("Игровое имя")] [Remainder] string name = null)
        {
            // игровое имя не должно быть пустым
            if (name is null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UsernameNull.Parse()));
            }
            else
            {
                // проверяем нет ли в базе пользователя с таким id
                var reg = await _mediator.Send(new CheckUserByIdQuery((long) Context.User.Id));

                if (reg)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.RegistrationAlready.Parse()));
                }
                // проверяем введенное имя на валидность
                else if (!StringExtensions.CheckValid(name))
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UsernameNotValid.Parse(name)));
                }
                else
                {
                    // проверяем не занято ли желаемое игровое имя
                    var userWithName = await _mediator.Send(new CheckUserByNameQuery(name));

                    if (userWithName)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UsernameTaken.Parse(name)));
                    }
                    else
                    {
                        // получаем иконки из базы
                        var emotes = await _mediator.Send(new GetEmotesQuery());
                        // получаем количество начального капитала
                        var startupCapital = await _mediator.Send(
                            new GetPropertyValueQuery(Property.EconomyStartupCapital));
                        // получаем количество затрачиваемых иен на обучение
                        var trainingCost = await _mediator.Send(
                            new GetPropertyValueQuery(Property.EconomyTrainingCost));
                        // получаем количество получаемых иен за обучение
                        var trainingAward = await _mediator.Send(
                            new GetPropertyValueQuery(Property.EconomyTrainingAward));

                        // добавляем пользователя в базу
                        await _mediator.Send(new CreateUserCommand((long) Context.User.Id, name));
                        // добавляем пользователю титул
                        await _mediator.Send(new AddTitleToUserCommand((long) Context.User.Id, Title.Newbie));
                        // выдаем пользователю роль столицы в дискорде
                        await _mediator.Send(
                            new AddDiscordRoleToUserCommand((long) Context.User.Id, DiscordRole.LocationCapital));
                        // переименовываем пользователя в дискорде
                        await _mediator.Send(new RenameDiscordUserCommand((long) Context.User.Id, name));
                        // добавляем пользователю стандартный баннер
                        await _mediator.Send(new AddBannerToUserCommand((long) Context.User.Id, 1));
                        // и сразу же устанавливаем его как активный
                        await _mediator.Send(new ActivateBannerInUserCommand((long) Context.User.Id, 1));
                        // добавляем пользователю стартовую валюту
                        await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                            (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            startupCapital));

                        var embed = new EmbedBuilder()
                            // подверждаем что регистрация пройдена успешно
                            .WithDescription(
                                IzumiReplyMessage.RegistrationSuccessDesc.Parse(
                                    name, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), startupCapital) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // предлагаем пройти обучение
                            .AddField(IzumiReplyMessage.RegistrationSuccessBeginTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessBeginDesc.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), trainingCost, trainingAward) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // рассказываем о подтверждении пола
                            .AddField(IzumiReplyMessage.RegistrationSuccessGenderTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessGenderDesc.Parse(
                                    emotes.GetEmoteOrBlank(Gender.None.Emote())) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // рассказываем о реферальной системе
                            .AddField(IzumiReplyMessage.RegistrationSuccessReferralTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessReferralDesc.Parse(
                                    emotes.GetEmoteOrBlank(Box.Capital.Emote())));

                        await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
