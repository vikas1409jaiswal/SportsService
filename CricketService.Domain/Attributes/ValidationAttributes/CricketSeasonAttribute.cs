using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CricketService.Domain.Attributes.ValidationAttributes
{
    public class CricketSeasonAttribute : ValidationAttribute
    {
        private readonly string pattern = @"^(19|20)\d{2}(/(\d{2}))?$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }

            string inputString = value.ToString()!;

            if (Regex.IsMatch(inputString, pattern))
            {
                return ValidationResult.Success!;
            }
            else
            {
                return new ValidationResult("Season is not in correct format xxxx or xxxx/xx");
            }
        }
    }
}
