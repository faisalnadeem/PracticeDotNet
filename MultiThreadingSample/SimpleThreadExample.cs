using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingSample
{
    public class SimpleThreadExample
    {

        public void StartMultipleThread()
        {
            DateTime startTime = DateTime.Now;

            Thread t1 = new Thread(() =>
            {
                int numberOfSeconds = 0;
                while (numberOfSeconds < 5)
                {
					Console.WriteLine("T1: Sleeping for 1 second");
                    Thread.Sleep(1000);

                    numberOfSeconds++;
                }

                Console.WriteLine("T1: I ran for 5 seconds");
            });

            Thread t2 = new Thread(() =>
            {
                int numberOfSeconds = 0;
                while (numberOfSeconds < 8)
                {
					Console.WriteLine("T2: Sleeping for 1 second");
                    Thread.Sleep(1000);
                    numberOfSeconds++;
                }

                Console.WriteLine("T2: I ran for 8 seconds");
            });


            //parameterized thread
            Thread t3 = new Thread(p =>
            {
                int numberOfSeconds = 0;
                while (numberOfSeconds < Convert.ToInt32(p))
                {
					Console.WriteLine("T3: Sleeping for 1 second");
                    Thread.Sleep(1000);

                    numberOfSeconds++;
                }

                Console.WriteLine("T3: I ran for {0} seconds", numberOfSeconds);
            });

            t1.Start();
            t2.Start();
            //passing parameter to parameterized thread
            t3.Start(20);

            ////wait for t1 to fimish
            //t1.Join();

            ////wait for t2 to finish
            //t2.Join();

            ////wait for t3 to finish
            //t3.Join();


            //Console.WriteLine("All Threads Exited in {0} secods", (DateTime.Now - startTime).TotalSeconds);
        }
       
    }
}
