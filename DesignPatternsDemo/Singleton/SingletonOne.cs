using System;

namespace DesignPatternsDemo.Singleton
{
	public class SingletonOne
	{
		public void SayHello()
		{
			Console.WriteLine("Hello singleton one");
		}

		public void DoWork()
		{
			Console.WriteLine("SingletonOne: DoWork");
		}
	}
}
