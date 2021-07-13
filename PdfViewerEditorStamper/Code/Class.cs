using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using PdfViewerEditorStamper.Controllers;

namespace PdfViewerEditorStamper.Code
{
    public class TestPdfReplaceText
    {
        static iTextSharp.text.pdf.PdfStamper stamper = null;
        static void MainTest(string[] args)
        {
            var oldFile = @"C:\oldFile.pdf";
            var newFile = @"C:\newFile.pdf";

            string replacingVariable = "Test";
            PdfReader pReader = new PdfReader(oldFile);
            stamper = new iTextSharp.text.pdf.PdfStamper(pReader, new System.IO.FileStream(newFile, System.IO.FileMode.Create));
            PDFTextGetter("ExistingVariableinPDF", replacingVariable, StringComparison.CurrentCultureIgnoreCase, oldFile, newFile);
            stamper.Close();
            pReader.Close();
        }

        public static void PDFTextGetter(string pSearch, string replacingText, StringComparison SC, string SourceFile, string DestinationFile)
        {
            try
            {
                iTextSharp.text.pdf.PdfContentByte cb = null;
                iTextSharp.text.pdf.PdfContentByte cb2 = null;
                iTextSharp.text.pdf.PdfWriter writer = null;
                iTextSharp.text.pdf.BaseFont bf = null;

                if (System.IO.File.Exists(SourceFile))
                {
                    PdfReader pReader = new PdfReader(SourceFile);
                    for (int page = 1; page <= pReader.NumberOfPages; page++)
                    {
                        myLocationTextExtractionStrategy strategy = new myLocationTextExtractionStrategy();
                        cb = stamper.GetOverContent(page);
                        cb2 = stamper.GetOverContent(page);
                        strategy.UndercontentCharacterSpacing = (int)cb.CharacterSpacing;
                        strategy.UndercontentHorizontalScaling = (int)cb.HorizontalScaling;
                        string currentText = PdfTextExtractor.GetTextFromPage(pReader, page, strategy);

                        List<iTextSharp.text.Rectangle> MatchesFound = strategy.GetTextLocations(pSearch, SC);
                        cb.SetColorFill(BaseColor.WHITE);
                        foreach (iTextSharp.text.Rectangle rect in MatchesFound)
                        {
                            cb.Rectangle(rect.Left, rect.Bottom, 60, rect.Height);
                            cb.Fill();
                            cb2.SetColorFill(BaseColor.BLACK);
                            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            cb2.SetFontAndSize(bf, 9);
                            cb2.BeginText();
                            cb2.ShowTextAligned(0, replacingText, rect.Left, rect.Bottom, 0);
                            cb2.EndText();
                            cb2.Fill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
