using Core.Pagination;
using Core.Pagination.Responses;

namespace Application.Features.Auth.Queries.SearchUsers
{
    public class SearchUsersResponseDTO : GetListResponse<SearchUserItemDTO>
    {
    }

    public class SearchUserItemDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}