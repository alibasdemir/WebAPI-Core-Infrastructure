namespace Core.Security.UserTokenGeneration
{
    public class UserTokenExpirationOptions
    {
        public int EmailVerificationTokenExpirationHours { get; set; } = 24;
        public int PasswordResetTokenExpirationHours { get; set; } = 1;
    }
}
