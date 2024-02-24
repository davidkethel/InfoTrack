using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Core.Users.Queries;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GetUserQuery> _getUserQueryValidator;
        private readonly IValidator<FindUsersQuery> _findUsersQueryValidator;

        public UsersController(IMediator mediator, IValidator<GetUserQuery> getUserQueryValidator, IValidator<FindUsersQuery> findUsersQueryValidator)
        {
            _mediator = mediator;
            _getUserQueryValidator = getUserQueryValidator;
            _findUsersQueryValidator = findUsersQueryValidator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(
            [FromQuery] GetUserQuery query,
            CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [Route("Find")]
        public async  Task<IActionResult> FindUserAsync([FromQuery] FindUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route (at /List) that can retrieve a paginated list of users using the `ListUsersQuery`

        // TODO: create a route that can create a user using the `CreateUserCommand`

        // TODO: create a route that can update an existing user using the `UpdateUserCommand`

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
    }
}
