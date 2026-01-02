namespace Application.Features.UserOperationClaims.Commands.AssignMultiple
{
    public class AssignMultipleOperationClaimsToUserResponseDTO
    {
        public int UserId { get; set; }
        public int AssignedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}