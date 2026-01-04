using MediatR;

namespace Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordResponseDTO>
    {
        public string Email { get; set; }
    }
}
