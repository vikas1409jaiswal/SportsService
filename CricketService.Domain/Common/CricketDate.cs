using System.Globalization;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CricketService.Domain.Common
{
    public class CricketDate
    {
        public CricketDate(string infoString, InfoFormats infoFormats = InfoFormats.BirthInfo)
        {
            if (string.IsNullOrWhiteSpace(infoString))
            {
                Date = null;
            }
            else
            {
                if (infoFormats == InfoFormats.BirthInfo)
                {
                    var dateString = string.Join(", ", infoString.Split(", ").Take(2));
                    try
                    {
                        Date = DateTime.ParseExact(dateString, "MMMM dd, yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        Console.WriteLine($"{ dateString } is not valid date format");
                        Date = null;
                    }
                }
            }
        }

        public DateTime? Date { get; set; } = null;

        public override string ToString()
        {
           if (Date is null)
            {
                return string.Empty;
            }

           return Date?.ToString("MMM dd, yyyy")!;
        }
    }
}
