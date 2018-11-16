using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timers = System.Timers;

namespace ThreadDemo
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1.continuewith?view=netframework-4.7.2
    /*
        The returned Task will not be scheduled for execution until the current task has completed.
        If the criteria specified through the continuationOptions parameter are not met, the continuation task will be canceled instead of scheduled.
     
     
     */
    class TaskExampleContinueWith
    {
        public static void TestTask()
        {
            var taskA = Task.Run(() => DateTime.Today.DayOfWeek);
            var taskB = Task.Run(() => "TaskB - Hello world");

            var continuationA = taskA.ContinueWith(antecedent => Console.WriteLine("Today is {0}.", antecedent.Result));
            var continuationB = taskB.ContinueWith(antecedent => Console.WriteLine("Continuing after task B - {0}.", antecedent.Result));
        }

        static CancellationTokenSource ts;

        public static void TestTaskPrimeNumbersWithOptions(string[] args)
        {
            int upperBound = args.Length >= 1 ? Int32.Parse(args[0]) : 200;
            ts = new CancellationTokenSource();
            CancellationToken token = ts.Token;
            Timers.Timer timer = new Timers.Timer(3000);
            timer.Elapsed += TimedOutEvent;
            timer.AutoReset = false;
            timer.Enabled = true;

            var t1 = Task.Run(() => { // True = composite.
                                      // False = prime.
                bool[] values = new bool[upperBound + 1];
                for (int ctr = 2; ctr <= (int)Math.Sqrt(upperBound); ctr++)
                {
                    if (values[ctr] == false)
                    {
                        for (int product = ctr * ctr; product <= upperBound;
                                                      product = product + ctr)
                            values[product] = true;
                    }
                    token.ThrowIfCancellationRequested();
                }
                return values;
            }, token);

            var t2 = t1.ContinueWith((antecedent) => { // Create a list of prime numbers.
                var primes = new List<int>();
                token.ThrowIfCancellationRequested();
                bool[] numbers = antecedent.Result;
                string output = String.Empty;

                for (int ctr = 1; ctr <= numbers.GetUpperBound(0); ctr++)
                    if (numbers[ctr] == false)
                    {
                        int.Parse(ctr.ToString()+"asldfkj");
                        primes.Add(ctr);
                    }

                // Create the output string.
                for (int ctr = 0; ctr < primes.Count; ctr++)
                {
                    token.ThrowIfCancellationRequested();
                    output += primes[ctr].ToString("N0");
                    if (ctr < primes.Count - 1)
                        output += ",  ";
                    if ((ctr + 1) % 8 == 0)
                        output += Environment.NewLine;
                }
                //Display the result.
                Console.WriteLine("Prime numbers from 1 to {0}:\n",
                                  upperBound);
                Console.WriteLine(output);
            }, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);
            try
            {
                t2.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e.GetType() == typeof(TaskCanceledException))
                        Console.WriteLine("The operation was cancelled.");
                    else
                        Console.WriteLine("ELSE: {0}: {1}", e.GetType().Name, e.Message);
                }
            }
            finally
            {
                ts.Dispose();
            }
        }

        public static void TestTaskPrimeNumbers(string[] args)
        {
            int upperBound = args.Length >= 1 ? Int32.Parse(args[0]) : 200;
            ts = new CancellationTokenSource();
            CancellationToken token = ts.Token;
            Timers.Timer timer = new Timers.Timer(3000);
            timer.Elapsed += TimedOutEvent;
            timer.AutoReset = false;
            timer.Enabled = true;

            var t1 = Task.Run(() => { // True = composite.
                                      // False = prime.
                bool[] values = new bool[upperBound + 1];
                for (int ctr = 2; ctr <= (int)Math.Sqrt(upperBound); ctr++)
                {
                    if (values[ctr] == false)
                    {
                        for (int product = ctr * ctr; product <= upperBound;
                                                      product = product + ctr)
                            values[product] = true;
                    }
                    token.ThrowIfCancellationRequested();
                }
                return values;
            }, token);

            var t2 = t1.ContinueWith((antecedent) => { // Create a list of prime numbers.
                var primes = new List<int>();
                token.ThrowIfCancellationRequested();
                bool[] numbers = antecedent.Result;
                string output = String.Empty;

                for (int ctr = 1; ctr <= numbers.GetUpperBound(0); ctr++)
                    if (numbers[ctr] == false)
                        primes.Add(ctr);

                // Create the output string.
                for (int ctr = 0; ctr < primes.Count; ctr++)
                {
                    token.ThrowIfCancellationRequested();
                    output += primes[ctr].ToString("N0");
                    if (ctr < primes.Count - 1)
                        output += ",  ";
                    if ((ctr + 1) % 8 == 0)
                        output += Environment.NewLine;
                }
                //Display the result.
                Console.WriteLine("Prime numbers from 1 to {0}:\n",
                                  upperBound);
                Console.WriteLine(output);
            }, token);
            try
            {
                t2.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e.GetType() == typeof(TaskCanceledException))
                        Console.WriteLine("The operation was cancelled.");
                    else
                        Console.WriteLine("ELSE: {0}: {1}", e.GetType().Name, e.Message);
                }
            }
            finally
            {
                ts.Dispose();
            }
        }

        private static void TimedOutEvent(Object source, Timers.ElapsedEventArgs e)
        {
            ts.Cancel();
        }


    }
}
