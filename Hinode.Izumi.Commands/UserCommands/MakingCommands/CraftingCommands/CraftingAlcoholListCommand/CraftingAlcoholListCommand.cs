using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholListCommand
{
    [InjectableService]
    public class CraftingAlcoholListCommand : ICraftingAlcoholListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingAlcoholListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем алкоголь
            var alcohols = await _mediator.Send(new GetAllAlcoholQuery());

            var embed = new EmbedBuilder()
                // изображение изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // рассказываем как изготовить алкоголь
                .WithDescription(
                    IzumiReplyMessage.CraftingAlcoholListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого алкоголя создаем embed field
            foreach (var alcohol in alcohols)
            {
                embed.AddField(
                    // выводим название изготавливаемого алкоголя
                    IzumiReplyMessage.CraftingListFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), alcohol.Id, emotes.GetEmoteOrBlank(alcohol.Name),
                        _local.Localize(alcohol.Name, 5), Location.Village.Localize(true)),
                    $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
