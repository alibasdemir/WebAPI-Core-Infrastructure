namespace Application.Features.Tests.Queries.Search
{
    public class SearchTestsResponseDTO
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public IList<SearchTestItemDTO> Items { get; set; }
    }

    public class SearchTestItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}