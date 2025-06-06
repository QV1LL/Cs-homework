using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Warehouse.Exceptions;

namespace Warehouse.Entities;

internal class Product
{
    public Guid Id { get; set; }

    public string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }

    public string Type
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            field = value;
        }
    }

    private Guid ProviderId { get; set; }
    public Provider Provider
    {
        get => field ?? new Provider(ProviderId);
        set => field = value;
    }

    public int Quantity
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            field = value;
        }
    }

    public decimal Price
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            field = value;
        }
    }

    public DateTimeOffset SupplyDate
    {
        get => field;
        set
        {
            if (value > DateTimeOffset.Now)
                throw new ArgumentOutOfRangeException(nameof(value));

            field = value;
        }
    }

    public static string ConnectionString = null!;
    public static string TableName = "Products";

    public Product() { }

    public Product(Guid id)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {TableName} WHERE Id = @Id";
        cmd.Parameters.AddWithValue("@Id", id.ToString());

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new KeyNotFoundException("Product not found.");

        Id = id;
        Title = reader.GetString(reader.GetOrdinal(nameof(Title)));
        Type = reader.GetString(reader.GetOrdinal(nameof(Type)));
        ProviderId = reader.GetGuid(reader.GetOrdinal(nameof(ProviderId)));
        Quantity = reader.GetInt32(reader.GetOrdinal(nameof(Quantity)));
        Price = reader.GetDecimal(reader.GetOrdinal(nameof(Price)));
        SupplyDate = reader.GetDateTime(reader.GetOrdinal(nameof(SupplyDate)));
    }

    public void Save()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
            command.CommandText = $"INSERT INTO {TableName} (Id, Title, Type, ProviderId, Quantity, Price, SupplyDate) VALUES (@Id, @Title, @Type, @ProviderId, @Quantity, @Price, @SupplyDate)";
        }
        else
        {
            command.CommandText = $"UPDATE {TableName} SET Title = @Title, Type = @Type, ProviderId = @ProviderId, Quantity = @Quantity, Price = @Price, SupplyDate = @SupplyDate WHERE Id = @Id";
        }

        command.Parameters.AddWithValue("@Id", Id.ToString());
        command.Parameters.AddWithValue("@Title", Title);
        command.Parameters.AddWithValue("@Type", Type);
        command.Parameters.AddWithValue("@ProviderId", Provider.Id.ToString());
        command.Parameters.AddWithValue("@Quantity", Quantity);
        command.Parameters.AddWithValue("@Price", Price);
        command.Parameters.AddWithValue("@SupplyDate", SupplyDate.DateTime);
        command.ExecuteNonQuery();
    }

    public static IEnumerable<Product> GetAll()
    {
        var products = new List<Product>();
        using var conn = new SQLiteConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {TableName}";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var product = new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal(nameof(Id))),
                Title = reader.GetString(reader.GetOrdinal(nameof(Title))),
                Type = reader.GetString(reader.GetOrdinal(nameof(Type))),
                ProviderId = reader.GetGuid(reader.GetOrdinal(nameof(ProviderId))),
                Quantity = reader.GetInt32(reader.GetOrdinal(nameof(Quantity))),
                Price = reader.GetDecimal(reader.GetOrdinal(nameof(Price))),
                SupplyDate = reader.GetDateTime(reader.GetOrdinal(nameof(SupplyDate)))
            };
            products.Add(product);
        }

        return products;
    }

    public void Delete()
    {
        if (Id == Guid.Empty) return;

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var deleteCommand = connection.CreateCommand();
        deleteCommand.CommandText = $"DELETE FROM {TableName} WHERE Id = @Id";
        deleteCommand.Parameters.AddWithValue("@Id", Id.ToString());
        deleteCommand.ExecuteNonQuery();
    }
}
