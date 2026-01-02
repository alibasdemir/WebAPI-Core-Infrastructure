namespace Application.Features.UserOperationClaims.Commands.Revoke
{
    public class RevokeOperationClaimFromUserResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "Operation claim revoked from user successfully.";
    }
}