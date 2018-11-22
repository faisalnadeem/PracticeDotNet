using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzSampleFromConfig.Helpers
{
	public class EmailHelper
	{
		public static void SendEmail()
		{
			Console.WriteLine($"{DateTime.Now}: Email sent...");
		}

	}
}
