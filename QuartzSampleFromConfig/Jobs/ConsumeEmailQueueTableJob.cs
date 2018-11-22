using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Quartz;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig.Jobs
{
	public class ConsumeEmailQueueTableJob: IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			StartThreads();

			//var task1 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-1", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task2 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-2", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task3 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-3", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task4 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-4", SqlDataHelper.GetNextUserIdToSendEmail()));
			//var task5 = Task.Factory.StartNew(() => ConsumeEmailRow("Thread-5", SqlDataHelper.GetNextUserIdToSendEmail()));

			//Task.WaitAll(task1, task2, task3, task4, task5);
			//Console.WriteLine("All task finished");
		}

		private void StartThreads()
		{
			var t3 = new Thread(emailRow =>
			{
				var numberOfSeconds = 0;
				while (emailRow != null) //< Convert.ToInt32(p))
				{
					Console.WriteLine("T3: Sleeping for 0.5 second");
					Thread.Sleep(500);
					numberOfSeconds++;

					emailRow = SqlDataHelper.GetNextUserIdToSendEmail();
				}

				Console.WriteLine("T3: I ran for {0} seconds", numberOfSeconds);
			});

			t3.Start(SqlDataHelper.GetNextUserIdToSendEmail());

		}

		private void ConsumeEmailRow(string threadName, EmailQueue emailRow)
		{
			//using (var scope = new TransactionScope())
			//{
				if (emailRow == null) return;
				Console.WriteLine( $"{DateTime.Now}: {threadName} Sending email to user Id: {emailRow.UserId}");
				//EmailHelper.SendEmail();
				Console.WriteLine( $"{DateTime.Now}: {threadName} Email sent to user Id: {emailRow.UserId}");
				Console.WriteLine($"{DateTime.Now}: {threadName} Updatnig row Id:{emailRow.Id} user Id:{emailRow.UserId}");
				SqlDataHelper.MarkAsConsumedByEmailQueueId(emailRow.Id);
				Console.WriteLine($"{DateTime.Now}: {threadName} Done updating row Id:{emailRow.Id} user Id: {emailRow.UserId}");
			//	scope.Complete();
			//}
		}
	}
}
