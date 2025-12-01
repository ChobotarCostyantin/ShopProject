using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Orders.BLL.Features.OrderItems.DTOs.Requests;

namespace Orders.BLL.Features.OrderItems.Validators
{
    public class CreatOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreatOrderItemRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order id is required");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product id is required")
                .MustAsync(CheckIfProductExists).WithMessage("Product with provided id does not exist");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }

        private Task<bool> CheckIfProductExists(Guid productId, CancellationToken cancellationToken)
        {
            // TODO: Microservice communication to check if product with provided id exists
            return Task.FromResult(true);
        }
    }
}