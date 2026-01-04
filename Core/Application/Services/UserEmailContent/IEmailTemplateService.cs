namespace Core.Application.Services.UserEmailContent
{
    public interface IEmailTemplateService
    {
        string GetVerificationEmailTemplate(string userName, string verificationLink);
        string GetPasswordResetEmailTemplate(string userName, string resetLink);
        string GetPasswordChangedEmailTemplate(string userName);
        string GetWelcomeEmailTemplate(string userName);
    }
}
