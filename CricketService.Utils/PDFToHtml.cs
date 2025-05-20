using SautinSoft;

namespace CricketService.Utils
{
    public class PDFToHtml
    {
        //public static async Task Main(string[] args)
        //{
        //    string filePath = @"D:\CricketData\IPLMatches\IPL_2023_MatchNumber-18_Punjab Kings vs Gujarat Titans.pdf";
        //    ConvertPdfToHtml(filePath);
        //}

        public static void ConvertPdfToHtml(string inputFilePath)
        {
            // Create a new output file stream
            FileStream outputFileStream = new FileStream(@"D:\CricketData\IPLMatches\IPL_2023_MatchNumber-18_Punjab Kings vs Gujarat Titans.html", FileMode.Create);

            // Create an instance of the PdfFocus class
            PdfFocus converter = new PdfFocus()
            {
                HtmlOptions = new PdfFocus.CHtmlOptions
                {
                  IncludeImageInHtml = true,
                  Title = "Converted PDF Document",
                },
                Password = "Vikas",
            };

            // Load the PDF document
            converter.OpenPdf(inputFilePath);

            // Convert the PDF document to HTML format
            if (converter.PageCount > 0)
            {
                // Get the HTML output as a string
                string htmlOutput = converter.ToHtml();

                // Write the HTML output to the output file stream
                byte[] htmlBytes = System.Text.Encoding.UTF8.GetBytes(htmlOutput);
                outputFileStream.Write(htmlBytes, 0, htmlBytes.Length);
            }

            // Clean up resources
            outputFileStream.Close();
            converter.ClosePdf();
        }
    }
}
