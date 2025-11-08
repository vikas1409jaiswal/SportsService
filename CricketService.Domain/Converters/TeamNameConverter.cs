using System.Text.RegularExpressions;

public static class TeamNameConverter
{
    private static readonly Dictionary<string, string> ReplacementMap =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "ARG Women", "Argentina Women" },
        { "AUS Women", "Australia Women" },
        { "AUT Women", "Austria Women" },
        { "BAN Women", "Bangladesh Women" },
        { "BHR Women", "Bahrain Women" },
        { "BHU Women", "Bhutan Women" },
        { "BOT Women", "Botswana Women" },
        { "BRA Women", "Brazil Women" },
        { "BUL Women", "Bulgaria Women" },
        { "CAM Women", "Cambodia Women" },
        { "CAN Women", "Canada Women" },
        { "Cayman Wmn", "Cayman Women" },
        { "CHN Women", "China Women" },
        { "CMR Women", "Cameroon Women" },
        { "COK Women", "Cook Islands Women" },
        { "CRC Women", "Costa Rica Women" },
        { "CRC-W", "Costa Rica Women" },
        { "CRO Women", "Croatia Women" },
        { "CZE Women", "Czech Republic Women" },
        { "DEN Women", "Denmark Women" },
        { "ENG Women", "England Women" },
        { "ESP Women", "Spain Women" },
        { "EST Women", "Estonia Women" },
        { "FIJ Women", "Fiji Women" },
        { "FRA Women", "France Women" },
        { "GER Women", "Germany Women" },
        { "GHA Women", "Ghana Women" },
        { "GIB Women", "Gibraltar Women" },
        { "GRE Women", "Greece Women" },
        { "GUE Women", "Guernsey Women" },
        { "HKG Women", "Hong Kong Women" },
        { "IND Women", "India Women" },
        { "IDN Women", "Indonesia Women" },
        { "IOM Women", "Isle of Man Women" },
        { "IRE Women", "Ireland Women" },
        { "ITA Women", "Italy Women" },
        { "JEY Women", "Jersey Women" },
        { "JPN Women", "Japan Women" },
        { "KEN Women", "Kenya Women" },
        { "KUW Women", "Kuwait Women" },
        { "LES Women", "Lesotho Women" },
        { "LUX Women", "Luxembourg Women" },
        { "Mas Women", "Malaysia Women" },
        { "MEX Women", "Mexico Women" },
        { "MLT Women", "Malta Women" },
        { "MNG Women", "Mongolia Women" },
        { "MOZ Women", "Mozambique Women" },
        { "MWI Womwn", "Malawi Women" },
        { "MYA Women", "Myanmar Women" },
        { "NAM Women", "Namibia Women" },
        { "NEP Women", "Nepal Women" },
        { "NGA Women", "Nigeria Women" },
        { "NOR Women", "Norway Women" },
        { "NL Women", "Netherlands Women" },
        { "NZ Women", "New Zealand Women" },
        { "OMA Women", "Oman Women" },
        { "PAK Women", "Pakistan Women" },
        { "PHI Women", "Philippines Women" },
        { "PNG Women", "Papua New Guinea Women" },
        { "POR Women", "Portugal Women" },
        { "QAT Women", "Qatar Women" },
        { "ROU Women", "Romania Women" },
        { "RWA Women", "Rawanda Women" },
        { "SA Women", "South Africa Women" },
        { "SAM Women", "Samoa Women" },
        { "SCO Women", "Scotland Women" },
        { "SGP Women", "Singapore Women" },
        { "SK Women", "South Korea Women" },
        { "SL Women", "Sri Lanka Women" },
        { "SLE Women", "Sierra Leone Women" },
        { "SRB Women", "Serbia Women" },
        { "SWE Women", "Sweden Women" },
        { "TAN Women", "Tanzania Women" },
        { "THA Women", "Thailand Women" },
        { "UAE Women", "United Arab Emirates Women" },
        { "UGA Women", "Uganda Women" },
        { "USA Women", "United States of America Women" },
        { "VAN Women", "Vanuatu Women" },
        { "WI Women", "West Indies Women" },
        { "ZIM Women", "Zimbabwe Women" },
        { "Czech Rep.", "Czech Republic" },
        { "P.N.G.", "Papua New Guinea" },
        { "U.A.E.", "United Arab Emirates" },
        { "U.S.A.", "United States of America" },
        { "KKR", "Kolkata Knight Riders" },
        { "CSK", "Chennai Super Kings" },
        { "RR", "Rajasthan Royals" },
        { "MI", "Mumbai Indians" },
        { "PBKS", "Punjab Kings" },
        { "GT", "Gujarat Titans" },
        { "Guj Lions", "Gujarat Lions" },
        { "SRH", "Sunrisers Hyderabad" },
        { "DC", "Delhi Capitals" },
        { "LSG", "Lucknow Super Giants" },
        { "Pune Warriors", "Pune Warriors India" },
        { "RCB", "Royal Challengers Bangalore" },
    };

    public static string Replace(string input, string? season = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        if (new string[] { "2024", "2025" }.Contains(season))
        {
            ReplacementMap["RCB"] = "Royal Challengers Bengaluru";
        }
        else
        {
            ReplacementMap["RCB"] = "Royal Challengers Bangalore";
        }

        foreach (var pair in ReplacementMap)
        {
            input = Regex.Replace(
                input,
                $@"(^|\W){Regex.Escape(pair.Key)}($|\W)",
                $"$1{pair.Value}$2",
                RegexOptions.IgnoreCase
            );
        }

        return input.TrimStart().TrimEnd();
    }
}