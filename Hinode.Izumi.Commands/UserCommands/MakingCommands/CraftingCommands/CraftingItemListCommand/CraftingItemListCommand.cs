using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.
    CraftingItemListCommand
{
    [InjectableService]
    public class CraftingItemListCommand : ICraftingItemListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingItemListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем изготавливаемые предметы
            var craftings = await _mediator.Send(new GetAllCraftingsQuery());

            var embed = new EmbedBuilder()
                // изображение изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // рассказываем как изготовить алкоголь
                .WithDescription(
                    IzumiReplyMessage.CraftingItemListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого изготавливаемого предмета создаем embed field
            foreach (var crafting in craftings)
            {
                embed.AddField(
                    // выводим название изготавливаемого предмета
                    IzumiReplyMessage.CraftingListFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), crafting.Id, emotes.GetEmoteOrBlank(crafting.Name),
                        _local.Localize(crafting.Name, 5), crafting.Location.Localize(true)),
                    $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
