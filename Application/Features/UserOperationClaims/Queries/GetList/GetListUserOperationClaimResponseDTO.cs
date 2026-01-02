namespace Application.Features.UserOperationClaims.Queries.GetList
{
    public class GetListUserOperationClaimResponseDTO
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public IList<UserOperationClaimListItemDTO> Items { get; set; }
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