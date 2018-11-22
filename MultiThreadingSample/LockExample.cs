using System.Threading;

namespace MultiThreadingSample
{
    
    public class LockExample
    {

        public int SharedResource { get; set; }

        public object _locker = 0;

        public void StartThreadAccessingSharedResource()
        {
            Thread t1 = new Thread(() =>
            {
                lock (_locker)
                {
                    SharedResource++;
                }

            });

            Thread t2 = new Thread(() =>
            {
                lock (_locker)
                {
                    SharedResource--;
                }

            });

            t1.Start();
            t2.Start();
        }
    }
}
