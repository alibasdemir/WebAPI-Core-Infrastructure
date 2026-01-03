namespace Core.Application.Services.Email
{
    public class EmailMessage
    {
        public List<EmailRecipient> To { get; set; } = new();
        public List<EmailRecipient>? Cc { get; set; }
        public List<EmailRecipient>? Bcc { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string? TextBody { get; set; }
        public string? HtmlBody { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
        public string? UnsubscribeLink { get; set; }
        public Dictionary<string, string>? CustomHeaders { get; set; }

        public EmailMessage() { }

        public EmailMessage(string to, string subject, string htmlBody)
        {
            To = new List<EmailRecipient> { new(to) };
            Subject = subject;
            HtmlBody = htmlBody;
        }

        public void AddRecipient(string email, string? name = null)
        {
            To.Add(new EmailRecipient(email, name));
        }

        public void AddCc(string email, string? name = null)
        {
            Cc ??= new List<EmailRecipient>();
            Cc.Add(new EmailRecipient(email, name));
        }

        public void AddBcc(string email, string? name = null)
        {
            Bcc ??= new List<EmailRecipient>();
            Bcc.Add(new EmailRecipient(email, name));
        }

        public void AddAttachment(string fileName, byte[] content, string contentType = "application/octet-stream")
        {
            Attachments ??= new List<EmailAttachment>();
            Attachments.Add(new EmailAttachment(fileName, content, contentType));
        }
    }
}
