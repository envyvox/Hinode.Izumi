using System;
using System.Data;
using Dapper;

namespace Hinode.Izumi.Framework.Database.DapperHandlers
{
    public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override void SetValue(IDbDataParameter parameter, TimeSpan value) => parameter.Value = value.Ticks;

        public override TimeSpan Parse(object value) => TimeSpan.FromTicks((long) value);
    }
}
