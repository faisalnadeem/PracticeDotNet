using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    public class ThreadClassPractice
    {
        public void Test()
        {
            var t1 = new Thread(TestThread) {Name = "T1"};
            var t2 = new Thread(PrintMyName){ Name = "T2"};
            Console.WriteLine("Starting t1");
            t1.Start();

            while (!t1.IsAlive)
            {
                Console.WriteLine("T1 is dead");
            }
            while(t1.IsAlive) Console.WriteLine($"{Thread.CurrentThread.Name} - T1 is alive");
            MainThread();
            Console.WriteLine("Starting t1");
            t2.Start();
            while (!t2.IsAlive) ;

        }
        private  void MainThread()
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread test thread printing main " + i);
            }
        }
        private  void TestThread()
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread test thread printing number " + i);
            }
        }

        void PrintMyName()
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread test thread printing name " + i);
            }
        }

    }
}
