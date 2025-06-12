using System.Data.SQLite;

namespace Warehouse.Entities;

internal class Provider
{
    internal Guid Id { get; set; }

    internal string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }

    internal static string ConnectionString = null!;
    internal static string TableName = "Providers";

    internal Provider() { }

    internal Provider(Guid id)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = $"SELECT * FROM {TableName} WHERE Id = @Id";
        selectCommand.Parameters.AddWithValue("@Id", id.ToString());

        using var reader = selectCommand.ExecuteReader();
        if (!reader.Read())
            throw new KeyNotFoundException("Provider not found.");

        Id = id;
        Title = reader.GetString(reader.GetOrdinal(nameof(Title)));
    }

    internal void Save()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
            command.CommandText = $"INSERT INTO {TableName} (Id, Title) VALUES (@Id, @Title)";
        }
        else
        {
            command.CommandText = $"UPDATE {TableName} SET Title = @Title WHERE Id = @Id";
        }

        command.Parameters.AddWithValue("@Id", Id.ToString());
        command.Parameters.AddWithValue("@Title", Title);
        command.ExecuteNonQuery();
    }

    internal void Delete()
    {
        if (Id == Guid.Empty) return;

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = $"DELETE FROM {TableName} WHERE Id = @Id";
        deleteCommand.Parameters.AddWithValue("@Id", Id.ToString());
        deleteCommand.ExecuteNonQuery();
    }

    internal static IEnumerable<Provider> GetAll()
    {
        var providers = new List<Provider>();
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = $"SELECT * FROM {TableName}";

        using var reader = selectCommand.ExecuteReader();
        while (reader.Read())
        {
            var provider = new Provider
            {
                Id = reader.GetGuid(reader.GetOrdinal(nameof(Id))),
                Title = reader.GetString(reader.GetOrdinal(nameof(Title)))
            };
            providers.Add(provider);
        }

        return providers;
    }
}
