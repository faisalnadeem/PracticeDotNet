using System;
using System.Threading;

namespace QuartzSampleFromConfig
{
	public interface IEmailEngine
	{
		void Start();
		void Stop();
	}
	public class EmailEngine : IEmailEngine
	{	
		public void Start()
		{
			Console.WriteLine("Welcome to email engine");
			EmailEngineOrchestrator.StartEmailEngineThreads();
			//SelectUpdateInlineTransaction.RunTasksWitCancellationToken();
		}

		public void Stop()
		{
			EmailEngineOrchestrator.StopTasksWithCancellationToken();
		}
	}
}
