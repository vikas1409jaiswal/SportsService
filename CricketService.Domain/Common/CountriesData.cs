namespace CricketService.Domain.Common
{
    public class CountriesData
    {
        public CountriesData(Name name, Flags flags)
        {
            Name = name;
            Flags = flags;
        }

        public Name Name { get; set; }

        public Flags Flags { get; set; }
    }

    public class Name
    {
        public Name(string common)
        {
            Common = common;
        }

        public string Common { get; set; } = string.Empty;
    }

    public class Flags
    {
        public Flags(string svg, string png)
        {
            Svg = svg;
            Png = png;
        }

        public string Svg { get; set; } = string.Empty;

        public string Png { get; set; } = string.Empty;
    }
}
