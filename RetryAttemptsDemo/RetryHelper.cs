using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NhibernateWithRetry
{
    public static class RetryHelper
    {
        public static void ExecuteTaskWithRetryAttempts(Action operation)
        {
            var maxRetryAttempts = 3;
            var delayAfterFailure = TimeSpan.FromSeconds(2);

            Console.WriteLine("Execution started");
            RetryHelper.RetryOnException(maxRetryAttempts, delayAfterFailure, operation);
            Console.WriteLine("Execution finished");
        }
        public static void TestRetryAttempts()
        {
            var maxRetryAttempts = 3;
            var delayAfterFailure = TimeSpan.FromSeconds(2);

            Console.WriteLine("Starting connections");

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Starting connections no {i + 1}");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage result = null;

                    RetryHelper.RetryOnException(maxRetryAttempts, delayAfterFailure, () =>  InvokeUrl(client, out result));

                    if (result != null)
                        Console.WriteLine(result.StatusCode);
                }
            }

            Console.WriteLine("Connections done");
        }

        public static void TestRetryAttempts(Action operation)
        {
            var maxRetryAttempts = 3;
            var delayAfterFailure = TimeSpan.FromSeconds(2);

            Console.WriteLine("Starting connections");

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Starting connections no {i + 1}");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage result = null;

                    RetryHelper.RetryOnException(maxRetryAttempts, delayAfterFailure, operation);

                    if (result != null)
                        Console.WriteLine(result.StatusCode);
                }
            }

            Console.WriteLine("Connections done");
        }

        private static void InvokeUrl(HttpClient client, out HttpResponseMessage result)
        {
            result = client.GetAsync("").Result;
        }

        public static void RetryOnException(int maxRetryAttempts, TimeSpan delayAfterFailure, Action operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == maxRetryAttempts)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Exception caught on attempt {attempts} - No more retries. {Environment.NewLine} Exception message: {ex}");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Exception caught on attempt {attempts} - will retry after delay {delayAfterFailure}. {Environment.NewLine} Exception message: {ex}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Task.Delay(delayAfterFailure).Wait();
                }
            } while (true);
        }
    }
}