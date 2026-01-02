namespace Application.Features.UserOperationClaims.Commands.Assign
{
    public class AssignOperationClaimToUserResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; } = "Operation claim assigned to user successfully.";
    }
}