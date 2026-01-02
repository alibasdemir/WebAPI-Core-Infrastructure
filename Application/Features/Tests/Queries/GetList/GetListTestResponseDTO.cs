namespace Application.Features.Tests.Queries.GetList
{
    public class GetListTestResponseDTO
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public IList<TestItemDTO> Items { get; set; }
    }

    public class TestItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}