using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsharpPlayGround
{
    public class Program
    {
        private static string location;
        private static DateTime time;
        private static string result;
        delegate void Printer();
        delegate void PrinterWithMessage(string message);

        delegate void Greeting(string language, string message);

        static void Main(string[] args)
        {
            //SizeOfOperator.MainTest();
            //TraceTest.MainTest();
            //HashTableExample.MainTest();
            //InvariantCultureExample.MainTest();
            //CsharpPlayGround.MainTest();
            //SatelliteAssembliesExample.MainTest();
            //SamplesCalendar.MainTest();
            //CompareInfoSample.MainTest();
            //StaticConstructorTest.MainTest();
            //TypeAttributesTest.MainTest();
            //ValueTypeTest.MainTest();

            //TuplesTest.MainTest();

            HashSetExample.MainTest();

            var languages = new Dictionary<string, string>
                {{"English", "Hello"}, {"Spanish", "Hola"}, {"French", "Bonjour"}, {"Urdu", "AoA"}};
            ;
            List<Greeting> greetings = new List<Greeting>();
            foreach (var lang in languages)
            {
                greetings.Add(delegate(string language, string message)
                {
                    //Console.WriteLine($"{lang.Key} - {lang.Value}");
                });
            }

            foreach (var greeting in greetings)
            {

            }
//            greetings.Add((("English", "Hello") => {Console.WriteLine($"{language} - {message}");}));

            List<Printer> printers = new List<Printer>();
            int i = 0;
            for (; i < 10; i++)
            {
                printers.Add(delegate{Console.WriteLine(i);});
            }

            foreach (var printer in printers)
            {
                printer();
            }

            SaySomething();
            Console.WriteLine(result);

            Console.WriteLine(location == null ? "location is null": location);
            Console.WriteLine(time == null ? "time is null": time.ToString());

            string[] names = new string[3] {"ab", "cd", "ef"};
            names.Append("alb");
            Console.WriteLine(names[2]);


            int[] numbers = new  int[]{2,3,4,5,6,7};
            var totalOfEven = numbers.Where(x => x % 2 == 0).Sum();
            Console.WriteLine("total of even numbers is " + totalOfEven);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static async Task<string> SaySomething()
        {
            await Task.Delay(5);
            //Thread.Sleep(5);
            result = "Hello world!";
            Console.WriteLine("result after await" + result);
            return "Something";
        }
    }
}
