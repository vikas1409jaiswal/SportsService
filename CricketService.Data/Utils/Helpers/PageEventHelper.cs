using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CricketService.Data.Utils.Helpers
{
    public class PageEventHelper : PdfPageEventHelper
    {
        private Font pageNumberFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            Phrase pageNumberPhrase = new Phrase(string.Format("Page {0} of {1}", writer.PageNumber, 1), pageNumberFont);

            PdfContentByte canvas = writer.DirectContent;
            float x = (document.Right + document.Left) / 2;
            float y = document.Bottom - 20;
            canvas.MoveTo(x, y);

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, pageNumberPhrase, x, y, 0);

            float x2 = document.Left + 20;
            float y2 = document.Top - 10;
            canvas.MoveTo(x2, y2);

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, new Phrase("Owned By: Vikas Jaiswal"), x2, y2, 0);

            float x3 = document.Right - 20;
            float y3 = document.Top - 10;
            canvas.MoveTo(x3, y3);

            ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, new Phrase("Cricket Data"), x3, y3, 0);
        }
    }
}
