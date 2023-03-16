using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CricketService.Domain.Attributes.ValidationAttributes
{
    public class CricketMatchNumberAttribute : ValidationAttribute
    {
        private readonly string pattern = @"^(ODI|T20I|Test) no\. [1-9]\d{0,3}$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string inputString = value.ToString();

            if (Regex.IsMatch(inputString, pattern))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("The cricket match number must be in the format 'ODI no. xxxx' or 'T20I no. xxxx' or 'Test no. xxxx'.");
            }
        }
    }
}
