using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;

namespace Orders.BLL.Features.OrderShipping.Validators
{
    public class GetOrderShippingByIdRequestValidator : AbstractValidator<GetOrderShippingByIdRequest>
    {
        public GetOrderShippingByIdRequestValidator()
        {
            RuleFor(x => x.ShippingId)
                .NotEmpty().WithMessage("Shipping id is required");
        }
    }
}