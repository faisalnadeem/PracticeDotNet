using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSerialiserTest.Processors
{
	public class NoAlertNotificationEmailProcessor : IEmailProcessor
	{
		private AppSettingHelper _appSettings;

		public void Process(string templateName)
		{
			if (templateName != nameof(NoAlertNotificationEmailModel))
				return;

			UserData user = new UserData();
			var model1 = new NoAlertNotificationEmailModel(user.Email,
				user.BrandedAccount,
				user.FirstName,
				user.CustomerId,
				_appSettings.GetPageNameBy(user.BrandedAccount));

			//send email


		}
	}
}
