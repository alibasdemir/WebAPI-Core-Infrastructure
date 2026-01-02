using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<DeleteUserResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<DeleteUserResponseDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                // Get existing user
                User? user = await _userRepository.GetAsync(u => u.Id == request.Id);
                _authBusinessRules.CheckEntityExists(user);

                // Hard delete
                await _userRepository.DeleteAsync(user!);

                DeleteUserResponseDTO response = _mapper.Map<DeleteUserResponseDTO>(user);
                return response;
            }
        }
    }
}