using Application.Features.Auth.Rules;
using Application.Repositories;
using Application.Services.AuthService;
using AutoMapper;
using Core.Application.Pipelines.Logging;
using Core.Extensions;
using Core.Security.Entities;
using Core.Security.JWT;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDTO>, ILoggableRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? IpAddress { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDTO>
        {
            private readonly IAuthService _authService;
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public LoginCommandHandler(
                IAuthService authService, 
                IUserRepository userRepository,
                IMapper mapper, 
                AuthBusinessRules authBusinessRules,
                IHttpContextAccessor httpContextAccessor)
            {
                _authService = authService;
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                // Business rules
                User? user = await _authBusinessRules.UserShouldExistWhenLogin(request.Email);
                _authBusinessRules.PasswordShouldBeCorrect(request.Password, user.PasswordHash, user.PasswordSalt);
                
                // Check email verification
                _authBusinessRules.EmailShouldBeVerified(user.IsEmailVerified);

                // Update last login info
                string ipAddress = request.IpAddress ?? _httpContextAccessor.HttpContext.GetClientIpAddress();
                user.LastLoginDate = DateTime.UtcNow;
                user.LastLoginIp = ipAddress;
                await _userRepository.UpdateAsync(user);

                // Create token
                AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

                // Map response
                LoginResponseDTO loginResponse = _mapper.Map<LoginResponseDTO>(createdAccessToken);

                return loginResponse;
            }
        }
    }
}
