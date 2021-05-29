using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCommand
{
    [InjectableService]
    public class UserCollectionCommand : IUserCollectionCommand
    {
        private readonly IMediator _mediator;

        public UserCollectionCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var embed = new EmbedBuilder()
                // баннер коллекции
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Collection)))
                // рассказываем как просматривать коллекцию по категориям
                .WithDescription(IzumiReplyMessage.UserCollectionDesc.Parse())
                // выводим список категорий коллекции с номером и названием
                .AddField(IzumiReplyMessage.UserCollectionFieldName.Parse(),
                    Enum.GetValues(typeof(CollectionCategory))
                        .Cast<CollectionCategory>()
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
