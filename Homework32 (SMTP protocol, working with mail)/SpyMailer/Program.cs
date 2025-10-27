using System.Text.Json;

namespace SpyMailer;

internal class Program
{
    private static async Task Main()
    {
        Console.Title = "SpyMailer.exe";
        Console.WriteLine("> SpyMailer.exe");
        Console.WriteLine("[1] Send All Emails");
        Console.WriteLine("[2] Test Single");
        Console.WriteLine("[3] Encrypt Text");
        Console.Write("Виберіть: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await SendAllEmailsAsync();
                break;
            case "2":
                await TestSingleAsync();
                break;
            case "3":
                EncryptTextMenu();
                break;
            default:
                Console.WriteLine("Невірний вибір!");
                break;
        }

        Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }

    private static async Task SendAllEmailsAsync()
    {
        try
        {
            var config = LoadConfig();
            var appPassword = GetAppPassword();
            var service = new EmailService(config.Host, config.Port, config.Username, appPassword);

            var emailsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emails.txt");
            if (!File.Exists(emailsFile))
                throw new FileNotFoundException("emails.txt not found!");

            var lines = (await File.ReadAllLinesAsync(emailsFile))
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToArray();

            int total = lines.Length, success = 0;

            Console.WriteLine("\nШифруємо... Відправляємо...");

            var tasks = lines.Select(async line =>
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) return;

                string name = parts[0];
                string email = parts[1];

                try
                {
                    await service.SendMail(email, name);
                    success++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERR] {email}: {ex.Message}");
                }
            });

            await Task.WhenAll(tasks);

            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sent_emails.log");
            File.AppendAllText(logPath, $"{DateTime.Now}: {success}/{total} emails sent successfully{Environment.NewLine}");

            Console.WriteLine($"✓ {success}/{total} листів успішно!");
            Console.WriteLine($"Лог збережено: {logPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }

    private static async Task TestSingleAsync()
    {
        var config = LoadConfig();
        var appPassword = GetAppPassword();
        var service = new EmailService(config.Host, config.Port, config.Username, appPassword);

        Console.Write("Введіть ім'я: ");
        string name = Console.ReadLine() ?? "User";

        Console.Write("Введіть email: ");
        string email = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Тестове відправлення...");
        await service.SendMail(email, name);

        Console.WriteLine("Тестовий лист відправлено!");
    }

    private static void EncryptTextMenu()
    {
        Console.Write("Введіть текст для шифрування: ");
        string input = Console.ReadLine() ?? string.Empty;

        Console.Write("Зсув (0–25): ");
        if (!int.TryParse(Console.ReadLine(), out int shift))
            shift = 3;

        string encrypted = CryptoHelper.Encrypt(input, shift);
        Console.WriteLine($"\n🔐 Зашифрований текст: {encrypted}");
        Console.WriteLine($"🔑 Ключ (Caesar Shift): {shift}");
    }

    private static Config LoadConfig()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        if (!File.Exists(configPath))
            throw new FileNotFoundException("config.json not found!");

        string json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<Config>(json);
        return config ?? throw new InvalidOperationException("Invalid config.json format!");
    }

    private static string GetAppPassword()
    {
        var env = Environment.GetEnvironmentVariable("MAIL_APP_PASS");
        if (!string.IsNullOrWhiteSpace(env))
        {
            return env;
        }

        return AskAppPassword();
    }

    private static string AskAppPassword()
    {
        Console.Write("Введіть пароль (App Password): ");
        var password = string.Empty;

        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password.Trim();
    }

    private sealed record Config
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
    }
}
