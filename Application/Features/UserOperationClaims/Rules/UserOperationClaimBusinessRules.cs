using Application.Repositories;
using Core.Application.Rules;
using Core.Security.Entities;

namespace Application.Features.UserOperationClaims.Rules
{
    public class UserOperationClaimBusinessRules : BaseBusinessRules
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;

        public UserOperationClaimBusinessRules(
            IUserOperationClaimRepository userOperationClaimRepository,
            IUserRepository userRepository,
            IOperationClaimRepository operationClaimRepository)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _userRepository = userRepository;
            _operationClaimRepository = operationClaimRepository;
        }

        /// <summary>
        /// Checks if user operation claim exists by ID
        /// </summary>
        public async Task UserOperationClaimShouldExistWhenSelected(int id)
        {
            UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(
                uoc => uoc.Id == id && !uoc.IsDeleted
            );
            CheckEntityExists(userOperationClaim, "User operation claim not found.");
        }

        /// <summary>
        /// Checks if user exists
        /// </summary>
        public async Task UserShouldExist(int userId)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == userId && !u.IsDeleted);
            CheckEntityExists(user, "User not found.");
        }

        /// <summary>
        /// Checks if operation claim exists
        /// </summary>
        public async Task OperationClaimShouldExist(int operationClaimId)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(
                oc => oc.Id == operationClaimId && !oc.IsDeleted
            );
            CheckEntityExists(operationClaim, "Operation claim not found.");
        }

        /// <summary>
        /// Checks if user already has the operation claim
        /// </summary>
        public async Task UserShouldNotHaveOperationClaim(int userId, int operationClaimId)
        {
            bool exists = await _userOperationClaimRepository.AnyAsync(
                uoc => uoc.UserId == userId && uoc.OperationClaimId == operationClaimId && !uoc.IsDeleted
            );

            if (exists)
                throw new Core.CrossCuttingConcerns.Exceptions.Types.BusinessException(
                    "User already has this operation claim."
                );
        }

        /// <summary>
        /// Checks if user operation claim is not soft deleted
        /// </summary>
        public void UserOperationClaimShouldNotBeDeleted(UserOperationClaim userOperationClaim)
        {
            CheckEntityNotDeleted(userOperationClaim, "User operation claim has been deleted.");
        }
    }
}