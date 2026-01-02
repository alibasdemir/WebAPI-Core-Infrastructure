using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Delete;
using Application.Features.Tests.Commands.SoftDelete;
using Application.Features.Tests.Commands.Update;
using Application.Features.Tests.Queries.GetById;
using Application.Features.Tests.Queries.GetList;
using Application.Features.Tests.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : BaseController
    {
        #region Commands

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTestCommand command)
        {
            CreateTestResponseDTO response = await _mediator.Send(command);
            return Created($"/api/Tests/{response.Id}", response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateTestCommand command)
        {
            UpdateTestResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteTestCommand command = new() { Id = id };
            DeleteTestResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("SoftDelete/{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDelete([FromRoute] int id)
        {
            SoftDeleteTestCommand command = new() { Id = id };
            SoftDeleteTestResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        #endregion

        #region Queries

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetByIdTestQuery query = new() { Id = id };
            GetByIdTestResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        // GET endpoint (simple list with default ordering)
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int index = 0, [FromQuery] int size = 10)
        {
            GetListTestQuery query = new() { Index = index, Size = size };
            GetListTestResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListWithDynamic([FromBody] GetListTestQuery query)
        {
            GetListTestResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchTestsQuery query)
        {
            SearchTestsResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        #endregion
    }
}
