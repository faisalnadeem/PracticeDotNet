using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var schedulerFactory = new StdSchedulerFactory();
	        ISchedulerFactory schedFact = new StdSchedulerFactory();

			// get a scheduler
			IScheduler sched = schedFact.GetScheduler();
			sched.Start();

			//var scheduler = schedulerFactory.GetScheduler().Result;
			//scheduler.Start();


			//var jobDetail = JobBuilder.Create<ConsoleWriterJob>().WithIdentity("job-add", "arithmatics").Build();
			//var trigger =
			//    TriggerBuilder.Create().WithIdentity("job-add-trigger", "arithmatics")
			//        .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
			//        .Build();

			//scheduler.ScheduleJob(jobDetail, trigger);
			Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            //scheduler.Shutdown();
        }
    }

    public class ConsoleWriterJob : IJob 
    {
        //public Task Execute(IJobExecutionContext context)
        //{
        //    return AddTask(2, 3);
        //}

        private int AddTask(int x, int y)
        {
            Console.WriteLine($"Adding numbers at {DateTime.Now}: {x} + {y} : {x + y}");
            //await Task.Delay(TimeSpan.FromSeconds(5));
            return x + y;
        }

		void IJob.Execute(IJobExecutionContext context)
		{
			AddTask(2, 3);
		}
	}

}
