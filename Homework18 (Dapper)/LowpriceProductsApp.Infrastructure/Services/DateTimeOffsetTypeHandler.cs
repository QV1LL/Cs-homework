using Dapper;
using System;
using System.Data;

namespace LowpriceProductsApp.Infrastructure.Services;

public class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
        parameter.Value = value.ToString("O");
        parameter.DbType = DbType.String;
    }

    public override DateTimeOffset Parse(object value)
    {
        return DateTimeOffset.Parse(value.ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}
