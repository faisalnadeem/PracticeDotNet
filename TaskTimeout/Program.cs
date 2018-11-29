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

	        var counter = 0; 
	        var tokenSource2 = new CancellationTokenSource();
	        CancellationToken ct = tokenSource2.Token;

	        var task = Task.Factory.StartNew(() =>
	        {

		        // Were we already canceled?
		        ct.ThrowIfCancellationRequested();

		        bool moreToDo = true;
		        while (moreToDo)
		        {
			        counter++;
					Console.WriteLine("wroking...."+ counter);
			        // Poll on this property if you have to do
			        // other cleanup before throwing.
			        if (ct.IsCancellationRequested)
			        {
						Console.WriteLine("cancelling....");
				        // Clean up here, then...
				        ct.ThrowIfCancellationRequested();
			        }

			        if (counter >= 900)
			        {
				        tokenSource2.Cancel();
					}

		        }
	        }, tokenSource2.Token); // Pass same token to StartNew.


	        // Just continue on this thread, or Wait/WaitAll with try-catch:
	        try
	        {
		        task.Wait();
	        }
	        catch (AggregateException e)
	        {
		        foreach (var v in e.InnerExceptions)
			        Console.WriteLine(e.Message + " " + v.Message);
	        }
	        finally
	        {
		        tokenSource2.Dispose();
	        }

	        Console.ReadKey();
			/*
						int timeout = 1000;
						TimeSpan timepSpan = new TimeSpan(0,0, 4);
						var delayTask = Task.Delay(timeout);
						var task = CountToAsync();
						var resutl = task.TimeoutAfter(timepSpan).Result;

						var addTask = AddTask(10, 5).TimeoutAfter(timepSpan);
						var subtractTask = AddTask(10, 5).TimeoutAfter(timepSpan);
						var multiplyTask = AddTask(10, 5).TimeoutAfter(timepSpan);

						Console.WriteLine("Press any key to exit");
						Console.ReadKey();*/
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
