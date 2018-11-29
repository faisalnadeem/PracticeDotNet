using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceListenersDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			
			Trace.Listeners.Add(TraceListenerSample.CreateTextWriterTraceListener());

			// Write output to the file.
			Trace.Write("Test output ");
			for(var i=0; i<1000; i++)
				Trace.WriteLine($"Line number {i}");

			// Flush the output.
			Trace.Flush();

		}
	}
}
