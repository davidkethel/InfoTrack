﻿using System.Threading;
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
        private readonly IValidator<GetUserQuery> _validator;

        public UsersController(IMediator mediator, IValidator<GetUserQuery> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(
            [FromQuery] GetUserQuery query,
            CancellationToken cancellationToken)
        {
            var validationResponse = await _validator.ValidateAsync(query, cancellationToken);
            if(!validationResponse.IsValid) 
            {
                return BadRequest(validationResponse.ToString());
            }

            UserDto result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route (at /Find) that can retrieve a list of matching users using the `FindUsersQuery`

        // TODO: create a route (at /List) that can retrieve a paginated list of users using the `ListUsersQuery`

        // TODO: create a route that can create a user using the `CreateUserCommand`

        // TODO: create a route that can update an existing user using the `UpdateUserCommand`

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
    }
}
