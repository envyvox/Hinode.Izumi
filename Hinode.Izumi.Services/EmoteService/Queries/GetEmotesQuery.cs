using System.Collections.Generic;
using Hinode.Izumi.Services.EmoteService.Records;
using MediatR;

namespace Hinode.Izumi.Services.EmoteService.Queries
{
    public record GetEmotesQuery : IRequest<Dictionary<string, EmoteRecord>>;
}
