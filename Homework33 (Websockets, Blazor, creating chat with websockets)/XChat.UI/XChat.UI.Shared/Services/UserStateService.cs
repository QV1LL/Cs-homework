namespace XChat.UI.Shared.Services;

public static class UserStateService
{
    public static string? Username { get; set; }
    public static Guid? UserId { get; set; }
    public static bool IsAuthenticated => !string.IsNullOrEmpty(Username);
}
