using System.Data;
using System.Globalization;

using Dapper;

namespace LibTrack.Data.Sql;

public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value.ToString("O", CultureInfo.InvariantCulture);
    }

    public override DateTime Parse(object value)
    {
        return DateTime.Parse((string)value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}