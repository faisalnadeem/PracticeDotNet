using System;
using NhibernateWithRetry.NhibernateData;

namespace NhibernateWithRetry
{
    class Program
    {
        static void Main(string[] args)
        {
            //RetryHelper.TestRetryAttempts();
            new DataHelper().Test();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
