namespace Core.Application.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
        Task SendEmailAsync(IEnumerable<EmailMessage> messages, CancellationToken cancellationToken = default);
    }
}
