using MediatR;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordResponseDTO>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
