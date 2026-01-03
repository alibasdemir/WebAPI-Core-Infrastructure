namespace Core.Application.Services.Email
{
    public class EmailRecipient
    {
        public string Email { get; set; }
        public string? Name { get; set; }

        public EmailRecipient(string email, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            Email = email;
            Name = name;
        }
    }
}
