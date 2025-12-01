using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Shared.ValidationAttributes;

namespace Orders.BLL.Features.Orders.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer id is required");

            RuleFor(x => x.DeliveryDate)
                .NotEmpty().WithMessage("Delivery date is required")
                .NotInPast().WithMessage("Delivery date must be in the future");
        }
    }
}