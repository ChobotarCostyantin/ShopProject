using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Shared.ValidationAttributes
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, DateTime> NotInPast<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Must(date => date >= DateTime.UtcNow)
                .WithMessage("The date must be in the future");
        }
    }
}