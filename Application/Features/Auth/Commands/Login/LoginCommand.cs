using Application.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Pagination;
using Core.Security.Entities;
using Core.Security.HashingSalting;
using Core.Security.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AccessToken>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, AccessToken>
        {
            private readonly IUserRepository _userRepository;
            private readonly IJwtHelper _jwtHelper;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;

            public LoginCommandHandler(IUserRepository userRepository, IJwtHelper jwtHelper, IUserOperationClaimRepository userOperationClaimRepository)
            {
                _userRepository = userRepository;
                _jwtHelper = jwtHelper;
                _userOperationClaimRepository = userOperationClaimRepository;
            }
            public async Task<AccessToken> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                User? user = await _userRepository.GetAsync(u => u.Email == request.Email);

                if (user is null)
                    throw new BusinessException("Login failed");

                bool isPasswordMatch = HashingSaltingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);

                if (!isPasswordMatch)
                    throw new BusinessException("Login failed");

                // Get user operation claims (roles/permissions)
                IPaginate<UserOperationClaim> userOperationClaimsPaginate = await _userOperationClaimRepository.GetListAsync(i => i.UserId == user.Id, include: i => i.Include(i => i.OperationClaim));
                List<UserOperationClaim> userOperationClaims = userOperationClaimsPaginate.Items.ToList();

                return _jwtHelper.CreateToken(user, userOperationClaims.Select(i => i.OperationClaim).ToList());
            }
        }
    }
}
