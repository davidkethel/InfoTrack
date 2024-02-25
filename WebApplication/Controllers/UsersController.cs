using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Core.Common.CustomProblemDetails;
using WebApplication.Core.Common.Models;
using WebApplication.Core.Users.Commands;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Core.Users.Queries;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser(
            [FromQuery] GetUserQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [Route("Find")]
        public async Task<IActionResult> FindUser([FromQuery] FindUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedDto<List<UserDto>>), StatusCodes.Status200OK)]
        [Route("List")]
        public async Task<IActionResult> ListUsers([FromQuery] ListUsersQuery query
            , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            var routeValues = new { id = result.UserId };
            return CreatedAtRoute(routeValues, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command
            , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);

        }

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
    }
}
