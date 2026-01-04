using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Core.Security.UserTokenGeneration
{
    public class UserTokenGeneratorService : IUserTokenGeneratorService
    {
        private readonly UserTokenExpirationOptions _expirationOptions;

        public UserTokenGeneratorService(IOptions<UserTokenExpirationOptions> expirationOptions)
        {
            _expirationOptions = expirationOptions.Value;
        }

        public string GenerateEmailVerificationToken()
        {
            // Generate a cryptographically secure random token (32 bytes = 256 bits)
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            
            // Convert to URL-safe base64 string
            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public string GeneratePasswordResetToken()
        {
            // Generate a cryptographically secure random token (32 bytes = 256 bits)
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            
            // Convert to URL-safe base64 string
            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public bool ValidateToken(string token)
        {
            // Basic validation: check if token is not empty and has reasonable length
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // Token should be at least 32 characters (after base64 encoding)
            if (token.Length < 32)
                return false;

            // Check if token contains only valid base64url characters
            foreach (char c in token)
            {
                if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
                    return false;
            }

            return true;
        }

        public DateTime GetEmailVerificationTokenExpiration()
        {
            return DateTime.UtcNow.AddHours(_expirationOptions.EmailVerificationTokenExpirationHours);
        }

        public DateTime GetPasswordResetTokenExpiration()
        {
            return DateTime.UtcNow.AddHours(_expirationOptions.PasswordResetTokenExpirationHours);
        }
    }
}
