using Application.Features.Auth.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Logging;
using Core.Application.Services.Email;
using Core.Application.Services.UserEmailContent;
using Core.Security.Entities;
using Core.Security.UserTokenGeneration;
using MediatR;

namespace Application.Features.Auth.Commands.ResendVerificationEmail
{
    public class ResendVerificationEmailCommandHandler : IRequestHandler<ResendVerificationEmailCommand, ResendVerificationEmailResponseDTO>, ILoggableRequest
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserTokenGeneratorService _tokenGeneratorService;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailLinkService _emailLinkService;

        public ResendVerificationEmailCommandHandler(
            IUserRepository userRepository, 
            AuthBusinessRules authBusinessRules,
            IUserTokenGeneratorService tokenGeneratorService,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IEmailLinkService emailLinkService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _tokenGeneratorService = tokenGeneratorService;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _emailLinkService = emailLinkService;
        }

        public async Task<ResendVerificationEmailResponseDTO> Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
        {
            // Find user
            User? user = await _authBusinessRules.UserShouldExistForResendVerification(request.Email);

            // Check if already verified
            _authBusinessRules.EmailShouldNotBeAlreadyVerified(user.IsEmailVerified);

            // Generate new verification token with expiration
            user.EmailVerificationToken = _tokenGeneratorService.GenerateEmailVerificationToken();
            user.EmailVerificationTokenExpires = _tokenGeneratorService.GetEmailVerificationTokenExpiration();
            
            await _userRepository.UpdateAsync(user);

            // Create verification link
            string verificationLink = _emailLinkService.GenerateEmailVerificationLink(user.EmailVerificationToken);

            // Send verification email
            try
            {
                var verificationEmailHtml = _emailTemplateService.GetVerificationEmailTemplate(user.FirstName, verificationLink);
                var emailMessage = new EmailMessage(
                    to: user.Email,
                    subject: "Verify Your Email Address",
                    htmlBody: verificationEmailHtml
                );
                await _emailService.SendEmailAsync(emailMessage, cancellationToken);
            }
            catch (Exception)
            {
                throw new Exception("Failed to send verification email. Please try again later.");
            }

            return new ResendVerificationEmailResponseDTO
            {
                Success = true,
                Message = "Verification email has been sent. Please check your inbox."
            };
        }
    }
}
