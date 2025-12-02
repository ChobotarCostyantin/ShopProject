using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.ProductDetails.Requests;
using FluentValidation;

namespace Catalog.BLL.Validators.ProductDetails
{
    public class CreateProductDetailsRequestValidator : AbstractValidator<CreateProductDetailsRequest>
    {
        public CreateProductDetailsRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product id is required");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters");

            RuleFor(x => x.Manufacturer)
                .MaximumLength(100).WithMessage("Manufacturer must be less than 100 characters");

            RuleFor(x => x.Weight_Kg)
                .NotEmpty().WithMessage("Weight is required")
                .GreaterThan(0).WithMessage("Weight must be greater than 0");
        }
    }
}