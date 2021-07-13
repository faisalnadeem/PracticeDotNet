using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace CsharpPlayGround
{
    //[assembly: NeutralResourcesLanguage("en")]
    public class SatelliteAssembliesExample
    {
        public static void MainTest()
        {
            // Create array of supported cultures
            string[] cultures = {"en-CA", "en-US", "fr-FR", "ru-RU"};
            Random rnd = new Random();
            int cultureNdx = rnd.Next(0, cultures.Length);
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            try {
                CultureInfo newCulture = new CultureInfo(cultures[cultureNdx]);
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;
                ResourceManager rm = new ResourceManager("Greetings",
                    typeof(SatelliteAssembliesExample).Assembly);
                string greeting = String.Format("The current culture is {0}.\n{1}",
                    Thread.CurrentThread.CurrentUICulture.Name,
                    rm.GetString("HelloString"));

                Console.WriteLine(greeting);
            }
            catch (CultureNotFoundException e) {
                Console.WriteLine("Unable to instantiate culture {0}", e.InvalidCultureName);
            }
            finally {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalCulture;
            }
        }
    }
}
