using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.HashingSalting;
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

            public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
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

                await _userRepository.AddAsync(user);
                RegisterResponseDTO registerResponse = _mapper.Map<RegisterResponseDTO>(user);
                return registerResponse;
            }
        }
    }
}
