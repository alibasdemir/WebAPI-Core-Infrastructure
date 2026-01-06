using Application.Features.Auth.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Logging;
using Core.Application.Services.Email;
using Core.Application.Services.UserEmailContent;
using Core.Security.Entities;
using Core.Security.HashingSalting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponseDTO>, ILoggableRequest
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;

        public ResetPasswordCommandHandler(
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

        public async Task<ResetPasswordResponseDTO> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Validate passwords match
            _authBusinessRules.PasswordsShouldMatch(request.NewPassword, request.ConfirmPassword);
            
            // Validate password strength
            _authBusinessRules.PasswordShouldMeetRequirements(request.NewPassword);
            
            // Validate token format
            _authBusinessRules.ResetTokenShouldBeValid(request.Token);

            // Find user by reset token
            User? user = await _userRepository.Query()
                .Where(u => u.PasswordResetToken == request.Token)
                .FirstOrDefaultAsync(cancellationToken);

            // Business rules
            _authBusinessRules.UserShouldExistForPasswordReset(user);
            _authBusinessRules.PasswordResetTokenShouldNotBeExpired(user!.PasswordResetTokenExpires);

            // Create new password hash
            byte[] newPasswordHash, newPasswordSalt;
            HashingSaltingHelper.CreatePasswordHash(request.NewPassword, out newPasswordHash, out newPasswordSalt);

            // Update user password and clear reset token
            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            user.UpdatedDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            // Send confirmation email
            try
            {
                var confirmationEmailHtml = _emailTemplateService.GetPasswordChangedEmailTemplate(user.FirstName);
                var emailMessage = new EmailMessage(
                    to: user.Email,
                    subject: "Password Changed Successfully",
                    htmlBody: confirmationEmailHtml
                );
                await _emailService.SendEmailAsync(emailMessage, cancellationToken);
            }
            catch
            {
                // Email send failure should not prevent password reset completion
            }

            return new ResetPasswordResponseDTO
            {
                Success = true,
                Message = "Password has been reset successfully. You can now login with your new password."
            };
        }
    }
}
