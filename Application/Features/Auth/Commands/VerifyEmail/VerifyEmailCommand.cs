using MediatR;

namespace Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<VerifyEmailResponseDTO>
    {
        public string Token { get; set; }
    }
}
