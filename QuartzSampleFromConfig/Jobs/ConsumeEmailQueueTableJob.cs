using Quartz;

namespace QuartzSampleFromConfig.Jobs
{
	public class ConsumeEmailQueueTableJob: IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			SelectUpdateInlineTransaction.RunTasks();			
		}	
	}
}
