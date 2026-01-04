using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Services.Email;
using Core.Application.Services.UserEmailContent;
using Core.Security.Entities;
using Core.Security.HashingSalting;
using Core.Security.UserTokenGeneration;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResponseDTO>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;
            private readonly IUserTokenGeneratorService _tokenGeneratorService;
            private readonly IEmailService _emailService;
            private readonly IEmailTemplateService _emailTemplateService;
            private readonly IEmailLinkService _emailLinkService;

            public RegisterCommandHandler(
                IUserRepository userRepository,
                IMapper mapper,
                AuthBusinessRules authBusinessRules,
                IUserTokenGeneratorService tokenGeneratorService,
                IEmailService emailService,
                IEmailTemplateService emailTemplateService,
                IEmailLinkService emailLinkService)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
                _tokenGeneratorService = tokenGeneratorService;
                _emailService = emailService;
                _emailTemplateService = emailTemplateService;
                _emailLinkService = emailLinkService;
            }

            public async Task<RegisterResponseDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                User user = _mapper.Map<User>(request);

                // Business rules validation
                await _authBusinessRules.UserShouldNotExistWhenRegister(request.Email);
                await _authBusinessRules.UsernameShouldBeUnique(request.UserName);
                _authBusinessRules.PasswordShouldMeetRequirements(request.Password);

                // Hash password
                byte[] passwordHash, passwordSalt;
                HashingSaltingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

                // Generate email verification token with expiration
                user.EmailVerificationToken = _tokenGeneratorService.GenerateEmailVerificationToken();
                user.EmailVerificationTokenExpires = _tokenGeneratorService.GetEmailVerificationTokenExpiration();
                user.IsEmailVerified = false;

                await _userRepository.AddAsync(user);

                // Send verification email
                try
                {
                    string verificationLink = _emailLinkService.GenerateEmailVerificationLink(user.EmailVerificationToken);

                    var verificationEmailHtml = _emailTemplateService.GetVerificationEmailTemplate(user.FirstName, verificationLink);
                    var emailMessage = new EmailMessage(
                        to: user.Email,
                        subject: "Verify Your Email Address",
                        htmlBody: verificationEmailHtml
                    );
                    await _emailService.SendEmailAsync(emailMessage, cancellationToken);
                }
                catch
                {
                    // Email send failure should not prevent registration
                    // User can request resend later
                }

                RegisterResponseDTO registerResponse = _mapper.Map<RegisterResponseDTO>(user);
                return registerResponse;
            }
        }
    }
}
