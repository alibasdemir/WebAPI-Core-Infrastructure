using Core.Security.JWT;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginResponseDTO
    {
        public AccessToken AccessToken { get; set; }
    }
}
