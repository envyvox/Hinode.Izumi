using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.MasteryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Queries
{
    public record GetUserMasteriesQuery(long UserId) : IRequest<Dictionary<Mastery, UserMasteryRecord>>;
}
