using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace QuartzSampleFromConfig
{
	class Program
	{
		static void Main(string[] args)
		{
			var runner = new ApplicationServerHost(new ApplicationServerEngine());
			runner.Execute();
			Console.WriteLine("Press any key to exit...");
			Console.ReadLine();
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
