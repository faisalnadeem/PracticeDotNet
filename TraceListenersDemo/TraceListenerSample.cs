using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceListenersDemo
{
	public class TraceListenerSample
	{
		public static TextWriterTraceListener CreateTextWriterTraceListener()
		{
			// Create a file for output named TestFile.txt.
			Stream myFile = File.Create("TestFile.txt");

			/* Create a new text writer using the output stream, and add it to
			 * the trace listeners. */
			var textListener = new
				TextWriterTraceListener(myFile);

			return textListener;
		}
	}
}
