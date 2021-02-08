using System;

namespace NhibernateWithRetry
{
    class Program
    {
        static void Main(string[] args)
        {
			//RetryHelper.TestRetryAttempts();
			//new DataHelper().Test();
			//new LeftJoinDemo().Test();
			LeftOuterJoinExample.Example();

			Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
