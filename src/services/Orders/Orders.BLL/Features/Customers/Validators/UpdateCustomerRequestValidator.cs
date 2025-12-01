using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.Customers.DTOs.Requests;

namespace Orders.BLL.Features.Customers.Validators
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MinimumLength(3).WithMessage("Full name must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Full name must be at most 50 characters long");
        }
    }
}