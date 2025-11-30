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
            RuleFor(x => x.ProductId)
                .MustAsync(CheckIfProductExists)
                .WithMessage("Product with provided id does not exist");
        }

        private Task<bool> CheckIfProductExists(Guid productId, CancellationToken cancellationToken)
        {
            // TODO: Microservice communication to check if product with provided id exists
            return Task.FromResult(true);
        }
    }
}