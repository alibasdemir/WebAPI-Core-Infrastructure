using Application.Features.Tests.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTestCommand command)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                CreateTestResponseDTO response = await _mediator.Send(command);
                return Ok(response);
            }

            return Unauthorized("Unauthorized");
        }
    }
}
