using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSerialiserTest.Processors
{
	public interface IEmailProcessor
	{
		void Process(string templateName);
	}
	public class NoddleNoAlertNotificationEmailProcessor : IEmailProcessor
	{
		private AppSettingHelper _appSettings;

		public void Process(string templateName)
		{
			if (templateName != nameof(NoddleNoAlertNotificationEmailModel))
				return;

			UserData user = new UserData();
			var model1 = new NoddleNoAlertNotificationEmailModel(user.Email,
				user.BrandedAccount,
				user.FirstName,
				user.CustomerId,
				_appSettings.GetPageNameBy(user.BrandedAccount));

			//send email


		}
	}

	internal class AppSettingHelper
	{
		public string GetPageNameBy(BrandedAccount? userBrandedAccount)
		{
			return null;
		}
	}
}
