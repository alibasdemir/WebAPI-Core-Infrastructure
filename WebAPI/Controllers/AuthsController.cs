using Application.Features.Auth.Commands.ChangePassword;
using Application.Features.Auth.Commands.DeleteUser;
using Application.Features.Auth.Commands.ForgotPassword;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.ResendVerificationEmail;
using Application.Features.Auth.Commands.ResetPassword;
using Application.Features.Auth.Commands.SoftDeleteUser;
using Application.Features.Auth.Commands.UpdateUser;
using Application.Features.Auth.Commands.VerifyEmail;
using Application.Features.Auth.Queries.GetById;
using Application.Features.Auth.Queries.GetByIdUser;
using Application.Features.Auth.Queries.GetCurrentUser;
using Application.Features.Auth.Queries.GetListUser;
using Application.Features.Auth.Queries.SearchUsers;
using Core.Application.Pipelines.Authorization.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]         
    public class AuthsController : BaseController
    {
        #region Commands

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

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            VerifyEmailResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("ResendVerificationEmail")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailCommand command)
        {
            ResendVerificationEmailResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            ForgotPasswordResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            ResetPasswordResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            UpdateUserResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            DeleteUserCommand command = new() { Id = id };
            DeleteUserResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("SoftDelete/{id}")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> SoftDeleteUser([FromRoute] int id)
        {
            SoftDeleteUserCommand command = new() { Id = id };
            SoftDeleteUserResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            ChangePasswordResponseDTO response = await _mediator.Send(command);
            return Ok(response);
        }

        #endregion

        #region Queries

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> GetByIdUser([FromRoute] int id)
        {
            GetByIdUserQuery query = new() { Id = id };
            GetByIdUserResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetList")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> GetListUser([FromQuery] GetListUserQuery query)
        {
            GetListUserResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            GetCurrentUserQuery query = new();
            GetCurrentUserResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("Search")]
        [Authorize(Roles = GeneralOperationClaims.Admin)]
        public async Task<IActionResult> SearchUsers([FromQuery] SearchUsersQuery query)
        {
            SearchUsersResponseDTO response = await _mediator.Send(query);
            return Ok(response);
        }

        #endregion
    }
}
