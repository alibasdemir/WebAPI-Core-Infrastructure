using Application.Features.Auth.Rules;
using Application.Repositories;
using Core.Application.Services.Email;
using Core.Application.Services.UserEmailContent;
using Core.Security.Entities;
using Core.Security.UserTokenGeneration;
using MediatR;

namespace Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserTokenGeneratorService _tokenGeneratorService;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailLinkService _emailLinkService;

        public ForgotPasswordCommandHandler(
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

        public async Task<ForgotPasswordResponseDTO> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find user by email
            User? user = await _authBusinessRules.UserShouldExistWhenRequestingPasswordReset(request.Email);

            // Generate password reset token with expiration
            user.PasswordResetToken = _tokenGeneratorService.GeneratePasswordResetToken();
            user.PasswordResetTokenExpires = _tokenGeneratorService.GetPasswordResetTokenExpiration();

            await _userRepository.UpdateAsync(user);

            // Create reset link
            string resetLink = _emailLinkService.GeneratePasswordResetLink(user.PasswordResetToken);

            // Send password reset email
            try
            {
                var resetEmailHtml = _emailTemplateService.GetPasswordResetEmailTemplate(user.FirstName, resetLink);
                var emailMessage = new EmailMessage(
                    to: user.Email,
                    subject: "Reset Your Password",
                    htmlBody: resetEmailHtml
                );
                await _emailService.SendEmailAsync(emailMessage, cancellationToken);
            }
            catch (Exception)
            {
                // Rollback token if email fails
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpires = null;
                await _userRepository.UpdateAsync(user);

                throw new Exception("Failed to send password reset email. Please try again later.");
            }

            return new ForgotPasswordResponseDTO
            {
                Success = true,
                Message = "Password reset link has been sent to your email."
            };
        }
    }
}
