using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Channels;

namespace ThreadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //new ThreadClassPractice().Test();
            //while (true);

            //new ThreadPoolPractice().Test();
            //Console.WriteLine("25-05-2018 => " + DateTime.Parse("25-05-2018"));
            //Console.WriteLine("25/05/2018 => " + DateTime.Parse("25/05/2018"));
            //NullableVariableTest.TestNullable();

            //TaskExample.TestTask();
            TaskExampleSys32Files.TestTask();
            //TaskExampleOnlyRanToCompletion.TestTask();
            //TaskExampleContinueWith.TestTaskPrimeNumbersWithOptions(new []{"1000"});
            //TaskExampleContinuationState.TestTask();
            Console.ReadKey();
        }
    }
}
