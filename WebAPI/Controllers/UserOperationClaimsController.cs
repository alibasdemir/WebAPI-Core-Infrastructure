using Application.Features.UserOperationClaims.Commands.Assign;
using Application.Features.UserOperationClaims.Commands.AssignMultiple;
using Application.Features.UserOperationClaims.Commands.Revoke;
using Application.Features.UserOperationClaims.Commands.SoftDelete;
using Application.Features.UserOperationClaims.Queries.GetByUserId;
using Application.Features.UserOperationClaims.Queries.GetList;
using Application.Features.UserOperationClaims.Queries.GetUsersByOperationClaimId;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Pagination.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = GeneralOperationClaims.Admin)]
    public class UserOperationClaimsController : BaseController
    {
        #region Commands

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignOperationClaimToUser([FromBody] AssignOperationClaimToUserCommand command)
        {
            AssignOperationClaimToUserResponseDTO response = await _mediator.Send(command);
            return Created($"/api/UserOperationClaims/{response.Id}", response);
        }

        [HttpDelete("Revoke/{id}")]
        public async Task<IActionResult> RevokeOperationClaimFromUser([FromRoute] int id)
        {
            RevokeOperationClaimFromUserCommand command = new() { Id = id };
            RevokeOperationClaimFromUserResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete([FromRoute] int id)
        {
            SoftDeleteUserOperationClaimCommand command = new() { Id = id };
            SoftDeleteUserOperationClaimResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("AssignMultiple")]
        public async Task<IActionResult> AssignMultipleOperationClaimsToUser([FromBody] AssignMultipleOperationClaimsToUserCommand command)
        {
            AssignMultipleOperationClaimsToUserResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        #endregion

        #region Queries

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int userId)
        {
            GetUserOperationClaimsByUserIdQuery query = new() { UserId = userId };
            GetUserOperationClaimsByUserIdResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListUserOperationClaimQuery query = new() { PageRequest = pageRequest };
            GetListUserOperationClaimResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetListWithDynamic([FromBody] GetListUserOperationClaimQuery query)
        {
            GetListUserOperationClaimResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("Role/{operationClaimId}")]
        public async Task<IActionResult> GetUsersByOperationClaimId([FromRoute] int operationClaimId)
        {
            GetUsersByOperationClaimIdQuery query = new() { OperationClaimId = operationClaimId };
            GetUsersByOperationClaimIdResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        #endregion
    }
}