using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTimeout
{
    class Program
    {
        static void Main(string[] args)
        {

            int timeout = 1000;
            TimeSpan timepSpan = new TimeSpan(0,0, 4);
            var delayTask = Task.Delay(timeout);
            var task = CountToAsync();
            var resutl = task.TimeoutAfter(timepSpan).Result;

            var addTask = AddTask(10, 5).TimeoutAfter(timepSpan);
            var subtractTask = AddTask(10, 5).TimeoutAfter(timepSpan);
            var multiplyTask = AddTask(10, 5).TimeoutAfter(timepSpan);
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static async Task<int> AddTask(int x, int y)
        {
            Console.WriteLine($"Adding numbers: {x} + {y} : {x+y}");
            await Task.Delay(TimeSpan.FromSeconds(5));
            return x + y;
        }

        private static async Task<int> SubtractTask(int x, int y)
        {
            Console.WriteLine($"Subtracting numbers: {x} - {y} : {x-y}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            return x - y;
        }
        private static async Task<int> MultiplyTask(int x, int y)
        {
            Console.WriteLine($"Multiplying numbers: {x} * {y} : {x*y}");
            await Task.Delay(TimeSpan.FromSeconds(3));
            return x * y;
        }

        private static async Task<DateTime> CountToAsync(int num = 10)
        {
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine($"Counter is at: {i}");
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            return DateTime.Now;
        }


    }

    public static class TaskExtensions
    {
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }

}
