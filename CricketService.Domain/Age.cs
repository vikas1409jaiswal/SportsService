namespace CricketService.Domain
{
    public class Age
    {
        public Age(int years, int days)
        {
            Years = years;
            Days = days;
        }

        public int Years { get; set; }

        public int Days { get; set; }

        public static Age CalculateAge(DateOfEvent dateOfBirth, DateTime atDate)
        {
            if (dateOfBirth == null)
            {
                return new Age(0, 0);
            }

            // Create DateTime from DateOfEvent
            var birthDate = new DateTime(
                dateOfBirth.Year,
                dateOfBirth.Month ?? 1,
                dateOfBirth.Date ?? 1);

            var years = atDate.Year - birthDate.Year;
            
            // Adjust if birthday hasn't occurred this year
            if (atDate < birthDate.AddYears(years))
            {
                years--;
            }

            // Calculate days since last birthday
            var lastBirthday = birthDate.AddYears(years);
            var days = (int)(atDate - lastBirthday).TotalDays;

            return new Age(years, days);
        }
    }
}