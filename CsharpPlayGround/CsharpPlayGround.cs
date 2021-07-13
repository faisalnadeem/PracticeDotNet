using System;
using System.Globalization;
using System.Resources;
using System.IO;
using System.Reflection;

namespace CsharpPlayGround
{
    //ref: https://docs.microsoft.com/en-us/dotnet/api/system.resources.missingmanifestresourceexception?view=netcore-3.1
    public class MissingManifestResourceExceptionExample
    {
        public static void MainTest(string filename)
        {
            //if (Environment.GetCommandLineArgs().Length == 1) { 
            //    Console.WriteLine("No filename.");
            //    return;
            //}
      
            //string filename = Environment.GetCommandLineArgs()[1].Trim();
            // Check whether the file exists.
            if (! File.Exists(filename)) {
                Console.WriteLine("{0} does not exist.", filename);
                return;
            }   
      
            // Try to load the assembly.
            Assembly assem = Assembly.LoadFrom(filename);
            Console.WriteLine("File: {0}", filename);
         
            // Enumerate the resource files.
            string[] resNames = assem.GetManifestResourceNames();
            if (resNames.Length == 0)
                Console.WriteLine("   No resources found.");

            foreach (var resName in resNames)
                Console.WriteLine("   Resource: {0}", resName.Replace(".resources", ""));

            Console.WriteLine();
        }
    }
    public class CsharpPlayGround
    {
        public static void MainTest()
        {
            //test exception first
            MissingManifestResourceExceptionExample.MainTest("CsharpPlayGround");
            //

            string day;
            string year;
            string holiday;
            string celebrate = "{0} will occur on {1} in {2}.\n";

            // Create a resource manager.
            ResourceManager rm = new ResourceManager("rmc", 
                typeof(CsharpPlayGround).Assembly);

            Console.WriteLine("Obtain resources using the current UI culture.");

            // Get the resource strings for the day, year, and holiday
            // using the current UI culture.
            day  = rm.GetString("day");
            year = rm.GetString("year");
            holiday = rm.GetString("holiday");
            Console.WriteLine(celebrate, holiday, day, year);

            // Obtain the es-MX culture.
            CultureInfo ci = new CultureInfo("es-MX");

            Console.WriteLine("Obtain resources using the es-MX culture.");

            // Get the resource strings for the day, year, and holiday
            // using the specified culture.
            day  = rm.GetString("day", ci);
            year = rm.GetString("year", ci);
            holiday = rm.GetString("holiday", ci);
// ---------------------------------------------------------------
// Alternatively, comment the preceding 3 code statements and
// uncomment the following 4 code statements:
// ----------------------------------------------------------------
// Set the current UI culture to "es-MX" (Spanish-Mexico).
//    Thread.CurrentThread.CurrentUICulture = ci;

// Get the resource strings for the day, year, and holiday
// using the current UI culture. Use those strings to
// display a message.
//    day  = rm.GetString("day");
//    year = rm.GetString("year");
//    holiday = rm.GetString("holiday");
// ---------------------------------------------------------------

// Regardless of the alternative that you choose, display a message
// using the retrieved resource strings.
            Console.WriteLine(celebrate, holiday, day, year);
        }
    }
}
/*
This example displays the following output:

   Obtain resources using the current UI culture.
   "5th of May" will occur on Friday in 2006.

   Obtain resources using the es-MX culture.
   "Cinco de Mayo" will occur on Viernes in 2006.
*/