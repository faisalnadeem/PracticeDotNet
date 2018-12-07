using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using GenericSample;

namespace QuartzSampleFromConfig
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new WindsorContainer();
			container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());
			container.Register(Component.For<IConnectionStringProvider>().ImplementedBy<DefaultConnectionStringProvider>());
			container.Register(Component.For<IDbConnectionFactory>().ImplementedBy<SqlConnectionFactory>());
			container.Register(Component.For<ITestGenerics>().ImplementedBy<TestGenerics>());
			var root = container.Resolve<ICompositionRoot>();

			var connectionFactory = container.Resolve<IDbConnectionFactory>();

			var connection = connectionFactory.CreateConnection<SqlConnection>();

			var testGeneric = container.Resolve<ITestGenerics>();
			testGeneric.Test();
			testGeneric.TestGeneric();

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}

	public class CompositionRoot : ICompositionRoot
	{
		// We will do fancier stuff here in just a bit...
	}

	public interface ICompositionRoot
	{
	}
}
