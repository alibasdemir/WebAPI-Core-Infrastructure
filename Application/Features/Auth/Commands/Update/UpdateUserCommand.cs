using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UpdateUserResponseDTO>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<UpdateUserResponseDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                // Get existing user
                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                _authBusinessRules.CheckEntityExists(user);
                _authBusinessRules.UserShouldNotBeDeleted(user!);

                // Business rules validation
                await _authBusinessRules.EmailShouldBeUniqueForUpdate(request.Email, request.Id);
                await _authBusinessRules.UsernameShouldBeUniqueForUpdate(request.UserName, request.Id);

                // Update user properties
                user!.UserName = request.UserName;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                UpdateUserResponseDTO response = _mapper.Map<UpdateUserResponseDTO>(user);
                return response;
            }
        }
    }
}