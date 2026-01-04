using Application.Repositories;
using Core.Application.Rules;
using Core.Security.Entities;
using Core.Security.HashingSalting;

namespace Application.Features.Auth.Rules
{
    public class AuthBusinessRules : BaseBusinessRules
    {
        private readonly IUserRepository _userRepository;
        public AuthBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> UserShouldExistWhenLogin(string email)
        {
            // Email format validation
            //CheckEmailFormat(email);

            User? user = await _userRepository.GetAsync(u => u.Email == email);
            CheckEntityExistsForBusiness(user, "Invalid email or password.");

            // Check if user is soft deleted
            CheckEntityNotDeleted(user!, "User account has been deleted.");

            return user!;
        }

        public void PasswordShouldBeCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            bool isPasswordMatch = HashingSaltingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
            CheckBusinessRule(isPasswordMatch, "Invalid email or password.");
        }

        public async Task UserShouldNotExistWhenRegister(string email)
        {
            // Email format validation
            CheckEmailFormat(email);

            // Use AnyAsync for better performance (doesn't load entire entity)
            await CheckPropertyIsUniqueWithAny<User, IUserRepository>(
                _userRepository,
                u => u.Email == email,
                "User already exists with this email."
            );
        }

        public async Task UsernameShouldBeUnique(string username)
        {
            // Username format validation
            CheckUsernameFormat(username);

            // Use AnyAsync for better performance
            await CheckPropertyIsUniqueWithAny<User, IUserRepository>(
                _userRepository,
                u => u.UserName == username,
                "Username is already taken."
            );
        }

        public void PasswordShouldMeetRequirements(string password)
        {
            CheckPasswordStrength(password);
        }

        public async Task EmailShouldBeUniqueForUpdate(string email, int userId)
        {
            CheckEmailFormat(email);

            await CheckPropertyIsUniqueExcept<User, IUserRepository>(
                _userRepository,
                u => u.Email == email,
                userId,
                "Email is already in use by another user."
            );
        }

        public async Task UsernameShouldBeUniqueForUpdate(string username, int userId)
        {
            CheckUsernameFormat(username);

            await CheckPropertyIsUniqueExcept<User, IUserRepository>(
                _userRepository,
                u => u.UserName == username,
                userId,
                "Username is already in use by another user."
            );
        }

        public void UserShouldNotBeDeleted(User user)
        {
            CheckEntityNotDeleted(user, "User account has been deleted.");
        }

        public void CheckEntityExists(User? user)
        {
            CheckEntityExists(user, "User not found.");
        }

        // Email Verification Rules
        public void EmailShouldBeVerified(bool isEmailVerified)
        {
            CheckBusinessRule(isEmailVerified, "Please verify your email address before logging in. Check your inbox for the verification link.");
        }

        public void VerificationTokenShouldBeValid(string token)
        {
            CheckBusinessRule(!string.IsNullOrWhiteSpace(token), "Invalid verification token.");
            CheckBusinessRule(token.Length >= 32, "Invalid verification token format.");
        }

        public void UserShouldExistForVerification(User? user)
        {
            CheckEntityExistsForBusiness(user, "Invalid or expired verification token.");
        }

        public void EmailVerificationTokenShouldNotBeExpired(DateTime? tokenExpires)
        {
            CheckBusinessRule(tokenExpires.HasValue, "Verification token has expired.");
            CheckBusinessRule(tokenExpires.Value > DateTime.UtcNow, "Verification token has expired. Please request a new one.");
        }

        public void EmailShouldNotBeAlreadyVerified(bool isEmailVerified)
        {
            CheckBusinessRule(!isEmailVerified, "Email is already verified.");
        }

        public async Task<User> UserShouldExistForResendVerification(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email == email);
            CheckEntityExistsForBusiness(user, "No account found with this email address.");
            CheckEntityNotDeleted(user!, "User account has been deleted.");
            return user!;
        }

        // Password Reset Rules
        public async Task<User> UserShouldExistWhenRequestingPasswordReset(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email == email);
            CheckEntityExistsForBusiness(user, "No account found with this email address.");
            CheckEntityNotDeleted(user!, "User account has been deleted.");
            return user!;
        }

        public void ResetTokenShouldBeValid(string token)
        {
            CheckBusinessRule(!string.IsNullOrWhiteSpace(token), "Invalid reset token.");
            CheckBusinessRule(token.Length >= 32, "Invalid reset token format.");
        }

        public void UserShouldExistForPasswordReset(User? user)
        {
            CheckEntityExistsForBusiness(user, "Invalid or expired reset token.");
        }

        public void PasswordResetTokenShouldNotBeExpired(DateTime? tokenExpires)
        {
            CheckBusinessRule(tokenExpires.HasValue, "Reset token has expired.");
            CheckBusinessRule(tokenExpires.Value > DateTime.UtcNow, "Reset token has expired. Please request a new one.");
        }

        public void PasswordsShouldMatch(string password, string confirmPassword)
        {
            CheckBusinessRule(password == confirmPassword, "Passwords do not match.");
        }
    }
}
