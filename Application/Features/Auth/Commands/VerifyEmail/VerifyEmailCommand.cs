using Core.Application.Pipelines.Logging;
using MediatR;

namespace Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<VerifyEmailResponseDTO>, ILoggableRequest
    {
        public string Token { get; set; }
    }
}
