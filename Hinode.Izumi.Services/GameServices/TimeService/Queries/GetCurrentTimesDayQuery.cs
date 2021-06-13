using System;
using System.Threading;
using System.Threading.Tasks;
using CoordinateSharp;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.TimeService.Queries
{
    public record GetCurrentTimesDayQuery : IRequest<TimesDay>;

    public class GetCurrentTimesDayHandler : IRequestHandler<GetCurrentTimesDayQuery, TimesDay>
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        public GetCurrentTimesDayHandler(TimeZoneInfo timeZoneInfo)
        {
            _timeZoneInfo = timeZoneInfo;
        }

        public async Task<TimesDay> Handle(GetCurrentTimesDayQuery request, CancellationToken cancellationToken)
        {
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            var coordinate = new Coordinate(55.915379, 37.824598, timeNow);

            return await Task.FromResult(
                timeNow > coordinate.CelestialInfo.SunRise &&
                timeNow < coordinate.CelestialInfo.SunSet
                    ? TimesDay.Day
                    : TimesDay.Night);
        }
    }
}
