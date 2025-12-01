using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;

namespace Orders.BLL.Features.OrderShipping.Validators
{
    public class GetOrderShippingByOrderIdRequestValidator : AbstractValidator<GetOrderShippingByOrderIdRequest>
    {
        public GetOrderShippingByOrderIdRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order id is required");
        }
    }
}