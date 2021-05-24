using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.AchievementService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Queries
{
    public record GetUserAchievementsByCategoryQuery(
            long UserId,
            AchievementCategory Category)
        : IRequest<UserAchievementRecord[]>;

    public class GetUserAchievementsByCategoryHandler
        : IRequestHandler<GetUserAchievementsByCategoryQuery, UserAchievementRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserAchievementsByCategoryHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserAchievementRecord[]> Handle(GetUserAchievementsByCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, category) = request;
            return (await _con.GetConnection()
                    .QueryAsync<UserAchievementRecord>(@"
                        select ua.* from user_achievements as ua
                            inner join achievements a
                                on a.id = ua.achievement_id
                        where ua.user_id = @userId
                          and a.category = @category",
                        new {userId, category}))
                .ToArray();
        }
    }
}
