using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SpyMailer;

internal class EmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _appPassword;

    private const string SECRET_MESSAGE = "Wash my belly ^_^, clean my belly =-=";

    public EmailService(string host, int port, string username, string appPassword)
    {
        _host = host;
        _port = port;
        _username = username;
        _appPassword = appPassword;
    }

    public async Task SendMail(string recipientEmail, string recipientName)
    {
        try
        {
            using var client = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = new NetworkCredential(_username, _appPassword),
                EnableSsl = true
            };

            var mail = GetRandomMessage(recipientEmail, recipientName);
            await client.SendMailAsync(mail);
            Console.WriteLine($"✓ Email to {recipientEmail} sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERR] Failed to send to {recipientEmail}: {ex.Message}");
        }
    }

    private MailMessage GetRandomMessage(string recipientEmail, string recipientName)
    {
        var random = new Random();
        int shift = random.Next(1, 26);

        string selectedTemplate = GetRandomTemplate();
        string encrypted = CryptoHelper.Encrypt(SECRET_MESSAGE, shift);

        string template = selectedTemplate
            .Replace("{UserName}", recipientName)
            .Replace("{EncryptedText}", encrypted)
            .Replace("{CaesarKey}", shift.ToString());

        string selectedImage = GetRandomImagePath();

        var mail = CreateMailMessage(recipientEmail, template, selectedImage);
        return mail;
    }

    private string GetRandomTemplate()
    {
        string messagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages.txt");
        if (!File.Exists(messagesPath))
            throw new FileNotFoundException("messages.txt not found!");

        string fileContent = File.ReadAllText(messagesPath);
        var templates = fileContent
            .Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .ToArray();

        if (templates.Length == 0)
            throw new InvalidOperationException("No templates found in messages.txt");

        var random = new Random();
        return templates[random.Next(templates.Length)];
    }

    private string GetRandomImagePath()
    {
        string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        if (!Directory.Exists(imagesFolder))
            throw new DirectoryNotFoundException("images folder not found!");

        var imageFiles = Directory.GetFiles(imagesFolder, "*.*")
            .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                     || f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (imageFiles.Length == 0)
            throw new InvalidOperationException("No images found in images folder");

        var random = new Random();
        return imageFiles[random.Next(imageFiles.Length)];
    }

    private MailMessage CreateMailMessage(string recipientEmail, string htmlTemplate, string imagePath)
    {
        if (!File.Exists(imagePath))
            throw new FileNotFoundException("Image file not found", imagePath);

        var mail = new MailMessage
        {
            From = new MailAddress(_username, "Spammer"),
            Subject = "Random Message from Spammer",
            IsBodyHtml = true
        };

        mail.To.Add(recipientEmail);

        string extension = Path.GetExtension(imagePath).ToLowerInvariant();
        string mimeType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        var inlineImage = new LinkedResource(imagePath, mimeType)
        {
            ContentId = "RandomImage",
            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        };

        if (!htmlTemplate.Contains("cid:"))
        {
            htmlTemplate += $@"<br/><img src='cid:{inlineImage.ContentId}' 
                                style='border-radius:8px; width:300px; margin-top:10px;' />";
        }

        var htmlView = AlternateView.CreateAlternateViewFromString(htmlTemplate, null, "text/html");
        htmlView.LinkedResources.Add(inlineImage);
        mail.AlternateViews.Add(htmlView);

        return mail;
    }
}
