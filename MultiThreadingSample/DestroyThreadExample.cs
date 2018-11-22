using System;
using System.Threading;

namespace MultiThreadingSample
{
    public class DestroyThreadExample
    {
        public bool IsCancelled { get; set; }

        public Thread MyThread { get; set; }

        public void StartThread()
        {
            MyThread = new Thread(() =>
            {
                int numberOfSeconds = 0;
                while (numberOfSeconds < 8)
                {
                    if (IsCancelled == false)
                    {
                        break;
                    }

                    Thread.Sleep(1000);

                    numberOfSeconds++;
                }

                Console.WriteLine("I ran for {0} seconds", numberOfSeconds);
            });
        }

        public void Abort()
        {
            //Destroy thread
            MyThread.Abort();
        }

        public void GracefulAbort()
        {
            IsCancelled = true;
        }
    }
}
