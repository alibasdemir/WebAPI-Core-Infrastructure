namespace Core.Security.UserTokenGeneration
{
    public interface IUserTokenGeneratorService
    {
        string GenerateEmailVerificationToken();
        string GeneratePasswordResetToken();
        bool ValidateToken(string token);
        DateTime GetEmailVerificationTokenExpiration();
        DateTime GetPasswordResetTokenExpiration();
    }
}
