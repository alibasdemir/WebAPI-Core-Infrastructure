using Application.Repositories;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IJwtHelper _jwtHelper; 
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        public AuthService(IJwtHelper jwtHelper, IUserOperationClaimRepository userOperationClaimRepository)
        {
            _jwtHelper = jwtHelper;
            _userOperationClaimRepository = userOperationClaimRepository;
        }
        public async Task<AccessToken> CreateAccessToken(User user)
        {
            IList<OperationClaim> operationClaims = await _userOperationClaimRepository
                            .Query()
                            .AsNoTracking()
                            .Where(p => p.UserId == user.Id)
                            .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name })
                            .ToListAsync();

            AccessToken accessToken = _jwtHelper.CreateToken(user, operationClaims);
            return accessToken;
        }
    }
}
