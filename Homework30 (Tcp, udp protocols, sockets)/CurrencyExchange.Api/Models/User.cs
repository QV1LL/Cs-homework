using FluentResults;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyExchange.Api.Models
{
    public class User
    {
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public DateTimeOffset? BannedUntil { get; private set; }

        private const string USERS_FILE_NAME = "users.json";

        private User() { }

        [JsonConstructor]
        public User(string Username, string Password, DateTimeOffset? BannedUntil = null)
        {
            this.Username = Username;
            this.Password = Password;
            this.BannedUntil = BannedUntil;
        }

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, Password);
        }

        public static Result<List<User>> LoadAllUsers()
        {
            try
            {
                if (!File.Exists(USERS_FILE_NAME))
                {
                    return Result.Fail<List<User>>("Users file not found.");
                }

                var json = File.ReadAllText(USERS_FILE_NAME);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                return Result.Ok(users);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<User>>($"Failed to load users: {ex.Message}");
            }
        }

        public static Result<User> FindByUsername(string username)
        {
            var allUsers = LoadAllUsers();
            if (allUsers.IsFailed) return allUsers.ToResult<User>();

            var user = allUsers.Value.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return user != null ? Result.Ok(user) : Result.Fail<User>($"User '{username}' not found.");
        }

        public bool IsBanned() => BannedUntil.HasValue && BannedUntil > DateTimeOffset.UtcNow;

        public Result<User> BanForMinutes(int minutes)
        {
            if (minutes <= 0)
                return Result.Fail<User>("Ban time must be positive.");

            BannedUntil = DateTimeOffset.UtcNow.AddMinutes(minutes);
            return Result.Ok(this);
        }

        public static Result<bool> SaveUser(User user)
        {
            try
            {
                var allUsers = LoadAllUsers();
                if (allUsers.IsFailed) return false;

                var existingIndex = allUsers.Value.FindIndex(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
                if (existingIndex >= 0)
                {
                    allUsers.Value[existingIndex] = user;
                }
                else
                {
                    allUsers.Value.Add(user);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(allUsers.Value, options);
                File.WriteAllText(USERS_FILE_NAME, json);
                return Result.Ok(true);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>($"Failed to save user: {ex.Message}");
            }
        }
    }
}