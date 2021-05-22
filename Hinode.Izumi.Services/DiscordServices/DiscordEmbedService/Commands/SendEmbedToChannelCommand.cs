using Discord;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record SendEmbedToChannelCommand(
            DiscordChannel Channel,
            EmbedBuilder EmbedBuilder,
            string Message = "")
        : IRequest<IUserMessage>;
}
