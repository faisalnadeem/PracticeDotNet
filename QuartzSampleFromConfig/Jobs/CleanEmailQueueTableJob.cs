using System;
using Quartz;
using QuartzSampleFromConfig.Helpers;

namespace QuartzSampleFromConfig.Jobs
{
	public class CleanEmailQueueTableJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Console.WriteLine($"{DateTime.Now}: Starting to clean email queue");
			SqlDataHelper.CleanEmailQueueTable();
			Console.WriteLine($"{DateTime.Now}: Finished cleaning email queue");
		}
	}
}
