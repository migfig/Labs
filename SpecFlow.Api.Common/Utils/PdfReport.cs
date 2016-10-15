using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

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
                doc.SetMargins(1f, 1f, 40.0f, 40.0f);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                doc.Add(new Paragraph(30.0f, "Suite Results for " + suite.Name + " Feature"));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(PdfExtensions.GetSummaryTable(suite.GetSummary(), Rectangle.NO_BORDER, 0.25f, 0.25f, 0.5f));

                var i = 0;
                foreach (var scenario in suite.Scenarios)
                {
                    doc.Add(new Paragraph(Environment.NewLine));
                    doc.Add(scenario.GetBodyTable(++i));
                }

                doc.Close();
            }

            return pdfFile;
        }
    }

    internal static class PdfExtensions
    {
        internal static PdfPTable GetSummaryTable(IEnumerable<KeyValuePair<string, object>> values, int border = Rectangle.NO_BORDER, params float[] cellWidth)
        {
            var table = new PdfPTable(cellWidth);

            var blueFont = new Font(Font.HELVETICA, 10f, Font.BOLD, new Color(43, 145, 175));

            foreach (var kv in values)
            {
                table.AddCell(kv.Key.GetBorderlessCell(font: null, hasBackground: false, border: border));
                table.AddCell(kv.Value.GetBorderlessCell(font: blueFont, hasBackground: true, border: border));
                for (var i = 0; i < cellWidth.Length - 2; i++)
                    table.AddCell("".GetBorderlessCell(font: null, hasBackground: false, border: border));
            }

            return table;
        }

        internal static PdfPTable GetBodyTable(this RunScenario scenario, int testRun)
        {
            var table = new PdfPTable(new[] { 0.25f, 0.75f }); //two cols 1/3, 2/3
            var bodyProps = scenario.GetBodyProperties().ToList();
            bodyProps.Insert(1, new KeyValuePair<string, object>("Tet Run #", testRun));

            foreach (var kv in bodyProps)
            {
                table.AddCell(kv.Key.GetNormalCell());

                var value = kv.Value;
                if (kv.Key.Equals("RunTime"))
                    value += " Sec";

                if (kv.Key.Equals("Headers"))
                    table.AddCell(GetSummaryTable((IEnumerable<KeyValuePair<string, object>>)value, Rectangle.LISTITEM, 0.25f, 0.75f));
                else
                    table.AddCell(value.GetNormalCell(font: null, hasBackground: kv.Key.Equals("Name")));
            }

            return table;
        }

        internal static PdfPCell GetBorderlessCell(this object value, Font font = null, bool hasBackground = false, int border = Rectangle.NO_BORDER)
        {
            var cell = new PdfPCell(
                font != null
                    ? new Phrase(value.ToString(), font)
                    : new Phrase(value.ToString()));
            cell.Border = border;
            if (hasBackground)
                cell.BackgroundColor = new Color(255, 248, 204); //kind of beige

            return cell;
        }

        internal static PdfPCell GetNormalCell(this object value, Font font = null, bool hasBackground = false)
        {
            var strValue = value.ToString();

            var cell = new PdfPCell(
                font != null
                    ? new Phrase(strValue, font)
                    : new Phrase(strValue));
            cell.Border = Rectangle.LISTITEM;
            if (hasBackground)
                cell.BackgroundColor = new Color(192, 192, 192); //kind of gray

            return cell;
        }
    }
}
