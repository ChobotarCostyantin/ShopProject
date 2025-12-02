using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Products.Requests;
using FluentValidation;

namespace Catalog.BLL.Validators.Products
{
    public class RemoveTagFromProductRequestValidator : AbstractValidator<RemoveTagFromProductRequest>
    {
        public RemoveTagFromProductRequestValidator()
        {
            RuleFor(x => x.TagId)
                .NotEmpty().WithMessage("Tag id is required");
        }
    }
}