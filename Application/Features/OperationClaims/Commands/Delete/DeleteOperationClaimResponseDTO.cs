namespace Application.Features.OperationClaims.Commands.Delete
{
    public class DeleteOperationClaimResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "Operation claim deleted successfully.";
    }
}