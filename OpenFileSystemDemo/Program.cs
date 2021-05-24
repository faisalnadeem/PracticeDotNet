using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;

namespace OpenFileSystemDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var fields = ListFieldNames();

            FillForm();

            return;
            var designType = DesignType.Desktop;
            var imgDir = string.Format("/content/{0}/images/", designType.ToString("G"));
            var imgDirD = string.Format("/content/{0}/images/", designType.ToString("D"));
            var imgDirX = string.Format("/content/{0}/images/", designType.ToString("X"));
            var imgDirF = string.Format("/content/{0}/images/", designType.ToString("F"));
            //var imgDirT = string.Format("/content/{0}/images/", designType.ToString("T")); //expect exception

        }

        private static string ListFieldNames()
        {
            string pdfTemplate = @"c:\Temp\PDF\fw4.pdf";
            // title the form  
            var text = " - " + pdfTemplate;
            // create a new PDF reader based on the PDF template document  
            PdfReader pdfReader = new PdfReader(pdfTemplate);
            // create and populate a string builder with each of the  
            // field names available in the subject PDF  
            StringBuilder sb = new StringBuilder();
            foreach (var de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }

            return sb.ToString();
        }

        private static void FillForm()
        {
            var pdfTemplate = @"c:\Temp\PDF\fw4.pdf";
            var newFile = @"c:\Temp\PDF\completed_fw4.pdf";
            var pdfReader = new PdfReader(pdfTemplate);
            var pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            var pdfFormFields = pdfStamper.AcroFields;
            // set form pdfFormFields  
            // The first worksheet and W-4 form  
            pdfFormFields.SetField("f1_01(0)", "1");
            pdfFormFields.SetField("f1_02(0)", "1");
            
            // The form's checkboxes  
            pdfFormFields.SetField("c1_01(0)", "0");
            pdfFormFields.SetField("c1_02(0)", "Yes");
            // The rest of the form pdfFormFields  
            pdfFormFields.SetField("f1_14(0)", "100 North Cujo Street");
            pdfFormFields.SetField("f1_15(0)", "Nome, AK  67201");
            pdfFormFields.SetField("f1_17(0)", "10");
            // Second Worksheets pdfFormFields  
            // In order to map the fields, I just pass them a sequential  
            // number to mark them; once I know which field is which, I  
            // can pass the appropriate value  
            pdfFormFields.SetField("f2_01(0)", "1");
            // report by reading values from completed PDF  
            string sTmp = "W-4 Completed for " + pdfFormFields.GetField("f1_09(0)") + " " + pdfFormFields.GetField("f1_10(0)");
            //MessageBox.Show(sTmp, "Finished");
            // flatten the form to remove editting options, set it to false  
            // to leave the form open to subsequent manual edits  
            pdfStamper.FormFlattening = false;
            // close the pdf  
            pdfStamper.Close();
        }
    }

    public enum DesignType
    {
        Desktop,
        Mobile,
        Redesign,
        //RoyalMail
    }

    public class FileParser
    {
        //public void Parse(IFile cvOutputFile)
        //{

        //}
    }
}
