using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UserService.Models;

namespace UserService.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;

    public EmailService(EmailConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(Message message)
    {
        var email = CreateEmailMessage(message);
        Send(email);
    }

    private void Send(MimeMessage email)
    {
        using var client = new SmtpClient();
        client.Connect(_config.SmtpServer, _config.Port, SecureSocketOptions.SslOnConnect); // ✅ Required for port 465
        client.Authenticate(_config.Username, _config.Password);
        client.Send(email);
        client.Disconnect(true);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("email",_config.From));
        email.To.AddRange(message.To);
        email.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Content,
            TextBody = message.Content
        };

        if (message.Attachments != null)
        {
            foreach (var attachment in message.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment);
            }
        }

        email.Body = bodyBuilder.ToMessageBody();
        return email;
    }
}