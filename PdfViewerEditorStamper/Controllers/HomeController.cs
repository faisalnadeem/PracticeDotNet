using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfViewerEditorStamper.Models;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfViewerEditorStamper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            new PdfFileHelper().TestReplacingTextInPdf(@"C:\temp\pdf\BACL 3.5.pdf", @"C:\temp\pdf\New-BACL 3.5.pdf");
           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class PdfFileHelper
    {
        static iTextSharp.text.pdf.PdfStamper stamper = null;
        protected void Replace_Click(object sender, EventArgs e)
        {
            string ReplacingVariable = "";//txtReplace.Text; 
            string sourceFile = "Source File Path";
            string descFile = "Destination File Path";
            PdfReader pReader = new PdfReader(sourceFile);
            stamper = new iTextSharp.text.pdf.PdfStamper(pReader, new System.IO.FileStream(descFile, System.IO.FileMode.Create));
            PDFTextGetter("ExistingVariableinPDF", ReplacingVariable , StringComparison.CurrentCultureIgnoreCase, sourceFile, descFile);
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

                        //Send some data contained in PdfContentByte, looks like the first is always cero for me and the second 100, 
                        //but i'm not sure if this could change in some cases
                        strategy.UndercontentCharacterSpacing = (int)cb.CharacterSpacing;
                        strategy.UndercontentHorizontalScaling = (int)cb.HorizontalScaling;

                        //It's not really needed to get the text back, but we have to call this line ALWAYS, 
                        //because it triggers the process that will get all chunks from PDF into our strategy Object
                        string currentText = PdfTextExtractor.GetTextFromPage(pReader, page, strategy);

                        //The real getter process starts in the following line
                        List<iTextSharp.text.Rectangle> MatchesFound = strategy.GetTextLocations(pSearch, SC);

                        //Set the fill color of the shapes, I don't use a border because it would make the rect bigger
                        //but maybe using a thin border could be a solution if you see the currect rect is not big enough to cover all the text it should cover
                        cb.SetColorFill(BaseColor.WHITE);

                        //MatchesFound contains all text with locations, so do whatever you want with it, this highlights them using PINK color:

                        foreach (iTextSharp.text.Rectangle rect in MatchesFound)
                        {
                            //width
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

        public void TestReplacingTextInPdf(string templateFilename, string outputFilename)
        {
            var outputFile = outputFilename;
            using var fileStream = new FileStream(outputFile, FileMode.CreateNew);
            var pdfReader = new PdfReader(templateFilename);
            var pdfStamper = new PdfStamper(pdfReader, fileStream);
            var pdfFormFields = pdfStamper.AcroFields;
            //StampFieldsForDssy34Document(pdfFormFields, formData, pdfStamper);
            pdfReader.Close();
            pdfStamper.FormFlattening = true;
            pdfStamper.FreeTextFlattening = true;
            pdfStamper.Close();
        } 
    }
}
