using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.RpgServices.BannerService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Registration)]
    [IzumiRequireContext(DiscordContext.DirectMessage)]
    public class RegistrationCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;
        private readonly IBannerService _bannerService;
        private readonly IInventoryService _inventoryService;
        private readonly IPropertyService _propertyService;

        public RegistrationCommand(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IUserService userService, IEmoteService emoteService, IBannerService bannerService,
            IInventoryService inventoryService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _userService = userService;
            _emoteService = emoteService;
            _bannerService = bannerService;
            _inventoryService = inventoryService;
            _propertyService = propertyService;
        }

        [Command("регистрация"), Alias("registration")]
        [Summary("Зарегистрироваться в игровом мире")]
        [CommandUsage("!регистрация Вино из бананов")]
        public async Task RegistrationTask(
            [Summary("Игровое имя")] [Remainder] string name = null)
        {
            // игровое имя не должно быть пустым
            if (name == null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UsernameNull.Parse()));
            }
            else
            {
                // проверяем нет ли в базе пользователя с таким id
                var reg = await _userService.CheckUser((long) Context.User.Id);

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
                    var userWithName = await _userService.CheckUser(name);

                    if (userWithName)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UsernameTaken.Parse(name)));
                    }
                    else
                    {
                        // добавляем пользователя в базу
                        await _userService.AddUser((long) Context.User.Id, name);
                        // добавляем пользователю титул
                        await _userService.AddTitleToUser((long) Context.User.Id, Title.Newbie);
                        // выдаем пользователю роль столицы в дискорде
                        await _discordGuildService.ToggleRoleInUser(
                            (long) Context.User.Id, DiscordRole.LocationCapital, true);
                        // переименовываем пользователя в дискорде
                        await _discordGuildService.Rename((long) Context.User.Id, name);
                        // добавляем пользователю стандартный баннер
                        await _bannerService.AddBannerToUser((long) Context.User.Id, 1);
                        // и сразу же устанавливаем его как активный
                        await _bannerService.ToggleBannerInUser((long) Context.User.Id, 1, true);
                        // добавляем пользователю стартовую валюту
                        await _inventoryService.AddItemToUser(
                            (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            await _propertyService.GetPropertyValue(Property.EconomyStartupCapital));

                        // получаем все иконки из базы
                        var emotes = await _emoteService.GetEmotes();
                        var embed = new EmbedBuilder()
                            // подверждаем что регистрация пройдена успешно
                            .WithDescription(
                                IzumiReplyMessage.RegistrationSuccessDesc.Parse(
                                    name, emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                                    await _propertyService.GetPropertyValue(Property.EconomyStartupCapital)) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // предлагаем пройти обучение
                            .AddField(IzumiReplyMessage.RegistrationSuccessBeginTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessBeginDesc.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                                    await _propertyService.GetPropertyValue(Property.EconomyTrainingCost),
                                    await _propertyService.GetPropertyValue(Property.EconomyTrainingAward)) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // рассказываем о подтверждении гендера
                            .AddField(IzumiReplyMessage.RegistrationSuccessGenderTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessGenderDesc.Parse(
                                    emotes.GetEmoteOrBlank(Gender.None.Emote())) +
                                $"\n{emotes.GetEmoteOrBlank("Blank")}")
                            // рассказываем о реферальной системе
                            .AddField(IzumiReplyMessage.RegistrationSuccessReferralTitle.Parse(),
                                IzumiReplyMessage.RegistrationSuccessReferralDesc.Parse(
                                    emotes.GetEmoteOrBlank(Box.Capital.Emote())));

                        await _discordEmbedService.SendEmbed(Context.User, embed);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
