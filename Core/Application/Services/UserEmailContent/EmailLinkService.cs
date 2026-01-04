using Microsoft.Extensions.Configuration;

namespace Core.Application.Services.UserEmailContent
{
    public class EmailLinkService : IEmailLinkService
    {
        private readonly string _emailverificationUrl;

        public EmailLinkService(IConfiguration configuration)
        {
            _emailverificationUrl = configuration["EmailverificationUrl"] ?? "http://localhost:7068";
        }

        public string GenerateEmailVerificationLink(string token)
        {
            return $"{_emailverificationUrl}/verify-email?token={token}";
        }

        public string GeneratePasswordResetLink(string token)
        {
            return $"{_emailverificationUrl}/reset-password?token={token}";
        }
    }
}
