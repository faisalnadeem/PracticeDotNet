using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    public class ThreadPoolPractice
    {
        private int _numberOfThreads = 5;
        private int[] _inputArray;
        private double[] _resultArray;
        private ManualResetEvent[] _resetEvents;

        public void Test()
        {
            _inputArray = new int[_numberOfThreads];
            _resultArray = new double[_numberOfThreads];
            _resetEvents = new ManualResetEvent[_numberOfThreads];


            for (var i = 0; i < _numberOfThreads; i++)
            {
                _inputArray[i] = i;
                _resetEvents[i] = new ManualResetEvent(false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), (object) i);
            }

            Console.WriteLine("Waiting.....");
            WaitHandle.WaitAll(_resetEvents);

            Console.WriteLine("Answers are: ");

            for (int i = 0; i < _numberOfThreads; i++)
            {
                Console.WriteLine(_inputArray[i] + " -> " + _resultArray[i]);
            }
        }

        private void DoWork(object state)
        {
            int index = (int) state;
            _resultArray[index] = index * 5;
            _resetEvents[index].Set();
        }
    }
}
