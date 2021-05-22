using Discord;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyEmbedViewQuery(EmbedBuilder EmbedBuilder, FamilyRecord Family) : IRequest<EmbedBuilder>;
}
