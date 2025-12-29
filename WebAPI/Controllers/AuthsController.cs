using Application.Features.Auth.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RegisterCommand registerCommand)
        {
            RegisterResponseDTO registerResponse = await _mediator.Send(registerCommand);
            return Ok(registerResponse);
        }
    }
}
