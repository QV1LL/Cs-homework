using CurrencyExchange.Api.Models;
using FluentResults;
using System.Net.Sockets;

namespace CurrencyExchange.Api.Services;

internal static class UserService
{
    public static async Task<Result<User>> LoginAsync(Socket client)
    {
        var remote = client.RemoteEndPoint?.ToString() ?? "Unknown";

        try
        {
            using var stream = new NetworkStream(client);
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("AUTH_REQUIRED");
            await writer.FlushAsync();

            string? username = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(username))
            {
                await writer.WriteLineAsync("ERROR: Invalid username");
                return Result.Fail<User>("Invalid username");
            }

            string? password = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(password))
            {
                await writer.WriteLineAsync("ERROR: Invalid password");
                return Result.Fail<User>("Invalid password");
            }

            var userResult = User.FindByUsername(username.Trim());
            if (userResult.IsFailed)
            {
                await writer.WriteLineAsync("ERROR: Authentication failed");
                return userResult.ToResult<User>();
            }

            var user = userResult.Value;
            if (user.IsBanned())
            {
                await writer.WriteLineAsync("ERROR: Account is banned");
                return Result.Fail<User>("Account is banned until " + user.BannedUntil?.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            if (!user.VerifyPassword(password))
            {
                await writer.WriteLineAsync("ERROR: Authentication failed");
                return Result.Fail<User>("Invalid credentials");
            }

            await writer.WriteLineAsync("AUTH_OK");
            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail<User>($"Login error for {remote}: {ex.Message}");
        }
    }

    public static Result<User> BanUser(string username, int minutes)
    {
        var userResult = User.FindByUsername(username);
        if (userResult.IsFailed)
        {
            return userResult.ToResult<User>();
        }

        var user = userResult.Value;
        var banResult = user.BanForMinutes(minutes);
        if (banResult.IsFailed)
        {
            return banResult.ToResult<User>();
        }

        var saveResult = User.SaveUser(user);
        return saveResult.IsSuccess ? Result.Ok(user) : saveResult.ToResult<User>();
    }
}