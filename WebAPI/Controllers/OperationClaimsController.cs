using Application.Features.OperationClaims.Commands.Create;
using Application.Features.OperationClaims.Commands.Delete;
using Application.Features.OperationClaims.Commands.Update;
using Application.Features.OperationClaims.Queries.GetById;
using Application.Features.OperationClaims.Queries.GetList;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Pagination.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = GeneralOperationClaims.Admin)]
    public class OperationClaimsController : BaseController
    {
        #region Commands

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOperationClaimCommand command)
        {
            CreateOperationClaimResponseDTO response = await _mediator.Send(command);
            return Created($"/api/OperationClaims/{response.Id}", response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand command)
        {
            UpdateOperationClaimResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteOperationClaimCommand command = new() { Id = id };
            DeleteOperationClaimResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        #endregion

        #region Queries

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            GetByIdOperationClaimQuery query = new() { Id = id };
            GetByIdOperationClaimResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListOperationClaimQuery query = new() { PageRequest = pageRequest };
            GetListOperationClaimResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListWithDynamic([FromBody] GetListOperationClaimQuery query)
        {
            GetListOperationClaimResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        #endregion
    }
}