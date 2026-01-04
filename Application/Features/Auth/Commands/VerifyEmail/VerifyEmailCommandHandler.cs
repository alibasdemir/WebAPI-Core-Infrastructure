using Application.Repositories;
using Application.Features.Auth.Rules;
using Core.Application.Services.Email;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Core.Application.Services.UserEmailContent;

namespace Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public VerifyEmailCommandHandler(
            IUserRepository userRepository, 
            AuthBusinessRules authBusinessRules,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task<VerifyEmailResponseDTO> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            // Validate token format
            _authBusinessRules.VerificationTokenShouldBeValid(request.Token);

            // Find user by verification token
            User? user = await _userRepository.Query()
                .Where(u => u.EmailVerificationToken == request.Token)
                .FirstOrDefaultAsync(cancellationToken);

            // Business rules
            _authBusinessRules.UserShouldExistForVerification(user);
            _authBusinessRules.EmailVerificationTokenShouldNotBeExpired(user!.EmailVerificationTokenExpires);
            _authBusinessRules.EmailShouldNotBeAlreadyVerified(user!.IsEmailVerified);

            // Update user
            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpires = null;

            await _userRepository.UpdateAsync(user);

            // Send welcome email
            try
            {
                var welcomeEmailHtml = _emailTemplateService.GetWelcomeEmailTemplate(user.FirstName);
                var emailMessage = new EmailMessage(
                    to: user.Email,
                    subject: "Welcome! Your Email is Verified",
                    htmlBody: welcomeEmailHtml
                );
                await _emailService.SendEmailAsync(emailMessage, cancellationToken);
            }
            catch
            {
                // Email send failure should not prevent verification completion
            }

            return new VerifyEmailResponseDTO
            {
                Success = true,
                Message = "Email verified successfully. You can now login."
            };
        }
    }
}
