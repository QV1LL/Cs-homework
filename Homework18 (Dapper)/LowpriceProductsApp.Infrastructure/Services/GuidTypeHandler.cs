﻿using Dapper;
using System;
using System.Data;

namespace LowpriceProductsApp.Infrastructure.Services;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
        parameter.DbType = DbType.String;
    }

    public override Guid Parse(object value)
    {
        return Guid.Parse(value.ToString());
    }
}
