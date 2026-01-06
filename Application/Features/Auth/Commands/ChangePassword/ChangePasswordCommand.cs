using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Logging;
using Core.Security.Entities;
using Core.Security.HashingSalting;
using MediatR;

namespace Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResponseDTO>, ILoggableRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }

        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public ChangePasswordCommandHandler(IUserRepository userRepository, IMapper mapper, AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<ChangePasswordResponseDTO> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                // Get existing user
                User? user = await _userRepository.GetAsync(u => u.Id == request.UserId);
                _authBusinessRules.CheckEntityExists(user);
                _authBusinessRules.UserShouldNotBeDeleted(user!);

                // Verify current password
                _authBusinessRules.PasswordShouldBeCorrect(request.CurrentPassword, user!.PasswordHash, user.PasswordSalt);

                // Validate new password strength
                _authBusinessRules.PasswordShouldMeetRequirements(request.NewPassword);

                // Hash new password
                byte[] newPasswordHash, newPasswordSalt;
                HashingSaltingHelper.CreatePasswordHash(request.NewPassword, out newPasswordHash, out newPasswordSalt);

                // Update password
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
                user.UpdatedDate = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                ChangePasswordResponseDTO response = _mapper.Map<ChangePasswordResponseDTO>(user);
                return response;
            }
        }
    }
}