using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
	class MultiThreadingSample
	{
		static void Test0()
		{
			var ts = new CancellationTokenSource();
			CancellationToken ct = ts.Token;
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					// do some heavy work here
					Thread.Sleep(100);
					if (ct.IsCancellationRequested)
					{
						// another thread decided to cancel
						Console.WriteLine("task canceled");
						break;
					}
				}
			}, ct);

			// Simulate waiting 3s for the task to complete
			Thread.Sleep(3000);

			// Can't wait anymore => cancel this task 
			ts.Cancel();
			Console.ReadLine();
		}
	}
}
