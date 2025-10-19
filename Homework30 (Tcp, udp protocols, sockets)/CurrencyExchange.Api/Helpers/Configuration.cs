using System.Text.Json;
using FluentResults;

namespace CurrencyExchange.Api.Helpers;

public class Configuration : IDisposable
{
    private readonly JsonDocument _document;
    private readonly bool _disposed = false;

    private Configuration(JsonDocument document)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
    }

    public static Result<Configuration> Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Result.Fail("File path cannot be null or empty.");
        }

        try
        {
            if (!File.Exists(filePath))
            {
                return Result.Fail($"Configuration file not found: {filePath}");
            }

            string jsonContent = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return Result.Fail("Configuration file is empty.");
            }

            JsonDocument document = JsonDocument.Parse(jsonContent);
            return Result.Ok(new Configuration(document));
        }
        catch (JsonException ex)
        {
            return Result.Fail($"Invalid JSON in configuration file: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result.Fail($"Access denied to configuration file: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Unexpected error loading configuration: {ex.Message}");
        }
    }

    public Result<T> Get<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Fail<T>("Key cannot be null or empty.");
        }

        if (_disposed)
        {
            return Result.Fail<T>("Configuration has been disposed.");
        }

        try
        {
            if (_document.RootElement.TryGetProperty(key, out JsonElement element))
            {
                if (element.ValueKind == JsonValueKind.Null)
                {
                    return Result.Ok<T>(default);
                }

                using JsonDocument subDoc = JsonDocument.Parse(element.GetRawText());
                T? value = JsonSerializer.Deserialize<T>(subDoc.RootElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });

                if (value is null && typeof(T) != typeof(object) && typeof(T) != typeof(Nullable))
                {
                    return Result.Fail<T>($"Failed to deserialize value for key '{key}' to type {typeof(T).Name}.");
                }

                return value;
            }
            else
            {
                return Result.Fail<T>($"Key '{key}' not found in configuration.");
            }
        }
        catch (JsonException ex)
        {
            return Result.Fail<T>($"JSON deserialization error for key '{key}' to type {typeof(T).Name}: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail<T>($"Error retrieving value for key '{key}': {ex.Message}");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _document.Dispose();
        }
    }

    ~Configuration()
    {
        Dispose(false);
    }
}