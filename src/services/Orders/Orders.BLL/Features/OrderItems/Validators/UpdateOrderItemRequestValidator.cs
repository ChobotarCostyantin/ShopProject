using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderItems.DTOs.Requests;

namespace Orders.BLL.Features.OrderItems.Validators
{
    public class UpdateOrderItemRequestValidator : AbstractValidator<UpdateOrderItemRequest>
    {
        public UpdateOrderItemRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}