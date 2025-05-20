using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Domain.RequestDomains;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CricketService.Data.Utils
{
    public class PDFHandler
    {
        private readonly IMapper mapper;
        private readonly ILogger<PDFHandler> logger;

        public PDFHandler(IMapper mapper, ILogger<PDFHandler> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public PDFHandler(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public static void WritePdfFilex()
        {
            StreamReader r = new StreamReader("D:\\MyYoutubeRepos\\repo\\CricketService\\CricketService.Data\\StaticData\\Data_BackUp\\T20I_matches.json");

            var matchesData = JsonConvert.DeserializeObject<List<InternationalCricketMatchRequest>>(r.ReadToEnd())!
                .OrderBy(m => Convert.ToInt32(m.MatchNumber.Replace("T20I no. ", string.Empty))).ToList();

            // Create a new PDF doc
            Document document = new Document();

            document.AddAuthor("Vikas Jaiswal");
            document.AddCreator("Sample application using iTextSharp");
            document.AddKeywords("PDF tutorial education");
            document.AddSubject("International");
            document.AddTitle("T20 International Matches");

            // Create a new PDF writer
            PdfWriter.GetInstance(document, new FileStream("D:/CricketData/T20IMatches.pdf", FileMode.Create));

            // Open the doc
            document.Open();

            // Create a new table with three columns
            PdfPTable table = new PdfPTable(5);

            // Set the width of the table cells
            table.WidthPercentage = 100;

            // Set the background color and border color of the table cells
            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = new BaseColor(255, 221, 221); // light red
            cell.BorderColor = new BaseColor(255, 0, 0); // red

            // Add column headers to the table
            cell.Phrase = new Phrase("SN", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.WHITE));
            table.AddCell(cell);

            cell.Phrase = new Phrase("Title", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.WHITE));
            table.AddCell(cell);

            cell.Phrase = new Phrase("Result", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.WHITE));
            table.AddCell(cell);

            cell.Phrase = new Phrase("Date", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.WHITE));
            table.AddCell(cell);

            cell.Phrase = new Phrase("Venue", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.WHITE));
            table.AddCell(cell);

            // Add data rows to the table
            foreach (var match in matchesData)
            {
                cell = new PdfPCell(new Phrase(Convert.ToInt32(match.MatchNumber.Replace("T20I no. ", string.Empty)).ToString(), new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));
                cell.BackgroundColor = new BaseColor(221, 255, 221); // light green
                cell.BorderColor = new BaseColor(0, 128, 0); // green
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(match.MatchTitle, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));
                cell.BackgroundColor = new BaseColor(221, 255, 221); // light green
                cell.BorderColor = new BaseColor(0, 128, 0); // green
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(match.Result, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));
                cell.BackgroundColor = new BaseColor(221, 255, 221); // light green
                cell.BorderColor = new BaseColor(0, 128, 0); // green
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(match.MatchDate, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));
                cell.BackgroundColor = new BaseColor(221, 255, 221); // light green
                cell.BorderColor = new BaseColor(0, 128, 0); // green
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(match.Venue, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)));
                cell.BackgroundColor = new BaseColor(221, 255, 221); // light green
                cell.BorderColor = new BaseColor(0, 128, 0); // green
                table.AddCell(cell);
            }

            /// Download the image from the URL
            WebRequest request = WebRequest.Create("https://upload.wikimedia.org/wikipedia/en/thumb/4/41/Flag_of_India.svg/188px-Flag_of_India.svg.png");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            // Load the image from the stream
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);

            // Set the position and size of the image on the page
            img.SetAbsolutePosition(100, 100);
            img.ScaleToFit(400, 400);

            // Add the image to the doc
            document.Add(img);

            // Add the table to the doc
            document.Add(table);

            // Close the doc
            document.Close();
        }

        public static void WritePdfFile()
        {
            StreamReader r = new StreamReader("D:\\MyYoutubeRepos\\repo\\CricketService\\CricketService.Data\\StaticData\\Data_BackUp\\T20I_matches.json");

            var matchData = JsonConvert.DeserializeObject<List<InternationalCricketMatchRequest>>(r.ReadToEnd())!
                .OrderBy(m => Convert.ToInt32(m.MatchNumber.Replace("T20I no. ", string.Empty))).ToList().Single(x => x.MatchNumber.Contains("1991"));

            // Create a new PDF doc
            Document document = new Document();

            // Create a new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("D:/CricketData/MatchNo1991.pdf", FileMode.Create));

            // Open the doc
            document.Open();

            //HeaderEventHandler header = new HeaderEventHandler(matchResponse.MatchTitle);

            //writer.PageEvent = header;

            AddMatchHeader(document, matchData.MatchTitle);

            //document.AddMatchDetails(matchResponse);

            // Close the doc
            document.Close();
        }

        private static void AddMatchHeader(Document document, string matchTitle)
        {
            WebRequest request = WebRequest.Create("https://tse4.mm.bing.net/th?id=OIP.0ubwvFWDjDkiJ0oCOszk5gHaHX&pid=Api&P=0");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(stream), ImageFormat.Jpeg);
            stream.Close();

            var leftImage = image;
            var rightImage = image;

            var table = new PdfPTable(10)
            {
                WidthPercentage = 95,
            };

            leftImage.ScaleAbsolute(20f, 20f);
            table.AddCell(leftImage);

            table.AddCell(new PdfPCell(new Phrase(matchTitle, new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD, BaseColor.BLUE)))
            {
                BackgroundColor = new BaseColor(System.Drawing.Color.Honeydew),
                BorderColor = BaseColor.BLACK,
                PaddingTop = 20,
                PaddingBottom = 20,
                HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE,
                Colspan = 8,
                BorderWidthBottom = 2f,
                BorderWidthTop = 2f,
                BorderWidthLeft = 2f,
                BorderWidthRight = 2f,
            });

            rightImage.ScaleAbsolute(20f, 20f);
            table.AddCell(rightImage);

            document.Add(table);
        }

        //public static void AddScorboardHeader(this Document document, PdfWriter writer, string headerText)
        //{
        //    PdfPTable header = new PdfPTable(1);

        //    var doc = new HtmlDocument();

        //    var h3 = doc.CreateElement("h3");
        //    h3.InnerHtml = headerText;
        //    h3.Attributes.Add("style", "text-align: center; padding: 10px; margin: 20px; background-color: #eee; border: 2px solid black; border-radius: 10px;");

        //    doc.DocumentNode.AppendChild(h3);

        //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(doc.DocumentNode.OuterHtml));
        //}
    }
}
