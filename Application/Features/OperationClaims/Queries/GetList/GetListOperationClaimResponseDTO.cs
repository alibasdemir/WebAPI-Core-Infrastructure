namespace Application.Features.OperationClaims.Queries.GetList
{
    public class GetListOperationClaimResponseDTO
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public IList<OperationClaimItemDTO> Items { get; set; }
    }

    public class OperationClaimItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}