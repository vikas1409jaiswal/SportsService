using System.ComponentModel.DataAnnotations;

namespace CricketService.Domain.Attributes.ValidationAttributes
{
    public class CricketMatchTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Match title cannot be null.");
            }

            string matchTitle = value.ToString()!;

            if (string.IsNullOrWhiteSpace(matchTitle))
            {
                return new ValidationResult("Match title cannot be empty or whitespace.");
            }

            if (!matchTitle.Contains(" vs "))
            {
                return new ValidationResult("Match title must contain ' vs ' to separate teams.");
            }

            string[] splitByVs = matchTitle.Split(new[] { " vs " }, StringSplitOptions.None);

            if (splitByVs.Length != 2)
            {
                return new ValidationResult("Match title must contain exactly two teams separated by ' vs '.");
            }

            foreach (string part in splitByVs)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    return new ValidationResult("Team names cannot be empty or whitespace.");
                }

                if (part.StartsWith(" ") || part.EndsWith(" "))
                {
                    return new ValidationResult("Team names must not start or end with whitespace.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}