using Core.Pagination.Responses;

namespace Application.Features.OperationClaims.Queries.GetList
{
    public class GetListOperationClaimResponseDTO : GetListResponse<OperationClaimItemDTO>
    {
    }

    public class OperationClaimItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}