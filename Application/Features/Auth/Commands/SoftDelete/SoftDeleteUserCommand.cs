using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.SoftDeleteUser
{
    public class SoftDeleteUserCommand : IRequest<SoftDeleteUserResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class SoftDeleteUserCommandHandler : IRequestHandler<SoftDeleteUserCommand, SoftDeleteUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public SoftDeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<SoftDeleteUserResponseDTO> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
            {
                // Get existing user
                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                _authBusinessRules.CheckEntityExists(user);
                _authBusinessRules.UserShouldNotBeDeleted(user!);

                // Soft delete
                await _userRepository.SoftDeleteAsync(user!);

                SoftDeleteUserResponseDTO response = _mapper.Map<SoftDeleteUserResponseDTO>(user);
                return response;
            }
        }
    }
}