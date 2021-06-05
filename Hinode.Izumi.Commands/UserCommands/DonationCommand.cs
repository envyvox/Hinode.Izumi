using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.WorldInfo)]
    public class DonationCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public DonationCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("пожертвования")]
        [Summary("Информация о возможностях поддержки проекта")]
        public async Task DonationCommandTask()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var embed = new EmbedBuilder()
                .WithDescription(
                    "Вы можете поддержать развитие нашего проекта и получить различные бонусы!" +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField("Серверные бонусы",
                    $"{emotes.GetEmoteOrBlank("List")} **Личная роль** на **30 дней** стоимостью {emotes.GetEmoteOrBlank("Ruble")} **150 рублей**.\n" +
                    "> *Роль создается с указанным вами названием и цветом.*\n" +
                    "> *Роль не выделяется отдельно в списке пользователей на сервере.*\n\n" +
                    $"{emotes.GetEmoteOrBlank("List")} **Личный голосовой** канал на **30 дней** стоимостью {emotes.GetEmoteOrBlank("Ruble")} **200 рублей**.\n" +
                    "> *Голосовой канал создается с лимитом пользователей 5. За каждого*\n" +
                    $"> *дополнительного пользователя необходимо доплатить {emotes.GetEmoteOrBlank("Ruble")} **50 рублей***." +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField("Игровые бонусы",
                    $"{emotes.GetEmoteOrBlank("List")} {emotes.GetEmoteOrBlank("Premium")} **Премиум-статус** на **30 дней** стоимостью {emotes.GetEmoteOrBlank("Ruble")} **350 рублей**.\n" +
                    $"> *Ознакомиться с преимуществами премиум-статуса можно [нажав сюда]({await _mediator.Send(new GetImageUrlQuery(Image.Premium))})*.\n\n" +
                    $"{emotes.GetEmoteOrBlank("List")} {emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} **Жемчуг** стоимостью {emotes.GetEmoteOrBlank("Ruble")} **1 рубль** за {emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} **1 жемчуг**.\n" +
                    "> *Жемгуг это особая игровая валюта, позволяющая приобретать уникальные товары.*" +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                .AddField("Реквизиты",
                    $"{emotes.GetEmoteOrBlank("Monobank")} `5375 4141 0460 6651` EUGENE GARBUZOV\n" +
                    $"{emotes.GetEmoteOrBlank("Sberbank")} `4276 6800 1181 7390` TATYANA CHERNYKH\n\n" +
                    "При оплате указывайте в комментарии к платежу ваш никнейм в дискорде.\n" +
                    "После оплаты напишите в личные сообщения <@550493599629049858> что вы хотели бы получить и прикрепите скриншот с оплатой.");

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
