using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.Customers.DTOs.Requests;

namespace Orders.BLL.Features.Customers.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required")
                .MustAsync(CheckIfUserExists).WithMessage("User with provided id does not exist");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MinimumLength(3).WithMessage("Full name must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Full name must be at most 50 characters long");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is not valid (e.g. 2hj2K@example.com)");
        }

        private Task<bool> CheckIfUserExists(Guid userId, CancellationToken cancellationToken)
        {
            // TODO: Microservice communication to check if user with provided id exists
            return Task.FromResult(true);
        }
    }
}