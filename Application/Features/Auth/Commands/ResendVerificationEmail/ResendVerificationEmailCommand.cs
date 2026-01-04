using MediatR;

namespace Application.Features.Auth.Commands.ResendVerificationEmail
{
    public class ResendVerificationEmailCommand : IRequest<ResendVerificationEmailResponseDTO>
    {
        public string Email { get; set; }
    }
}
