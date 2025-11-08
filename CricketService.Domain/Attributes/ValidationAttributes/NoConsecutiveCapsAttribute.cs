using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CricketService.Domain.Attributes.ValidationAttributes
{
    public class NoConsecutiveCapsAttribute : ValidationAttribute
    {
        private readonly List<string> allowedStrings = new()
        {
            "Kings XI Punjab",
            "ICC World XI",
            "World-XI",
            "Asia XI",
            "Africa XI",
            "Asia XI vs Africa XI",
            "Africa XI vs Asia XI",
            "ICC World XI vs Asia XI",
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Field cannot be null");
            }

            string input = value.ToString()!;

            string inputWithoutDls = Regex.Replace(
                input,
                @"\bDLS\b",
                string.Empty,
                RegexOptions.IgnoreCase
            );

            string filteredString = string.Empty;

            foreach (string str in allowedStrings)
            {
                if (inputWithoutDls.Contains(str))
                {
                    filteredString = inputWithoutDls?.Replace(str, string.Empty);
                }
            }

            if (Regex.IsMatch(filteredString, "[A-Z]{2}"))
            {
                return new ValidationResult(
                    $"'{input}' contains consecutive uppercase letters (excluding 'DLS')."
                );
            }

            return ValidationResult.Success!;
        }
    }
}