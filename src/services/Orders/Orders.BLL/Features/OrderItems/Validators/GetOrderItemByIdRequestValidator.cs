using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderItems.DTOs.Requests;

namespace Orders.BLL.Features.OrderItems.Validators
{
    public class GetOrderItemByIdRequestValidator : AbstractValidator<GetOrderItemByIdRequest>
    {
        public GetOrderItemByIdRequestValidator()
        {
            RuleFor(x => x.OrderItemId)
                .NotEmpty().WithMessage("Order item id is required");
        }
    }
}