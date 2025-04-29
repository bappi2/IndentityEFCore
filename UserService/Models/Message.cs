using MimeKit;

namespace UserService.Models;

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string? Content { get; set; } // optional HTML or plain text
    public List<string>? Attachments { get; set; } // paths to files

    public Message(IEnumerable<string> to, string subject, string? content = null, List<string>? attachments = null)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("email", x)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }
}