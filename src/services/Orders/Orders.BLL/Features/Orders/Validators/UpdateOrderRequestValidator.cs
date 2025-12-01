using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Shared.ValidationAttributes;

namespace Orders.BLL.Features.Orders.Validators
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.DeliveryDate)
                .NotEmpty().WithMessage("Delivery date is required")
                .NotInPast().WithMessage("Delivery date must be in the future");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required");
        }
    }
}