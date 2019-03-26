using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.Singleton
{
	public class SingletonPrivateContructor
	{
		private SingletonPrivateContructor()
		{

		}

		public static SingletonPrivateContructor GetInstance()
		{
			return new SingletonPrivateContructor();
		}
	}
}
