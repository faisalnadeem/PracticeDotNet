using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.ServiceModel;

namespace EmailManager.Local
{
	public class EmailManager
	{
		public static void SendEmails(int emailCount)
		{
			for (var i = 0; i < emailCount; i++)
			{
				SendEmail("Hi this is an email manager testing email message no " + i);
			}

		}

		static string emailDirectory = @"c:\TempEmails";

		public static void SendEmail(string message)
		{
			var smtpClient = new SmtpClient
			{
				DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory
			};


			if (!Directory.Exists(emailDirectory))
			{
				Directory.CreateDirectory(emailDirectory);
			}

			smtpClient.PickupDirectoryLocation = emailDirectory;

			var emailMessage = new MailMessage
			{
				Body = message,
				To = {"testmail@my.email"},
				From = new MailAddress("noreplay@to.localhost.com")
			};

			if (message.Contains("throwexceptionrequest"))
			{
				Trace.WriteLine("EmailManager: throwing exception as per request for this email.");
				throw new CommunicationException();
			}

			smtpClient.Send(emailMessage);
		}

		public static void DeleteTempEmailDirectory()
		{
			if (Directory.Exists(emailDirectory))
			{
				Directory.Delete(emailDirectory);
			}

		}
	}
}