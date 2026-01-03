using Core.Application.Services.Email;
using Core.Application.Services.Email.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

namespace Infrastructure.Services.Email.MailKit
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<MailKitEmailService> _logger;
        private readonly DkimSigner? _dkimSigner;

        public MailKitEmailService(
            IOptions<EmailSettings> settings,
            ILogger<MailKitEmailService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _settings.Validate();
            _dkimSigner = InitializeDkimSigner();
        }

        public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.To == null || !message.To.Any())
            {
                _logger.LogWarning("Email send skipped: No recipients provided.");
                return;
            }

            try
            {
                var mimeMessage = BuildMimeMessage(message);
                await SendMimeMessageAsync(mimeMessage, cancellationToken);

                _logger.LogInformation(
                    "Email sent successfully to {RecipientCount} recipient(s). Subject: {Subject}",
                    message.To.Count,
                    message.Subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email. Subject: {Subject}", message.Subject);
                throw new EmailSendException($"Failed to send email: {ex.Message}", ex);
            }
        }

        public async Task SendEmailAsync(IEnumerable<EmailMessage> messages, CancellationToken cancellationToken = default)
        {
            if (messages == null || !messages.Any())
            {
                _logger.LogWarning("Bulk email send skipped: No messages provided.");
                return;
            }

            var tasks = messages.Select(msg => SendEmailAsync(msg, cancellationToken));
            await Task.WhenAll(tasks);
        }

        private MimeMessage BuildMimeMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();

            // Sender
            mimeMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

            // Recipients
            mimeMessage.To.AddRange(message.To.Select(r => new MailboxAddress(r.Name, r.Email)));

            if (message.Cc != null && message.Cc.Any())
                mimeMessage.Cc.AddRange(message.Cc.Select(r => new MailboxAddress(r.Name, r.Email)));

            if (message.Bcc != null && message.Bcc.Any())
                mimeMessage.Bcc.AddRange(message.Bcc.Select(r => new MailboxAddress(r.Name, r.Email)));

            // Subject
            mimeMessage.Subject = message.Subject;

            // Custom Headers
            if (!string.IsNullOrWhiteSpace(message.UnsubscribeLink))
                mimeMessage.Headers.Add("List-Unsubscribe", $"<{message.UnsubscribeLink}>");

            if (message.CustomHeaders != null)
            {
                foreach (var header in message.CustomHeaders)
                    mimeMessage.Headers.Add(header.Key, header.Value);
            }

            // Body
            var bodyBuilder = new BodyBuilder
            {
                TextBody = message.TextBody,
                HtmlBody = message.HtmlBody
            };

            // Attachments
            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add(
                        attachment.FileName,
                        attachment.Content,
                        ContentType.Parse(attachment.ContentType));
                }
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            // DKIM Signing
            if (_dkimSigner != null)
            {
                mimeMessage.Prepare(EncodingConstraint.SevenBit);
                _dkimSigner.Sign(mimeMessage, new[] { HeaderId.From, HeaderId.Subject, HeaderId.To });
            }

            return mimeMessage;
        }

        private async Task SendMimeMessageAsync(MimeMessage mimeMessage, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();

            try
            {
                client.Timeout = _settings.TimeoutSeconds * 1000;

                await client.ConnectAsync(
                    _settings.Server,
                    _settings.Port,
                    _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None,
                    cancellationToken);

                if (_settings.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
                }

                await client.SendAsync(mimeMessage, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP error occurred while sending email.");
                throw;
            }
        }

        private DkimSigner? InitializeDkimSigner()
        {
            if (string.IsNullOrWhiteSpace(_settings.DkimPrivateKey) ||
                string.IsNullOrWhiteSpace(_settings.DkimSelector) ||
                string.IsNullOrWhiteSpace(_settings.DkimDomain))
            {
                _logger.LogInformation("DKIM signing is disabled (missing configuration).");
                return null;
            }

            try
            {
                var privateKey = ReadPrivateKeyFromPem(_settings.DkimPrivateKey);

                return new DkimSigner(privateKey, _settings.DkimDomain, _settings.DkimSelector)
                {
                    HeaderCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    BodyCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    AgentOrUserIdentifier = $"@{_settings.DkimDomain}",
                    QueryMethod = "dns/txt"
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize DKIM signer. Emails will be sent without DKIM signature.");
                return null;
            }
        }

        private AsymmetricKeyParameter ReadPrivateKeyFromPem(string privateKey)
        {
            var pemKey = $"-----BEGIN RSA PRIVATE KEY-----\n{privateKey}\n-----END RSA PRIVATE KEY-----";

            using var stringReader = new StringReader(pemKey);
            var pemReader = new PemReader(stringReader);
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();

            return keyPair.Private;
        }
    }
}