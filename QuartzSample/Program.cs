using System;
using System.Configuration;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

//using Topshelf;

namespace QuartzSample
{
    class Program
    {
        static void Main(string[] args)
        {

            //HostFactory.Run(x =>
            //{
            //    x.RunAsLocalSystem();

            //    x.SetDescription(Configuration.ServiceDescription);
            //    x.SetDisplayName(Configuration.ServiceDisplayName);
            //    x.SetServiceName(Configuration.ServiceName);

            //    x.Service(factory =>
            //    {
            //        QuartzServer server = QuartzServerFactory.CreateServer();
            //        server.Initialize().GetAwaiter().GetResult();
            //        return server;
            //    });
            //});
            var schedulerFactory = new StdSchedulerFactory();

            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.Start();
            var jobDetail = JobBuilder.Create<ConsoleWriterJob>().WithIdentity("job-wr", "writeconsole").Build();
            var trigger =
                TriggerBuilder.Create().WithIdentity("job-wr-trigger", "writeconsole")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                    .Build();
            scheduler.ScheduleJob(jobDetail, trigger);

            var addJobDetail = JobBuilder.Create<AddNumbersJob>().WithIdentity("add-numbers-job", "arithmatics").Build();
            var addJobTrigger =
                TriggerBuilder.Create().WithIdentity("add-numbers-job-trigger", "arithmatics")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).RepeatForever())
                    .Build();
            scheduler.ScheduleJob(addJobDetail, addJobTrigger);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            scheduler.Shutdown();
        }
    }

    public class ConsoleWriterJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return WriteLineTask();
        }

        private static async Task WriteLineTask()
        {
            Console.WriteLine($"Time now {DateTime.Now}");
            await Task.Delay(TimeSpan.FromSeconds(5));
            return;
        }
    }
    public class AddNumbersJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return AddTask(2, 3);
        }

        private static async Task<int> AddTask(int x, int y)
        {
            Console.WriteLine($"Adding numbers at {DateTime.Now}: {x} + {y} : {x + y}");
            await Task.Delay(TimeSpan.FromSeconds(3));
            return x + y;
        }
    }

}
