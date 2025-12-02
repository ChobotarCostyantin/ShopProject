using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.DAL.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;

namespace Catalog.BLL.Validators.Categories
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        private readonly CatalogDbContext _dbContext;
        public CreateCategoryRequestValidator(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than 100 characters")
                .MustBeUniqueAsync(_dbContext.Categories, x => x.Name)
                .WithMessage("Category with this name already exists");
        }
    }
}