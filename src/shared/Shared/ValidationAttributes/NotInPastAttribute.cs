using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotInPastAttribute : ValidationAttribute
    {
        public NotInPastAttribute() : base("The {0} property must be in the future") { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime dto)
                return ValidationResult.Success;

            return dto < DateTime.UtcNow
                ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                : ValidationResult.Success;
        }
    }
}