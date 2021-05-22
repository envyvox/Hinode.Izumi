using System.Collections.Generic;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordChannelsQuery : IRequest<Dictionary<DiscordChannel, DiscordChannelRecord>>;
}
