using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApplication.Util.ValidationAttributes
{
    public class ValidateCpfAttribute : ValidationAttribute
    {
        public string InvalidMessage { get; private set; }

        public ValidateCpfAttribute(string message)
        {
            this.InvalidMessage = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            bool isCpfValid = Helpers.IsValidCpf(value.ToString());

            if (isCpfValid)
                return ValidationResult.Success;

            return new ValidationResult(this.FormatErrorMessage(this.InvalidMessage));
        }
    }
}