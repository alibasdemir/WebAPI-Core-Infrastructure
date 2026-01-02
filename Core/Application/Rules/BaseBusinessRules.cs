using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.DataAccess;
using System.Linq.Expressions;

namespace Core.Application.Rules
{
    public abstract class BaseBusinessRules
    {
        #region Entity Existence Checks

        /// <summary>
        /// Throws NotFoundException if entity is null
        /// </summary>
        protected void CheckEntityExists<TEntity>(TEntity? entity, string? customMessage = null)
            where TEntity : class
        {
            if (entity is null)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} not found.";
                throw new NotFoundException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if entity is null
        /// </summary>
        protected void CheckEntityExistsForBusiness<TEntity>(TEntity? entity, string? customMessage = null)
            where TEntity : class
        {
            if (entity is null)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} not found.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if entity already exists
        /// </summary>
        protected void CheckEntityShouldNotExist<TEntity>(TEntity? entity, string? customMessage = null)
            where TEntity : class
        {
            if (entity is not null)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} already exists.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Uniqueness Checks

        /// <summary>
        /// Checks if a property value is unique in the repository
        /// </summary>
        protected async Task CheckPropertyIsUnique<TEntity, TRepository>(
            TRepository repository,
            Expression<Func<TEntity, bool>> predicate,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            TEntity? existingEntity = await repository.GetAsync(predicate);

            if (existingEntity is not null)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} with this value already exists.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Checks if a property value is unique except for a specific entity (useful for updates)
        /// </summary>
        protected async Task CheckPropertyIsUniqueExcept<TEntity, TRepository>(
            TRepository repository,
            Expression<Func<TEntity, bool>> predicate,
            int exceptId,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            TEntity? existingEntity = await repository.GetAsync(predicate);

            if (existingEntity is not null && existingEntity.Id != exceptId)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} with this value already exists.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Boolean Condition Checks

        /// <summary>
        /// Throws BusinessException if condition is false
        /// </summary>
        protected void CheckBusinessRule(bool condition, string message)
        {
            if (!condition)
                throw new BusinessException(message);
        }

        /// <summary>
        /// Throws BusinessException if condition is true
        /// </summary>
        protected void CheckBusinessRuleInverted(bool condition, string message)
        {
            if (condition)
                throw new BusinessException(message);
        }

        /// <summary>
        /// Throws AuthorizationException if condition is false
        /// </summary>
        protected void CheckAuthorization(bool condition, string message)
        {
            if (!condition)
                throw new AuthorizationException(message);
        }

        #endregion

        #region Collection Checks

