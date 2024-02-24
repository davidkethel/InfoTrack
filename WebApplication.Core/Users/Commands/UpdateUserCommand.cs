﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Core.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public string GivenNames { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        public class Validator : AbstractValidator<UpdateUserCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("'Id' must be greater than '0'.");

                RuleFor(x => x.GivenNames)
                    .NotEmpty();

                RuleFor(x => x.LastName)
                    .NotEmpty();

                RuleFor(x => x.EmailAddress)
                    .NotEmpty();

                RuleFor(x => x.MobileNumber)
                    .NotEmpty();

                // TODO: If you are feeling ambitious, also create a validation rule that ensures the user exists in the database.
            }
        }

        public class Handler : IRequestHandler<UpdateUserCommand, UserDto>
        {
            /// <inheritdoc />
            public Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException("Implement a way to update the user associated with the provided Id.");
            }
        }
    }
}
