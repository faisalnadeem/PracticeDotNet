using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace GenericSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new WindsorContainer();
			container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
			container.Register(Component.For<ICompositionRoot>().ImplementedBy<CompositionRoot>());
			container.Register(Component.For<IConnectionStringProvider>().ImplementedBy<DefaultConnectionStringProvider>());
			container.Register(Component.For<IDbConnectionFactory>().ImplementedBy<SqlConnectionFactory>());
			container.Register(Component.For<ITestGenerics>().ImplementedBy<TestGenerics>());
			//container.Register(Classes.FromAssemblyContaining<ICar>().BasedOn<ICar>().WithService.FromInterface());
			container.Register(Classes.FromThisAssembly().BasedOn<ICar>().WithService.Select(new[] { typeof(ICar) }));
			container.Register(Component.For<ICarAssembler>().ImplementedBy<CarAssembler>());
			var root = container.Resolve<ICompositionRoot>();

			var connectionFactory = container.Resolve<IDbConnectionFactory>();

			var connection = connectionFactory.CreateConnection<SqlConnection>();

			//IEnumerable<ICar> cars = container.Resolve<IEnumerable<ICar>>();
			var carAssembler = container.Resolve<ICarAssembler>();

			var testGeneric = container.Resolve<ITestGenerics>();
			testGeneric.Test();
			testGeneric.TestGeneric();

			var testConnection = new TestDbConnection("Sql");
			testConnection.ReadData();
			testConnection.SaveData();



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

	public interface ICar
	{
		void Assemble();
	}

	public class Merc : ICar{
		public void Assemble()
		{
			throw new NotImplementedException();
		}
	}
	public class Bmw : ICar{
		public void Assemble()
		{
			throw new NotImplementedException();
		}
	}
	public class Audi : ICar{
		public void Assemble()
		{
			throw new NotImplementedException();
		}
	}
}
