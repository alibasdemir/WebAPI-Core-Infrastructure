using Application.Features.Tests.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTestCommand command)
        {
            CreateTestResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
