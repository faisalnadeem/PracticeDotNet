using System;
using Quartz;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig.Jobs
{
	public class PopulatingEmailQueueTableJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Console.WriteLine($"{DateTime.Now}: Starting to populate email queue");
			SqlDataHelper.PopulateEmailQueueTable();
			Console.WriteLine($"{DateTime.Now}: Finished populating email queue");
		}
	}
}
