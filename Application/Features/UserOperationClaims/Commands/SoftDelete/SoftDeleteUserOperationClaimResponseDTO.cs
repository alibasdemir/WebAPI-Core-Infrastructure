namespace Application.Features.UserOperationClaims.Commands.SoftDelete
{
    public class SoftDeleteUserOperationClaimResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User operation claim soft deleted successfully.";
        public DateTime DeletedDate { get; set; }
    }
}