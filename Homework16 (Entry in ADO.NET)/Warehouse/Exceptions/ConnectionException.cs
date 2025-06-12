using System.Data.Common;

namespace Warehouse.Exceptions;

internal class ConnectionException : DbException
{
    internal ConnectionException(string connectionString)
        : base($"Failed connection to: {connectionString}")
    {
    }
}
