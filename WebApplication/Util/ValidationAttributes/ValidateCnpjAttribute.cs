using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApplication.Util.ValidationAttributes
{
    public class ValidateCnpjAttribute : ValidationAttribute
    {
        public string InvalidMessage { get; private set; }

        public ValidateCnpjAttribute(string message)
        {
            this.InvalidMessage = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            bool isCnpjValid = Helpers.IsValidCnpj(value.ToString());

            if (isCnpjValid)
                return ValidationResult.Success;

            return new ValidationResult(this.FormatErrorMessage(this.InvalidMessage));
        }
    }
}