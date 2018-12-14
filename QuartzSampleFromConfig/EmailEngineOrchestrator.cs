using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig
{
	public class EmailEngineOrchestrator
	{
		private static List<Task> _tasks;
		static readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
		static string _connectionString = @"Server=PLLLP9435;initial catalog=EmailPoc;Integrated Security=true";

		public static void StartEmailEngineThreads()
		{
			var token = _tokenSource.Token;

			//Trace.Listeners.Add(CreateTextWriterTraceListener());
			//ConsoleOutputHelper.StartConsoleOutputToFile();
			Console.WriteLine("RunTasksWitCancellationToken: Starting tasks to send emails to users");
			_tasks = new List<Task>();

			for (var i = 1; i <= 3; i++)
			{
				var threadName = $"T-{i}";
				_tasks.Add(Task.Factory.StartNew(() => StartEmailConsumerThread(threadName, token), token));
			}

		}

		static void StartEmailConsumerThread(string threadName, CancellationToken token)
		{
			try
			{
				Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
				ConsumeQueueWithTransaction(threadName, token);
			}
			finally
			{
				Thread.CurrentThread.Priority = ThreadPriority.Normal;
			}
		}

		public static void StopTasksWithCancellationToken()
		{
			//ConsoleOutputHelper.StopConsoleOutputToFile();
			_tokenSource.Cancel();
			Task.WaitAll(_tasks.ToArray());
			foreach (var task in _tasks)
			{
				Console.WriteLine($"After waiting all: Task id {task.Id} status {task.Status}");
			}
		}


		private static void ConsumeQueueWithTransaction(string threadName, CancellationToken token)
		{
			//token.ThrowIfCancellationRequested();
			EmailQueue emailRow = null;

			do
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					connection.Open();
					using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
					{
						emailRow = GetTopRow(connection, transaction, threadName);

						if (emailRow == null)
							StopTasksWithCancellationToken();

						if (emailRow != null)
						{
							try
							{
								EmailHelper.SendEmail(threadName, emailRow);
								MarkAsConsumedById(emailRow.Id, connection, transaction, threadName);
							}
							catch (Exception e)
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine($"{DateTime.Now}: Exception occurred while sending email. Exception: {e}");
								Console.ForegroundColor = ConsoleColor.White;
								MarkAsErroredOrRetryById(emailRow.Id, connection, transaction, threadName);
							}

							transaction.Commit();
						}
					}
				}
			} while (emailRow != null);


			if (token.IsCancellationRequested)
			{
				try
				{
					token.ThrowIfCancellationRequested();
				}
				catch (Exception)
				{
					//break;
					//Thread.CurrentThread.Abort();
					//_tasks.ForEach(t => t.Wait());
				}
				finally
				{
					foreach (var task in _tasks)
					{
						Console.WriteLine($"Finally: Task id {task.Id} status {task.Status}");
					}

					_tokenSource.Dispose();
				}
			}

		}
		public static void MarkAsConsumedById(int id, SqlConnection connection, SqlTransaction transaction,
			string threadName)
		{
			var sqlQuery = $@"Update DBO.EmailQUEUE Set Status = 'SENT', DateSent = @dateSent WHERE ID = @id";
			using (var command = new SqlCommand(sqlQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@dateSent", DateTime.Now);
				command.ExecuteNonQuery();
			}
		}

		public static void MarkAsErroredOrRetryById(int id, SqlConnection connection, SqlTransaction transaction,
			string threadName)
		{
			var retryAfter = DateTime.Now.AddSeconds(30);
			var maxRetryAttempts = 3;

			var sqlQuery = $@" if (Select RetryCount+1 as NextRetryCount from EmailQueue where id = @id) > @maxRetryAttempts
								 Update DBO.EmailQUEUE Set Status = 'FAILED'  WHERE ID = @id
							   else
								 Update DBO.EmailQUEUE Set RetryCount = (RetryCount + 1 )  WHERE ID = @id";
			using (var command = new SqlCommand(sqlQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@retryAfter", retryAfter);
				command.Parameters.AddWithValue("@maxRetryAttempts",
					maxRetryAttempts);
				command.ExecuteNonQuery();
			}
		}


		private static EmailQueue GetTopRow(SqlConnection connection, SqlTransaction transaction, string threadName)
		{
			var emailRows = new List<EmailQueue>();
			var sqlQuery =
				@"declare @eid int
						SELECT TOP 1 @eid = eq.Id FROM dbo.EmailQueue eq 
						WHERE eq.DateSent IS NULL 
						AND (
								(eq.STATUS = 'QUEUED' And eq.RetryCount = 0)
							OR
								(eq.STATUS = 'PROCESSING' And eq.RetryCount > 0)
							)
						AND eq.SendAfter < getdate() 
						AND eq.RetryCount <= @retrycount

						SELECT * FROM dbo.EmailQueue eq WHERE id = @eid";

			using (var command = new SqlCommand(sqlQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@threadname", threadName);
				command.Parameters.AddWithValue("@retrycount", 3);
				var dataReader = command.ExecuteReader();
				while (dataReader.Read())
				{
					emailRows.Add(new EmailQueue()
					{
						Id = Convert.ToInt32(dataReader["Id"]),
						UserId = Convert.ToInt32(dataReader["UserFk"]),
						EmailTemplate = Convert.ToString(dataReader["EmailTemplate"])
					});
				}

				dataReader.Close();
			}

			return emailRows.FirstOrDefault();
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
						break;
						//Thread.CurrentThread.Abort();
						//_tasks.ForEach(t => t.Wait());
					}
					finally
					{
						foreach (var task in _tasks)
						{
							Console.WriteLine($"Finally: Task id {task.Id} status {task.Status}");
						}
						_tokenSource.Dispose();
					}
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