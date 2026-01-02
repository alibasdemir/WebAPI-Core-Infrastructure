using Application.Repositories;
using Application.Services.AuthService;
using AutoMapper;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Entities;
using Core.Security.HashingSalting;
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
            private readonly IUserRepository _userRepository;
            private readonly IAuthService _authService;
            private readonly IMapper _mapper;

            public LoginCommandHandler(IUserRepository userRepository, IAuthService authService, IMapper mapper)
            {
                _userRepository = userRepository;
                _authService = authService;
                _mapper = mapper;
            }
            public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                User? user = await _userRepository.GetAsync(u => u.Email == request.Email);

                if (user is null)
                    throw new BusinessException("Login failed");

                bool isPasswordMatch = HashingSaltingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordMatch)
                    throw new BusinessException("Login failed");

                AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

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
