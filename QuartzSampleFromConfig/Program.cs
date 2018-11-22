using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Quartz;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig
{
	class Program
	{
		static void Main(string[] args)
		{
			StartConsoleOutputToFile();
			StartThreads();
			//for (int i = 1; i <= 5; i++)
			//{
			//	new Thread(TheClub.Enter).Start(i);
			//}

			//SimpleThreadCancellation.Test();
			Console.ReadLine();
			return;
			//var i = 1;
			//while (true)
			//{
			//	Task.Factory.StartNew(() => ConsumeEmailRow("T-" + i));
			//	i++;
			//}

			var task1 = Task.Factory.StartNew(() => PerformTask("T-1"));
			var task2 = Task.Factory.StartNew(() => PerformTask("T-2"));
			var task3 = Task.Factory.StartNew(() => PerformTask("T-3"));
			var task4 = Task.Factory.StartNew(() => PerformTask("T-4"));
			var task5 = Task.Factory.StartNew(() => PerformTask("T-5"));

			Task.WaitAll(task1, task2, task3, task4, task5);
			Console.WriteLine("All task finished");

			//var runner = new ApplicationServerHost(new ApplicationServerEngine());
			//runner.Execute();
			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
		}

		private static void StopConsoleOutputToFile()
		{
			Console.SetOut(Console.Out);
		}
		private static void StartConsoleOutputToFile()
		{
			FileStream ostrm;
			StreamWriter writer;
			try
			{
				ostrm = new FileStream("C:/sftptemp/ConsoleOutputToFile.txt", FileMode.OpenOrCreate, FileAccess.Write);
				writer = new StreamWriter(ostrm);
			}
			catch (Exception e)
			{
				Console.WriteLine("Cannot open ConsoleOutputToFile.txt for writing");
				Console.WriteLine(e.Message);
				return;
			}
			Console.SetOut(writer);
		}

		private static void StartThreads()
		{
			var t1 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Console.WriteLine("T1: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T1", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Console.WriteLine("T1: I ran for {0} seconds", numberOfSeconds);
			});

			var t2 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Console.WriteLine("T2: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T2", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Console.WriteLine("T2: I ran for {0} seconds", numberOfSeconds);
			});

			var t3 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Console.WriteLine("T3: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T3", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Console.WriteLine("T3: I ran for {0} seconds", numberOfSeconds);
			});

			t1.Start(SqlDataHelper.GetNextUserIdToSendEmail());
			t2.Start(SqlDataHelper.GetNextUserIdToSendEmail());
			t3.Start(SqlDataHelper.GetNextUserIdToSendEmail());

		}

		private static void PerformTask(string threadName)
		{
			using (var scope = new TransactionScope())
			{
				ConsumeEmailRow(threadName);
				scope.Complete();
			}
		}

		private static void ConsumeEmailRow(string threadName)//, EmailQueue emailRow)
		{

			var emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
			if (emailRow == null) return;
			Console.WriteLine($"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
			//EmailHelper.SendEmail();
			Console.WriteLine($"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
			Console.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
			SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
			Console.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
		}
		private static void ConsumeEmailRow(string threadName, EmailQueue emailRow)//, EmailQueue emailRow)
		{
			//var emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
			if (emailRow == null) return;
			Console.WriteLine($"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
			//EmailHelper.SendEmail();
			Console.WriteLine($"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
			Console.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
			SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
			Console.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
		}

		public static void StartSimpleCanellationTest(object obj)
		{
			var ct = (CancellationToken)obj;
			Console.WriteLine("StartSimpleCanellationTest is running on another thread.");

			// Simulate work that can be canceled.
			while (!ct.IsCancellationRequested)
			{
				Thread.SpinWait(50000);
			}
			Console.WriteLine("The worker thread has been canceled. Press any key to exit.");
			Console.ReadKey(true);
		}
	}

	public class TheClub    
	{
		static SemaphoreSlim _sem = new SemaphoreSlim(3);    // Capacity of 3
	
		public static void Enter(object id)
		{
			Console.WriteLine(id + " wants to enter");
			//_sem.Wait();
			Console.WriteLine(id + " is in!");           // Only three threads
			//Thread.Sleep(1000 * (int)id);               // can be here at
			Console.WriteLine(id + " is leaving");       // a time.
			_sem.Release();
		}
	}

	public class SimpleThreadCancellation
	{
		public static void Test()
		{
			// The Simple class controls access to the token source.
			var cts = new CancellationTokenSource();

			Console.WriteLine("Press 'C' to terminate the application...\n");
			// Allow the UI thread to capture the token source, so that it
			// can issue the cancel command.
			var t1 = new Thread(() =>
			{
				//if (Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant() == "C")
				//	cts.Cancel();				
				var randomvalue = 1;
				while (randomvalue != 0)
				{
					Console.WriteLine("Random value is " + randomvalue);
					if (randomvalue == 0)
						cts.Cancel();
				
					randomvalue = new Random().Next(0, 10);
				}
			});

			// ServerClass sees only the token, not the token source.
			var t2 = new Thread(new ParameterizedThreadStart(Program.StartSimpleCanellationTest));
			// Start the UI thread.

			t1.Start();

			// Start the worker thread and pass it the token.
			t2.Start(cts.Token);

			t2.Join();
			cts.Dispose();
		}
	}

	public class CancellationTokenExample
	{
		public static void Test()
		{
			// Define the cancellation token.
			var source = new CancellationTokenSource();
			var token = source.Token;

			var rnd = new Random();
			object lockObj = new Object();

			var tasks = new List<Task<int[]>>();
			var factory = new TaskFactory(token);
			for (var taskCtr = 0; taskCtr <= 10; taskCtr++)
			{
				var iteration = taskCtr + 1;
				tasks.Add(factory.StartNew(() =>
				{
					Console.WriteLine("Loop " + taskCtr);
					int value;
					var values = new int[10];
					for (var ctr = 1; ctr <= 10; ctr++)
					{
						lock (lockObj)
						{
							value = rnd.Next(0, 101);
						}
						if (value == 0)
						{
							source.Cancel();
							Console.WriteLine("Cancelling at task {0}", iteration);
							break;
						}
						values[ctr - 1] = value;
						Console.WriteLine($"Vallue at {ctr - 1} is {value}");
					}
					return values;
				}, token));

			}
			try
			{
				var fTask = factory.ContinueWhenAll(tasks.ToArray(),
															 (results) =>
															 {
																 Console.WriteLine("Calculating overall mean...");
																 long sum = 0;
																 int n = 0;
																 foreach (var t in results)
																 {
																	 foreach (var r in t.Result)
																	 {
																		 sum += r;
																		 n++;
																	 }
																 }
																 return sum / (double)n;
															 }, token);
				Console.WriteLine("The mean is {0}.", fTask.Result);
			}
			catch (AggregateException ae)
			{
				foreach (Exception e in ae.InnerExceptions)
				{
					if (e is TaskCanceledException)
						Console.WriteLine("Unable to compute mean: {0}",
										  ((TaskCanceledException)e).Message);
					else
						Console.WriteLine("Exception: " + e.GetType().Name);
				}
			}
			finally
			{
				source.Dispose();
			}
		}
	}

	public class ConsoleWriterJob : IJob
	{
		//public Task Execute(IJobExecutionContext context)
		//{
		//    return AddTask(2, 3);
		//}

		private int AddTask(int x, int y)
		{
			Console.WriteLine($"Adding numbers at {DateTime.Now}: {x} + {y} : {x + y}");
			//await Task.Delay(TimeSpan.FromSeconds(5));
			return x + y;
		}

		void IJob.Execute(IJobExecutionContext context)
		{
			AddTask(2, 3);
		}
	}
}
