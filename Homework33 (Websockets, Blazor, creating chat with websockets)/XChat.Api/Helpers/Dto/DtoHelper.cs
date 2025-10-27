using FluentResults;
using System.Text.Json;

namespace XChat.Api.Helpers.Dto;

internal static class DtoHelper
{
    public static Result<T> DeserializeRequest<T>(string? body) where T : class
    {
        if (string.IsNullOrWhiteSpace(body))
            return Result.Fail<T>("Request body is empty");

        try
        {
            var dto = JsonSerializer.Deserialize<T>(body);
            if (dto is null)
                return Result.Fail<T>("Failed to deserialize request");

            return dto;
        }
        catch (JsonException)
        {
            return Result.Fail<T>("Invalid JSON format");
        }
    }
}
