namespace Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Roles { get; set; }
    }
}