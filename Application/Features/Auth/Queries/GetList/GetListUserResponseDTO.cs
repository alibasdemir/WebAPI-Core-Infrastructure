using Core.Pagination.Responses;

namespace Application.Features.Auth.Queries.GetListUser
{
    public class GetListUserResponseDTO : GetListResponse<UserItemDTO>
    {
    }

    public class UserItemDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}