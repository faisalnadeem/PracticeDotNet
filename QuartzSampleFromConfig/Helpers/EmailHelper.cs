using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzSampleFromConfig.Helpers
{
	public class EmailHelper
	{
		public static void SendEmail(string threadName, EmailQueue emailRow)
		{
			try
			{
				Thread.Sleep(300);
				var messageForException = emailRow.Id % 17 == 0 ? "throwexceptionrequest" : "";
				EmailManager.Local.EmailManager.SendEmail($"{DateTime.Now}: Email sent UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}.{messageForException}");
				Console.WriteLine($"{DateTime.Now}: Email sent UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}");
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				throw;
			}
		}
		public static void SendEmailWithError(string threadName, EmailQueue emailRow)
		{
			Thread.Sleep(1000);
			if (emailRow.Id % 9 == 0)
			{
				Trace.WriteLine($"{DateTime.Now}: Error occured sending email UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}");
				SqlDataHelper.MarkAsErroredByEmailQueueId(emailRow.Id);
			}
			else
			{
				Trace.WriteLine($"{DateTime.Now}: Email sent UserId:{emailRow.UserId} Thread:{threadName} EmailId {emailRow.Id}");
			}
		}
	}
}
