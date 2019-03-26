using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DesignPatternsDemo.Singleton
{

	[TestFixture]
	public class SingletonClient
	{
		[Test]
		public void TestSingletonOne()
		{
			var s1 = new SingletonOne();
			var s2 = new SingletonOne();
			
			Console.WriteLine("not a singleton");

			Assert.AreNotEqual(s1, s2);
		}

		[Test]
		public void TestSingletonPrivateConstructor()
		{
			//compile time error
			//var s1 = new SingletonPrivateContructor();
			//var s2 = new SingletonPrivateContructor();

			SingletonPrivateContructor s1 = null;
			SingletonPrivateContructor s2 = null;
			Assert.AreEqual(s1, s2);

		}
		
		[Test]
		public void TestSingletonPrivateConstructor1()
		{
			var s1 = SingletonPrivateContructor.GetInstance();
			var s2 = SingletonPrivateContructor.GetInstance();
			Assert.AreEqual(s1, s2);

		}
	}	
}
