namespace Application.Features.UserOperationClaims.Queries.GetByUserId
{
    public class GetUserOperationClaimsByUserIdResponseDTO
    {
        public int UserId { get; set; }
        public IList<UserOperationClaimItemDTO> Items { get; set; }
    }

    public class UserOperationClaimItemDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int OperationClaimId { get; set; }
        public string OperationClaimName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}