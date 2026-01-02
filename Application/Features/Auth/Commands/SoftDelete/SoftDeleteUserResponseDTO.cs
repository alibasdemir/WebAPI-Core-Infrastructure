namespace Application.Features.Auth.Commands.SoftDeleteUser
{
    public class SoftDeleteUserResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User soft deleted successfully.";
        public DateTime DeletedDate { get; set; }
    }
}