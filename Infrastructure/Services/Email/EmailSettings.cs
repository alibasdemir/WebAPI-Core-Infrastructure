namespace Infrastructure.Services.Email
{
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; } = true;
        public bool RequiresAuthentication { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 30;

        // DKIM Settings (Optional)
        public string? DkimDomain { get; set; }
        public string? DkimSelector { get; set; }
        public string? DkimPrivateKey { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Server))
                throw new ArgumentException("SMTP Server is required.", nameof(Server));
            if (Port <= 0 || Port > 65535)
                throw new ArgumentException("Port must be between 1 and 65535.", nameof(Port));
            if (string.IsNullOrWhiteSpace(SenderEmail))
                throw new ArgumentException("Sender email is required.", nameof(SenderEmail));
            if (RequiresAuthentication && string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username is required when authentication is enabled.", nameof(Username));
        }
    }
}
