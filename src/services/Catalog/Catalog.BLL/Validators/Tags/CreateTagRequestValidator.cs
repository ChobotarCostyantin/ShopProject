using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Tags.Requests;
using Catalog.DAL.Database;
using FluentValidation;
using OpenTelemetry;
using Shared.Extensions;

namespace Catalog.BLL.Validators.Tags
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        private readonly CatalogDbContext _dbContext;
        public CreateTagRequestValidator(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must be less than 50 characters")
                .MustBeUniqueAsync(_dbContext.Tags, x => x.Name)
                .WithMessage("Tag with this name already exists");
        }
    }
}