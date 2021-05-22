using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.MasteryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Queries
{
    public record GetUserMasteryQuery(long UserId, Mastery Mastery) : IRequest<UserMasteryRecord>;
}
