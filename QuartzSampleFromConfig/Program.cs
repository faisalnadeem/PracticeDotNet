using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using NodaTime;
using Quartz;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig
{
	class Program
    {
        
		static string _connectionString = @"Server=PLLLP9435;initial catalog=EmailPoc;Integrated Security=true";
		static void Main(string[] args)
        {
        const string DATE_TIME_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";

            var estDel = "12/12/2020";
            const string startTime = "00:00:00";
            const string endTime = "23:59:59";

        DateTime? estStart; 
            estStart = DateTime.Parse($"{estDel} {startTime}");
        DateTime? estEmd;
           estEmd = DateTime.Parse($"{estDel} {endTime}");

            DateTimeOffset eventTimestamp = new DateTimeOffset(2020, 12, 10, 10, 12, 20, TimeSpan.Zero);
			var deliverySlotDate = ParseDeliverySlotDate(eventTimestamp);

        const string ESTIMATED_DELIVERY_DATE_TIME_START = "00:00:00";
        const string ESTIMATED_DELIVERY_DATE_TIME_END = "23:59:59";


		var dateStart = (DateTime?) DateTime.Parse($"{deliverySlotDate} {ESTIMATED_DELIVERY_DATE_TIME_START}");
		var dateEnd = (DateTime?) DateTime.Parse($"{deliverySlotDate} {ESTIMATED_DELIVERY_DATE_TIME_END}");


		   Console.WriteLine(estStart.Value.ToString(DATE_TIME_FORMAT));
		   Console.WriteLine(estEmd.Value.ToString(DATE_TIME_FORMAT));
        return;


		new CronExpressionToTimestamp().TestCronExpressionConversion();

			return;
			//Console.WriteLine("Writing out to file - C:/sftptemp/ConsoleOutputToFile.txt");
			//StartConsoleOutputToFile();
			var allThreadsCulture = new CultureInfo(ConfigurationManager.AppSettings["AllThreadsCulture"]);
			Thread.CurrentThread.CurrentCulture = allThreadsCulture;
			Thread.CurrentThread.CurrentUICulture = allThreadsCulture;
			CultureInfo.DefaultThreadCurrentCulture = allThreadsCulture;
			CultureInfo.DefaultThreadCurrentUICulture = allThreadsCulture;

			var runner = new ApplicationServerHost(new ApplicationServerEngine());
			runner.Execute();

			//StopConsoleOutputToFile();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

        private static LocalDate ParseDeliverySlotDate(DateTimeOffset? deliverySlotAvailableEvent)
        {
            var deliverySlotDate = deliverySlotAvailableEvent ?? DateTimeOffset.UtcNow;

            // convert the date to carrier default time zone
            var deliverySlotDateInCarrierZone = ConvertToCarrierZonedDateTime(deliverySlotDate);
            return deliverySlotDateInCarrierZone.Date;
        }
        public const string CARRIER_TIMEZONE = "Europe/London";

		private static ZonedDateTime ConvertToCarrierZonedDateTime(DateTimeOffset dateTimeOffset)
        {
            var carrierTimeZone = DateTimeZoneProviders.Tzdb[CARRIER_TIMEZONE];
            var zonedDateTimeNow = new ZonedDateTime(Instant.FromDateTimeOffset(dateTimeOffset), carrierTimeZone);
            return zonedDateTimeNow;
        }


        private static void StopConsoleOutputToFile()
		{
			Console.SetOut(Console.Out);
		}

		private static void StartConsoleOutputToFile()
		{
			var fileName = "C:/sftptemp/ConsoleOutputToFile.txt";
			if(File.Exists(fileName))
				File.Delete(fileName);

			FileStream ostrm;
			StreamWriter writer;
			try
			{
				ostrm = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
				writer = new StreamWriter(ostrm);
			}
			catch (Exception e)
			{
				Trace.WriteLine("Cannot open ConsoleOutputToFile.txt for writing");
				Trace.WriteLine(e.Message);
				return;
			}
			Console.SetOut(writer);
		}

		private static void StartTraceListeners()
		{
			Trace.Listeners.Clear();
			var tempFilePath = Path.Combine(Path.GetTempPath(), AppDomain.CurrentDomain.FriendlyName);
			var fileName = "C:/sftptemp/TextWriterConsoleOutput.log";
			var textWriterTraceListener =
				new TextWriterTraceListener(fileName, "textWriterListener")
				{
					Name = "TextWriterListener",
					TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime
				};

			var consoleTraceListener = new ConsoleTraceListener(false);
			consoleTraceListener.TraceOutputOptions = TraceOptions.DateTime;

			Trace.Listeners.Add(textWriterTraceListener);
			Trace.Listeners.Add(consoleTraceListener);
			Trace.AutoFlush = true;

			Trace.WriteLine($"Started console listener and text writer listener. File path for text writer {tempFilePath}");
		}

		private static void StartTasks()
		{
			var task1 = Task.Factory.StartNew(() =>
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					connection.Open();
					
						var emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
						while (emailRow != null)
						{
							ConsumeEmailRow("T-1", emailRow, connection);
							emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
						}
				}
			});

			var task2 = Task.Factory.StartNew(() =>
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					connection.Open();
						var emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
						while (emailRow != null)
						{
							ConsumeEmailRow("T-2", emailRow, connection);
							emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
						}
				}
			});

			//var task3 = Task.Factory.StartNew(() =>
			//{
			//	using (var connection = new SqlConnection(_connectionString))
			//	{
			//		connection.Open();
			//		using (var tx = connection.BeginTransaction())
			//		{

			//			var emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
			//			while (emailRow != null)
			//			{
			//				ConsumeEmailRow("T-3", emailRow, connection);
			//				emailRow = SqlDataHelper.GetNextUserIdToSendEmail(connection);
			//			}

			//			tx.Commit();
			//		}
			//	}
			//});

			//var task2 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-2", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task3 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-3", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task4 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-4", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task5 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-5", SqlDataHelper.GetNextUserIdToSendEmail()));

			Task.WaitAll(task1, task2);
			Trace.WriteLine("All task finished");

		}
		private static void StartThreads()
		{
			var t1 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Trace.WriteLine("T1: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T1", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Trace.WriteLine($"T1: I ran for {numberOfSeconds} seconds");
			});

			var t2 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Trace.WriteLine("T2: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T2", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Trace.WriteLine($"T2: I ran for {numberOfSeconds} seconds");
			});

			var t3 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Trace.WriteLine("T3: Sleeping for 0.5 second");
					Thread.Sleep(1000);
					numberOfSeconds++;
					ConsumeEmailRow("T3", (EmailQueue) emailRow);

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Trace.WriteLine($"T3: I ran for {numberOfSeconds} seconds");
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
			Trace.WriteLine($"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
			//EmailHelper.SendEmail();
			Trace.WriteLine($"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
			Trace.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
			SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
			SqlDataHelper.MarkAsConsumedByEmailQueueIdHistory(emailRow.Id, threadName);
			Trace.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
		}

		private static void ConsumeEmailRow(string threadName, EmailQueue emailRow)//, EmailQueue emailRow)
		{
			//var emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
			if (emailRow == null) return;
			Trace.WriteLine($"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
			//EmailHelper.SendEmail();
			Trace.WriteLine($"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
			Trace.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
			SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
			SqlDataHelper.MarkAsConsumedByEmailQueueIdHistory(emailRow.Id, threadName);
			Trace.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
		}

		private static void ConsumeEmailRow(string threadName, EmailQueue emailRow, SqlConnection connection)//, EmailQueue emailRow)
		{
			//var emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
			if (emailRow == null) return;
			Trace.WriteLine($"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
			//EmailHelper.SendEmail();
			Trace.WriteLine($"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
			Trace.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
			SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id, connection);
			SqlDataHelper.MarkAsConsumedByEmailQueueIdHistory(emailRow.Id, threadName, connection);
			Trace.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
		}

		public static void StartSimpleCanellationTest(object obj)
		{
			var ct = (CancellationToken)obj;
			Trace.WriteLine("StartSimpleCanellationTest is running on another thread.");

			// Simulate work that can be canceled.
			while (!ct.IsCancellationRequested)
			{
				Thread.SpinWait(50000);
			}
			Trace.WriteLine("The worker thread has been canceled. Press any key to exit.");
			Console.ReadKey(true);
		}
	}

	public class TheClub    
	{
		static SemaphoreSlim _sem = new SemaphoreSlim(3);    // Capacity of 3
	
		public static void Enter(object id)
		{
			Trace.WriteLine(id + " wants to enter");
			//_sem.Wait();
			Trace.WriteLine(id + " is in!");           // Only three threads
			//Thread.Sleep(1000 * (int)id);               // can be here at
			Trace.WriteLine(id + " is leaving");       // a time.
			_sem.Release();
		}
	}

	public class SimpleThreadCancellation
	{
		public static void Test()
		{
			// The Simple class controls access to the token source.
			var cts = new CancellationTokenSource();

			Trace.WriteLine("Press 'C' to terminate the application...\n");
			// Allow the UI thread to capture the token source, so that it
			// can issue the cancel command.
			var t1 = new Thread(() =>
			{
				//if (Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant() == "C")
				//	cts.Cancel();				
				var randomvalue = 1;
				while (randomvalue != 0)
				{
					Trace.WriteLine("Random value is " + randomvalue);
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
					Trace.WriteLine("Loop " + taskCtr);
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
							Trace.WriteLine($"Cancelling at task {iteration}");
							break;
						}
						values[ctr - 1] = value;
						Trace.WriteLine($"Vallue at {ctr - 1} is {value}");
					}
					return values;
				}, token));

			}
			try
			{
				var fTask = factory.ContinueWhenAll(tasks.ToArray(),
															 (results) =>
															 {
																 Trace.WriteLine("Calculating overall mean...");
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
				Trace.WriteLine($"The mean is {fTask.Result}.");
			}
			catch (AggregateException ae)
			{
				foreach (Exception e in ae.InnerExceptions)
				{
					if (e is TaskCanceledException)
						Trace.WriteLine("Unable to compute mean: {0}",
										  ((TaskCanceledException)e).Message);
					else
						Trace.WriteLine("Exception: " + e.GetType().Name);
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
			Trace.WriteLine($"Adding numbers at {DateTime.Now}: {x} + {y} : {x + y}");
			//await Task.Delay(TimeSpan.FromSeconds(5));
			return x + y;
		}

		void IJob.Execute(IJobExecutionContext context)
		{
			AddTask(2, 3);
		}
	}
}
