using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Util.ValidationAttributes
{
    public class ValidateDataRetroativaAttribute : ValidationAttribute
    {
        public string InvalidMessage { get; private set; }

        public ValidateDataRetroativaAttribute(string message)
        {
            this.InvalidMessage = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            
            DateTime dataValidar = Convert.ToDateTime(value);
            
            if(dataValidar >= DateTime.Now.Date || dataValidar == DateTime.MinValue)
                return ValidationResult.Success;

            return new ValidationResult(this.FormatErrorMessage(this.InvalidMessage));
        }
    }
}