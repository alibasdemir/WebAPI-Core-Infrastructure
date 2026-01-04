namespace Core.Application.Services.UserEmailContent
{
    public interface IEmailLinkService
    {
        string GenerateEmailVerificationLink(string token);
        string GeneratePasswordResetLink(string token);
    }
}
