namespace Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = "Password changed successfully.";
        public DateTime UpdatedDate { get; set; }
    }
}