        /// <summary>
        /// Throws BusinessException if collection is null or empty
        /// </summary>
        protected void CheckCollectionNotEmpty<T>(IEnumerable<T>? collection, string? customMessage = null)
        {
            if (collection is null || !collection.Any())
            {
                string message = customMessage ?? "Collection cannot be empty.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if collection exceeds max count
        /// </summary>
        protected void CheckCollectionMaxCount<T>(IEnumerable<T>? collection, int maxCount, string? customMessage = null)
        {
            if (collection is not null && collection.Count() > maxCount)
            {
                string message = customMessage ?? $"Collection cannot exceed {maxCount} items.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region String Validation

        /// <summary>
        /// Throws BusinessException if string is null, empty or whitespace
        /// </summary>
        protected void CheckStringNotEmpty(string? value, string parameterName, string? customMessage = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                string message = customMessage ?? $"{parameterName} cannot be empty.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if string length is invalid
        /// </summary>
        protected void CheckStringLength(string? value, int minLength, int maxLength, string parameterName, string? customMessage = null)
        {
            if (value is null)
            {
                throw new BusinessException($"{parameterName} cannot be null.");
            }

            if (value.Length < minLength || value.Length > maxLength)
            {
                string message = customMessage ?? $"{parameterName} must be between {minLength} and {maxLength} characters.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Numeric Validation

        /// <summary>
        /// Throws BusinessException if number is negative
        /// </summary>
        protected void CheckNumberNotNegative(decimal number, string parameterName, string? customMessage = null)
        {
            if (number < 0)
            {
                string message = customMessage ?? $"{parameterName} cannot be negative.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if number is not in range
        /// </summary>
        protected void CheckNumberInRange(decimal number, decimal min, decimal max, string parameterName, string? customMessage = null)
        {
            if (number < min || number > max)
            {
                string message = customMessage ?? $"{parameterName} must be between {min} and {max}.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Date Validation

        /// <summary>
        /// Throws BusinessException if date is in the past
        /// </summary>
        protected void CheckDateNotPast(DateTime date, string parameterName, string? customMessage = null)
        {
            if (date < DateTime.UtcNow)
            {
                string message = customMessage ?? $"{parameterName} cannot be in the past.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if date is in the future
        /// </summary>
        protected void CheckDateNotFuture(DateTime date, string parameterName, string? customMessage = null)
        {
            if (date > DateTime.UtcNow)
            {
                string message = customMessage ?? $"{parameterName} cannot be in the future.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Throws BusinessException if end date is before start date
        /// </summary>
        protected void CheckDateRange(DateTime startDate, DateTime endDate, string? customMessage = null)
        {
            if (endDate < startDate)
            {
                string message = customMessage ?? "End date cannot be before start date.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Enum Validation

        /// <summary>
        /// Throws BusinessException if enum value is not defined
        /// </summary>
        protected void CheckEnumIsDefined<TEnum>(TEnum value, string parameterName, string? customMessage = null)
            where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                string message = customMessage ?? $"Invalid {parameterName} value.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Relationship Validation

        /// <summary>
        /// Throws BusinessException if entity has related records (useful before delete)
        /// </summary>
        protected void CheckNoRelatedRecords(bool hasRelatedRecords, string relationshipName, string? customMessage = null)
        {
            if (hasRelatedRecords)
            {
                string message = customMessage ?? $"Cannot proceed because there are related {relationshipName}.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Soft Delete Checks

        /// <summary>
        /// Throws BusinessException if entity is soft deleted
        /// </summary>
        protected void CheckEntityNotDeleted<TEntity>(TEntity? entity, string? customMessage = null)
            where TEntity : Entity
        {
            if (entity is not null && entity.IsDeleted)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} has been deleted.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Custom Async Validations

        /// <summary>
        /// Executes async business rule and throws BusinessException if fails
        /// </summary>
        protected async Task CheckAsyncBusinessRule(Func<Task<bool>> asyncCondition, string message)
        {
            bool result = await asyncCondition();
            if (!result)
                throw new BusinessException(message);
        }

        #endregion

        #region Performance-Optimized Uniqueness Checks (using AnyAsync)

        /// <summary>
        /// Checks if a property value is unique using AnyAsync (more performant)
        /// </summary>
        protected async Task CheckPropertyIsUniqueWithAny<TEntity, TRepository>(
            TRepository repository,
            Expression<Func<TEntity, bool>> predicate,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            bool exists = await repository.AnyAsync(predicate);

            if (exists)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} with this value already exists.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Checks if entity exists by ID using AnyAsync
        /// </summary>
        protected async Task CheckEntityExistsByIdAsync<TEntity, TRepository>(
            TRepository repository,
            int id,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            bool exists = await repository.AnyAsync(e => e.Id == id);

            if (!exists)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"{entityName} not found.";
                throw new NotFoundException(message);
            }
        }

        /// <summary>
        /// Checks if entity count exceeds limit
        /// </summary>
        protected async Task CheckEntityCountLimit<TEntity, TRepository>(
            TRepository repository,
            Expression<Func<TEntity, bool>>? predicate,
            int maxCount,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            int count = await repository.CountAsync(predicate);

            if (count >= maxCount)
            {
                string entityName = typeof(TEntity).Name;
                string message = customMessage ?? $"Maximum {entityName} limit ({maxCount}) reached.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Email & Username Validation

        /// <summary>
        /// Validates email format using regex
        /// </summary>
        protected void CheckEmailFormat(string email, string? customMessage = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessException("Email cannot be empty.");
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                string message = customMessage ?? "Invalid email format.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Validates username format (alphanumeric, underscore, dot)
        /// </summary>
        protected void CheckUsernameFormat(string username, string? customMessage = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new BusinessException("Username cannot be empty.");
            }

            string usernamePattern = @"^[a-zA-Z0-9._]{3,20}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, usernamePattern))
            {
                string message = customMessage ?? "Username must be 3-20 characters and contain only letters, numbers, dots, and underscores.";
                throw new BusinessException(message);
            }
        }

        /// <summary>
        /// Validates password strength
        /// </summary>
        protected void CheckPasswordStrength(string password, string? customMessage = null)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new BusinessException("Password cannot be empty.");
            }

            bool hasMinLength = password.Length >= 8;
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            if (!hasMinLength || !hasUpperCase || !hasLowerCase || !hasDigit)
            {
                string message = customMessage ?? "Password must be at least 8 characters and contain uppercase, lowercase, and numbers.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Guid Validation

        /// <summary>
        /// Throws BusinessException if Guid is empty
        /// </summary>
        protected void CheckGuidNotEmpty(Guid guid, string parameterName, string? customMessage = null)
        {
            if (guid == Guid.Empty)
            {
                string message = customMessage ?? $"{parameterName} cannot be empty.";
                throw new BusinessException(message);
            }
        }

        #endregion

        #region Batch Validation

        /// <summary>
        /// Validates multiple entities in a collection
        /// </summary>
        protected async Task CheckBatchUniqueness<TEntity, TRepository>(
            TRepository repository,
            IEnumerable<TEntity> entities,
            Func<TEntity, Expression<Func<TEntity, bool>>> predicateSelector,
            string? customMessage = null)
            where TEntity : Entity
            where TRepository : IAsyncRepository<TEntity>
        {
            foreach (var entity in entities)
            {
                var predicate = predicateSelector(entity);
                bool exists = await repository.AnyAsync(predicate);

                if (exists)
                {
                    string entityName = typeof(TEntity).Name;
                    string message = customMessage ?? $"Duplicate {entityName} found in batch.";
                    throw new BusinessException(message);
                }
            }
        }

        #endregion

    }
}