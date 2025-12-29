using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]         
    public class AuthsController : BaseController
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Add([FromBody] RegisterCommand registerCommand)
        {
            RegisterResponseDTO registerResponse = await _mediator.Send(registerCommand);
            return Ok(registerResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            var loginResponse = await _mediator.Send(loginCommand);
            return Ok(loginResponse);
        }
    }
}
