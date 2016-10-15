using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Api.Common.Utils
{
    public class PdfReport
    {
        public static string CreateReport(RunSuite suite)
        {
            var pdfFile = suite.FileName.Replace(".json", ".pdf");
            using (var stream = new FileStream(pdfFile, FileMode.Create))
            {
                var doc = new Document();
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                
                doc.Add(new Paragraph(new Phrase(20.0f, "Suite Results for " + suite.Name + " Feature")));
                doc.Add(suite.GetSummaryTable());

                foreach (var scenario in suite.Scenarios)
                    doc.Add(scenario.GetBodyTable());

                doc.Close();
            }

            return pdfFile;
        }
    }

    internal static class PdfExtensions
    {
        internal static PdfPTable GetSummaryTable(this RunSuite suite)
        {
            var table = new PdfPTable(new[] { 0.5f, 0.5f }); //two cols 50% each

            var blueFont = new Font(Font.HELVETICA, 10f, Font.BOLD, new Color(43, 145, 175));
            var values = suite.GetSummary();

            foreach (var kv in values)
            {
                table.AddCell(kv.Key.GetBorderlessCell());
                table.AddCell(kv.Value.GetBorderlessCell(blueFont));
            }

            return table;
        }

        internal static PdfPTable GetBodyTable(this RunScenario scenario)
        {
            var table = new PdfPTable(new[] { 1f, 2f }); //two cols 1/3, 2/3
                        
            foreach (var kv in scenario.GetBodyProperties())
            {
                table.AddCell(kv.Key.GetNormalCell());
                table.AddCell(kv.Value.GetNormalCell());
            }

            return table;
        }

        internal static PdfPCell GetBorderlessCell(this string value, Font font = null)
        {
            var cell = new PdfPCell(
                font != null
                    ? new Phrase(value, font)
                    : new Phrase(value));
            cell.Border = Rectangle.NO_BORDER;

            return cell;
        }

        internal static PdfPCell GetNormalCell(this object value, Font font = null)
        {
            var cell = new PdfPCell(
                font != null
                    ? new Phrase(value.ToString(), font)
                    : new Phrase(value.ToString()));
            cell.Border = Rectangle.LISTITEM;

            return cell;
        }
    }
}
