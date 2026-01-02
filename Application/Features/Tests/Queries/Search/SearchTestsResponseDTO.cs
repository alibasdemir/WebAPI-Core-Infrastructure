using Core.Pagination.Responses;

namespace Application.Features.Tests.Queries.Search
{
    public class SearchTestsResponseDTO : GetListResponse<SearchTestItemDTO>
    {
    }

    public class SearchTestItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}