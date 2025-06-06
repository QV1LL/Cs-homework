using System.Data.Common;

namespace Warehouse.Exceptions;

internal class ConnectionException : DbException
{
    public ConnectionException(string connectionString)
        : base($"Failed connection to: {connectionString}")
    {
    }
}
