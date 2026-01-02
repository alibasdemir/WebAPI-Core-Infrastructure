namespace Application.Features.Auth.Commands.DeleteUser
{
    public class DeleteUserResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User deleted successfully.";
    }
}