using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig
{
	public class SelectUpdateInOneTransaction
	{
		//static string _connectionString = @"Server=PLLLP9435;initial catalog=EmailPoc;Integrated Security=true";
		public static void RunTasks()
		{

			var task1 = Task.Factory.StartNew(() => ConsumeQueue("T-1"));
			var task2 = Task.Factory.StartNew(() => ConsumeQueue("T-2"));
			var task3 = Task.Factory.StartNew(() => ConsumeQueue("T-3"));
			var task4 = Task.Factory.StartNew(() => ConsumeQueue("T-4"));
			var task5 = Task.Factory.StartNew(() => ConsumeQueue("T-5"));

			Task.WaitAll(task1, task2, task2, task3, task4, task5);
			Console.WriteLine("All task finished");

		}


		private static void ConsumeQueue(string threadName)
		{
			var emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmail(threadName);
			while (emailRow != null)
			{
				emailRow = SqlDataHelper.SelectAndUpdateNextUserIdToSendEmail(threadName);
				Console.WriteLine($"{DateTime.Now}: Sending email UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id} s");
				EmailHelper.SendEmail(threadName, emailRow);
			}
		}
	}
}
