using Dapper;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace LowpriceProductsApp.Infrastructure.Persistence.Repositories.Generic;

public abstract class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    protected ConnectionManager ConnectionManager { get; init; }
    protected string TableName { get; init; }
    protected List<string> Attributes { get; init; }

    protected GenericRepository(ConnectionManager connectionManager, string tableName, List<string> attributes)
    {
        ConnectionManager = connectionManager;
        TableName = tableName;
        Attributes = attributes;
    }

    public virtual T? Get(Guid id)
    {
        using var connection = ConnectionManager.GetConnection();
        string query = $"SELECT * FROM {TableName} WHERE Id = @Id;";
        return connection.QuerySingleOrDefault<T>(query, new { Id = id });
    }

    public virtual IEnumerable<T> GetAll()
    {
        using var connection = ConnectionManager.GetConnection();
        string query = $"SELECT * FROM {TableName};";
        return connection.Query<T>(query);
    }

    public virtual T? Find(Predicate<T> predicate)
    {
        return GetAll().FirstOrDefault(entity => predicate(entity));
    }

    public virtual void Remove(Guid id)
    {
        using var connection = ConnectionManager.GetConnection();
        string query = $"DELETE FROM {TableName} WHERE Id = @Id;";
        connection.Execute(query, new { Id = id });
    }

    public void Remove(T entity) => Remove(entity.Id ?? Guid.Empty);

    public virtual void Add(T entity)
    {
        using var connection = ConnectionManager.GetConnection();
        if (entity.Id == null)
        {
            entity.Id = Guid.NewGuid();
            Insert(entity, connection);
        }
        else
        {
            Update(entity, connection);
        }
    }

    protected virtual void Insert(T entity, DbConnection connection)
    {
        string columns = string.Join(", ", Attributes);
        string parameters = string.Join(", ", Attributes.Select(a => "@" + a));

        string query = $"INSERT INTO {TableName} (Id, {columns}) VALUES (@Id, {parameters});";
        connection.Execute(query, entity);
    }

    protected virtual void Update(T entity, DbConnection connection)
    {
        string setClause = string.Join(", ", Attributes.Select(a => $"{a} = @{a}"));
        string query = $"UPDATE {TableName} SET {setClause} WHERE Id = @Id;";
        connection.Execute(query, entity);
    }
}
