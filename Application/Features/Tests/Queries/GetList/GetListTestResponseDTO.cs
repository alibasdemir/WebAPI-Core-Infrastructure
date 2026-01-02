using Core.Pagination.Responses;

namespace Application.Features.Tests.Queries.GetList
{
    public class GetListTestResponseDTO : GetListResponse<TestItemDTO>
    {
    }

    public class TestItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}