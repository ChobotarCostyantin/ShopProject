using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;

namespace Orders.BLL.Features.OrderShipping.Validators
{
    public class UpdateOrderShippingRequestValidator : AbstractValidator<UpdateOrderShippingRequest>
    {
        public UpdateOrderShippingRequestValidator()
        {
            RuleFor(x => x.AddressLine)
                .NotEmpty().WithMessage("Address line is required")
                .MaximumLength(100).WithMessage("Address line cannot be longer than 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(50).WithMessage("City cannot be longer than 50 characters");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(10).WithMessage("Postal code cannot be longer than 10 characters");
        }
    }
}