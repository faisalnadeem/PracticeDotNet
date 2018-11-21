using System.Collections.Generic;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Simpl;
using Quartz.Spi;

namespace QuartzSampleFromConfig
{
	public interface IQuartzSchedulerEngine
	{
		List<string> GetRunningJobs();
		void Start();
		void Stop();
	}

	public class QuartzSchedulerEngine : IQuartzSchedulerEngine
	{
		private readonly IScheduler _scheduler;

		public QuartzSchedulerEngine():this(new SimpleJobFactory(), new JobChainingJobListener("joblistener")){ }
		public QuartzSchedulerEngine(
			IJobFactory jobFactory,
			IJobListener jobListener)
		{
			var schedulerFactory = new StdSchedulerFactory();
			_scheduler = schedulerFactory.GetScheduler();
			_scheduler.JobFactory = jobFactory;
			_scheduler.ListenerManager.AddJobListener(jobListener);
		}

		public void Start()
		{
			_scheduler.Start();
		}

		public void Stop()
		{
			_scheduler.Shutdown();
		}

		public List<string> GetRunningJobs()
		{
			var currentJobs = _scheduler.GetCurrentlyExecutingJobs();
			var jobsNames = currentJobs.Select(job => job.JobDetail.Key.Name).ToList();

			return jobsNames;
		}
	}
}
