using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.Orders.DTOs.Requests;

namespace Orders.BLL.Features.Orders.Validators
{
    public class DeleteOrderRequestValidator : AbstractValidator<DeleteOrderRequest>
    {
        public DeleteOrderRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order id is required");
        }
    }
}