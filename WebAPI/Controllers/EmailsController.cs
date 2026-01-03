using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Send a simple test email
        /// </summary>
        [HttpPost("send-test")]
        public async Task<IActionResult> SendTestEmail([FromBody] SendTestEmailRequest request)
        {
            var message = new EmailMessage
            {
                To = new List<EmailRecipient> { new(request.RecipientEmail, request.RecipientName) },
                Subject = "Test Email from Web API",
                HtmlBody = $"<h1>Hello {request.RecipientName}!</h1><p>This is a test email from our API.</p>",
                TextBody = $"Hello {request.RecipientName}! This is a test email from our API."
            };

            await _emailService.SendEmailAsync(message);

            return Ok(new { message = "Email sent successfully!" });
        }

        /// <summary>
        /// Send welcome email to new user
        /// </summary>
        [HttpPost("send-welcome")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] string email)
        {
            var message = new EmailMessage(
                to: email,
                subject: "Welcome to Our Platform!",
                htmlBody: @"
                    <div style='font-family: Arial, sans-serif;'>
                        <h1 style='color: #4CAF50;'>Welcome!</h1>
                        <p>Thank you for joining our platform.</p>
                        <p>We're excited to have you on board!</p>
                        <hr/>
                        <p style='font-size: 12px; color: #666;'>
                            If you have any questions, feel free to contact us.
                        </p>
                    </div>"
            );

            await _emailService.SendEmailAsync(message);

            return Ok(new { message = "Welcome email sent!" });
        }

        /// <summary>
        /// Send email with attachment
        /// </summary>
        [HttpPost("send-with-attachment")]
        public async Task<IActionResult> SendEmailWithAttachment([FromForm] SendEmailWithAttachmentRequest request)
        {
            var message = new EmailMessage
            {
                To = new List<EmailRecipient> { new(request.RecipientEmail) },
                Subject = request.Subject,
                HtmlBody = request.Body
            };

            if (request.Attachment != null)
            {
                using var memoryStream = new MemoryStream();
                await request.Attachment.CopyToAsync(memoryStream);
                message.AddAttachment(
                    request.Attachment.FileName,
                    memoryStream.ToArray(),
                    request.Attachment.ContentType);
            }

            await _emailService.SendEmailAsync(message);

            return Ok(new { message = "Email with attachment sent!" });
        }

        /// <summary>
        /// Send bulk emails
        /// </summary>
        [HttpPost("send-bulk")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> SendBulkEmails([FromBody] SendBulkEmailRequest request)
        {
            var messages = request.Recipients.Select(recipient =>
                new EmailMessage
                {
                    To = new List<EmailRecipient> { new(recipient.Email, recipient.Name) },
                    Subject = request.Subject,
                    HtmlBody = request.Body.Replace("{{name}}", recipient.Name)
                }
            );

            await _emailService.SendEmailAsync(messages);

            return Ok(new { message = $"{request.Recipients.Count} emails sent successfully!" });
        }
    }

    #region Request Models

    public class SendTestEmailRequest
    {
        public string RecipientEmail { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
    }

    public class SendEmailWithAttachmentRequest
    {
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IFormFile? Attachment { get; set; }
    }

    public class SendBulkEmailRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<RecipientDto> Recipients { get; set; } = new();
    }

    public class RecipientDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    #endregion
}
