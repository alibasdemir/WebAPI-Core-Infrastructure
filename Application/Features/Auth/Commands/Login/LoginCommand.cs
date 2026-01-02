using Application.Features.Auth.Rules;
using Application.Services.AuthService;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDTO>
        {
            private readonly IAuthService _authService;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public LoginCommandHandler(IAuthService authService, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _authService = authService;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }
            public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                // Business rules
                User? user = await _authBusinessRules.UserShouldExistWhenLogin(request.Email);
                _authBusinessRules.PasswordShouldBeCorrect(request.Password, user.PasswordHash, user.PasswordSalt);

                // Create token
                AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

                // Map response
                LoginResponseDTO loginResponse = _mapper.Map<LoginResponseDTO>(createdAccessToken);

                // for manual
                //LoginResponseDTO loginResponse = new LoginResponse
                //{
                //    AccessToken = createdAccessToken
                //};

                return loginResponse;
            }
        }
    }
}
