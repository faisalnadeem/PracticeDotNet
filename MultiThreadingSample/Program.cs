﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadingSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var st = new SimpleThreadExample();
			st.StartMultipleThread();

			Console.ReadLine();
		}
	}
}
