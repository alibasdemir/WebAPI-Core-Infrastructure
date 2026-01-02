using Core.Pagination.Responses;

namespace Application.Features.UserOperationClaims.Queries.GetList
{
    public class GetListUserOperationClaimResponseDTO : GetListResponse<UserOperationClaimListItemDTO>
    {
    }

    public class UserOperationClaimListItemDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int OperationClaimId { get; set; }
        public string OperationClaimName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}