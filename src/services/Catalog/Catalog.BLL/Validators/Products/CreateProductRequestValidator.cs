using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.DAL.Database;
using FluentValidation;
using Shared.Extensions;

namespace Catalog.BLL.Validators.Products
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly CatalogDbContext _dbContext;
        public CreateProductRequestValidator(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must be less than 200 characters");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("Sku is required")
                .MaximumLength(50).WithMessage("Sku must be less than 50 characters")
                .MustBeUniqueAsync(_dbContext.Products, x => x.Sku)
                .WithMessage("Product with this sku already exists");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.StockQuantity)
                .NotEmpty().WithMessage("Stock quantity is required")
                .GreaterThan(0).WithMessage("Stock quantity must be greater than 0");
        }
    }
}