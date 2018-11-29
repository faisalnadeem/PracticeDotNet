using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzSampleFromConfig.Helpers
{
	public class ConsoleOutputHelper
	{

		public static void StopConsoleOutputToFile()
		{
			Console.SetOut(Console.Out);
		}

		public static void StartConsoleOutputToFile()
		{
			var fileName = "C:/sftptemp/ConsoleOutputToFile.txt";
			if (File.Exists(fileName))
				File.Delete(fileName);

			FileStream ostrm;
			StreamWriter writer;
			try
			{
				ostrm = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
				writer = new StreamWriter(ostrm);
			}
			catch (Exception e)
			{
				Console.WriteLine("Cannot open ConsoleOutputToFile.txt for writing");
				Console.WriteLine(e.Message);
				return;
			}

			Console.SetOut(writer);
		}
	}
}
