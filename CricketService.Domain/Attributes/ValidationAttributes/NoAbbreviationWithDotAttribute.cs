using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CricketService.Domain.Attributes.ValidationAttributes
{
    public class NoAbbreviationWithDotAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Field cannot be null.");
            }

            string input = value.ToString()!;

            if (Regex.IsMatch(input, @"\b([A-Z]\.){2,}"))
            {
                return new ValidationResult($"Abbreviations with dots '{value}' are not allowed.");
            }

            return ValidationResult.Success!;
        }
    }
}
