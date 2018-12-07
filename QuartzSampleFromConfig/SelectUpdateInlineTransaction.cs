using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig
{
	public class SelectUpdateInlineTransaction
	{
		private static List<Task> _tasks;
		static readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
		public static void RunTasks()
		{
			Console.WriteLine("SelectUpdateInlineTransaction: Starting tasks to send emails to users");
			//ConsoleOutputHelper.StartConsoleOutputToFile();

			var task1 = Task.Factory.StartNew(() => ConsumeQueue("T-1"));
			var task2 = Task.Factory.StartNew(() => ConsumeQueue("T-2"));
			var task3 = Task.Factory.StartNew(() => ConsumeQueue("T-3"));
			var task4 = Task.Factory.StartNew(() => ConsumeQueue("T-4"));
			var task5 = Task.Factory.StartNew(() => ConsumeQueue("T-5"));


			//Task.WaitAll(task1, task2, task2, task3, task4, task5);

			//ConsoleOutputHelper.StopConsoleOutputToFile();
			Console.WriteLine("SelectUpdateInlineTransaction: All tasks finished no more emails to send");

		}

		public static void RunTasksWitCancellationToken()
		{
			var token = _tokenSource.Token;

			Trace.Listeners.Add(CreateTextWriterTraceListener());

			Trace.WriteLine("RunTasksWitCancellationToken: Starting tasks to send emails to users");
			_tasks = new List<Task>();

			for (var i = 1; i <= 3; i++)
			{
				_tasks.Add(Task.Factory.StartNew(() => ConsumeQueue($"T-{i}", token), token));
			}

		}

		public static void StopTasksWithCancellationToken()
		{
			_tokenSource.Cancel();
		}

		private static void StopTask(Task task)
		{

			Console.WriteLine($"Stopping task {task.Id}");
		}


		private static void ConsumeQueueTxnScope(string threadName)
		{
			var emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
			while (emailRow != null)
			{
				try
				{
					emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
					EmailHelper.SendEmail(threadName, emailRow);
				}
				catch (Exception e)
				{
					Console.WriteLine($"{DateTime.Now}: Exception occurred while sending email UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}. Exception: {e}");
					SqlDataHelper.MarkAsErroredByEmailQueueId(emailRow.Id);
				}
			}
		}

		private static void ConsumeQueue(string threadName, CancellationToken token)
		{		
			token.ThrowIfCancellationRequested();

			var emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
			if(emailRow == null && Thread.CurrentThread.IsAlive)
				StopTasksWithCancellationToken();

			while (emailRow != null)
			{
				emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
				if (emailRow == null)
						StopTasksWithCancellationToken();
				try
				{
					if (emailRow != null)
					{
						EmailHelper.SendEmail(threadName, emailRow);
						SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
					}
				}
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(
						$"{DateTime.Now}: Exception occurred while sending email. Exception: {e}");
					Console.ForegroundColor = ConsoleColor.White;
					if (emailRow != null)
						SqlDataHelper.MarkAsErroredOrRetryByEmailQueueId(emailRow.Id);
						//SqlDataHelper.MarkAsErroredByEmailQueueId(emailRow.Id);
				}

				if (token.IsCancellationRequested)
				{
					try
					{
						token.ThrowIfCancellationRequested();
					}
					catch (Exception)
					{
						_tasks.ForEach(t => t.Wait());
					}
					finally
					{
						_tokenSource.Dispose();
					}
				}
			}
		}


		private static void ConsumeQueue(string threadName)
		{
			var emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
			while (emailRow != null)
			{
				try
				{
					emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmailInlineTxn(threadName);
					EmailHelper.SendEmail(threadName, emailRow);

				}
				catch (OperationCanceledException)
				{
					Trace.WriteLine(
						$"{DateTime.Now}: OperationCanceledException for Thread:{threadName}");
					Thread.CurrentThread.Abort();
				}
				catch (Exception e)
				{
					Trace.WriteLine(
						$"{DateTime.Now}: Exception occurred while sending email UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}. Exception: {e}");
					SqlDataHelper.MarkAsErroredByEmailQueueId(emailRow.Id);
				}
			}
		}

		public static TextWriterTraceListener CreateTextWriterTraceListener()
		{
			// Create a file for output named TestFile.txt.
			Stream myFile = File.Create("EmailLogOutput.txt", 1, FileOptions.Asynchronous);

			/* Create a new text writer using the output stream, and add it to
			 * the trace listeners. */
			var textListener = new
				TextWriterTraceListener(myFile);

			return textListener;
		}
	}
}