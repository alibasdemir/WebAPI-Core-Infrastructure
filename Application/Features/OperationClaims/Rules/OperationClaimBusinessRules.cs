using Application.Repositories;
using Core.Application.Rules;
using Core.Security.Entities;

namespace Application.Features.OperationClaims.Rules
{
    public class OperationClaimBusinessRules : BaseBusinessRules
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;

        public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository, IUserOperationClaimRepository userOperationClaimRepository)
        {
            _operationClaimRepository = operationClaimRepository;
            _userOperationClaimRepository = userOperationClaimRepository;
        }

        /// <summary>
        /// Checks if operation claim exists by ID
        /// </summary>
        public async Task OperationClaimShouldExistWhenSelected(int id)
        {
            OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(
                oc => oc.Id == id && !oc.IsDeleted
            );
            CheckEntityExists(operationClaim, "Operation claim not found.");
        }

        /// <summary>
        /// Checks if operation claim name is unique (for create)
        /// </summary>
        public async Task OperationClaimNameShouldBeUnique(string name)
        {
            await CheckPropertyIsUniqueWithAny<OperationClaim, IOperationClaimRepository>(
                _operationClaimRepository,
                oc => oc.Name.ToLower() == name.ToLower() && !oc.IsDeleted,
                "An operation claim with this name already exists."
            );
        }

        /// <summary>
        /// Checks if operation claim name is unique (for update)
        /// </summary>
        public async Task OperationClaimNameShouldBeUniqueForUpdate(string name, int exceptId)
        {
            await CheckPropertyIsUniqueExcept<OperationClaim, IOperationClaimRepository>(
                _operationClaimRepository,
                oc => oc.Name.ToLower() == name.ToLower() && !oc.IsDeleted,
                exceptId,
                "An operation claim with this name already exists."
            );
        }

        /// <summary>
        /// Checks if operation claim is not soft deleted
        /// </summary>
        public void OperationClaimShouldNotBeDeleted(OperationClaim operationClaim)
        {
            CheckEntityNotDeleted(operationClaim, "Operation claim has been deleted.");
        }

        /// <summary>
        /// Validates operation claim name format
        /// </summary>
        public void OperationClaimNameShouldBeValid(string name)
        {
            CheckStringNotEmpty(name, "Operation Claim Name");
            CheckStringLength(name, 2, 50, "Operation Claim Name");
        }

        /// <summary>
        /// Checks if operation claim has related users before delete
        /// </summary>
        public async Task OperationClaimShouldNotHaveUsers(int operationClaimId)
        {
            bool hasUsers = await _userOperationClaimRepository.AnyAsync(
                uoc => uoc.OperationClaimId == operationClaimId && !uoc.IsDeleted
            );

            if (hasUsers)
                throw new Core.CrossCuttingConcerns.Exceptions.Types.BusinessException(
                    "Cannot delete operation claim because it is assigned to one or more users."
                );
        }

        /// <summary>
        /// Checks if entity exists
        /// </summary>
        public void CheckEntityExists(OperationClaim? operationClaim)
        {
            CheckEntityExists(operationClaim, "Operation claim not found.");
        }
    }
}