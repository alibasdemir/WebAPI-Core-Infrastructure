namespace Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}