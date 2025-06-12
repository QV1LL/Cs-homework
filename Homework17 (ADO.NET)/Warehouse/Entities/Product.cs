using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using Warehouse.Enums;

namespace Warehouse.Entities;

internal class Product
{
    internal Guid Id { get; set; }

    internal string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Title), "Name cannot be null or empty.");
            field = value;
        }
    }

    internal Color Color { get; set; }

    internal ProductType Type { get; set; }

    internal int CalorieContent
    {
        get => field;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(CalorieContent), "Calorie content must be positive.");

            field = value;
        }
    }

    internal static string ConnectionString = null!;
    internal static string TableName = "Products";

    internal Product() { }

    internal Product(Guid id)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {TableName} WHERE id = @id";
        command.Parameters.AddWithValue("@id", id.ToString());

        using var reader = command.ExecuteReader();
        if (!reader.Read())
            throw new KeyNotFoundException("Product not found.");

        Id = id;
        Title = reader.GetString(reader.GetOrdinal("name"));
        Type = Enum.Parse<ProductType>(reader.GetString(reader.GetOrdinal("type")));
        CalorieContent = reader.GetInt32(reader.GetOrdinal("calorie_content"));
        var colorString = reader.GetString(reader.GetOrdinal("color"));
        Color = ColorTranslator.FromHtml(colorString);
    }

    internal void Save()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
            command.CommandText = $"INSERT INTO {TableName} (id, name, type, calorie_content, color) VALUES (@id, @name, @type, @calories, @color)";
        }
        else
        {
            command.CommandText = $"UPDATE {TableName} SET name = @name, type = @type, calorie_content = @calories, color = @color WHERE id = @id";
        }

        command.Parameters.AddWithValue("@id", Id.ToString());
        command.Parameters.AddWithValue("@name", Title);
        command.Parameters.AddWithValue("@type", Type.ToString());
        command.Parameters.AddWithValue("@calories", CalorieContent);
        command.Parameters.AddWithValue("@color", ColorTranslator.ToHtml(Color));

        command.ExecuteNonQuery();
    }

    internal void Delete()
    {
        if (Id == Guid.Empty) return;

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM {TableName} WHERE id = @id";
        command.Parameters.AddWithValue("@id", Id.ToString());
        command.ExecuteNonQuery();
    }

    internal static IEnumerable<Product> GetAll()
    {
        var products = new List<Product>();

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {TableName}";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var colorString = reader.GetString(reader.GetOrdinal("color"));
            products.Add(new Product
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("name")),
                Type = Enum.Parse<ProductType>(reader.GetString(reader.GetOrdinal("type"))),
                CalorieContent = reader.GetInt32(reader.GetOrdinal("calorie_content")),
                Color = ColorTranslator.FromHtml(colorString)
            });
        }

        return products;
    }
}