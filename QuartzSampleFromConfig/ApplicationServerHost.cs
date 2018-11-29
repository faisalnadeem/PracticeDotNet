using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.ServiceConfigurators;

namespace QuartzSampleFromConfig
{
	public class ApplicationServerHost
	{
		private ApplicationServerEngine _appServerEngine;

		public ApplicationServerHost(ApplicationServerEngine appServerEngine)
		{
			_appServerEngine = appServerEngine;
		}

		public void Execute()
		{
			var host = HostFactory.New(Configure);

			//string apiBaseAddress = ConfigurationManager.AppSettings["Api_BaseAddress"];
			//var apiConfig = new HttpSelfHostConfiguration(apiBaseAddress);
			//apiConfig.Routes.MapHttpRoute(
			//	name: "API",
			//	routeTemplate: "{controller}/{action}/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//	);
			//_appServerEngine.ApiConfiguration = apiConfig;

			//var apiServer = new HttpSelfHostServer(apiConfig);
			//apiServer.OpenAsync().Wait();
			host.Run();
		}

		protected virtual void Configure(HostConfigurator confRunner)
		{
			string prefix = "QuartzSample"; //ConfigurationManager.AppSettings["Service_DisplayPrefix"];
			Action<ServiceConfigurator<ApplicationServerEngine>> behaviour = ConfigureBehaviour;
			confRunner.Service(behaviour);
			confRunner.RunAsLocalSystem();
			confRunner.SetDescription(prefix + " Application Server");
			confRunner.SetDisplayName(prefix + " Application Server");
			confRunner.SetServiceName("QuartsampelDebug"); //ConfigurationManager.AppSettings["Service_ServiceName"]);
		}

		protected virtual void ConfigureBehaviour(ServiceConfigurator<ApplicationServerEngine> behaviour)
		{
			behaviour.ConstructUsing(service => _appServerEngine);
			behaviour.WhenStarted(
				appServ =>
				{
					try
					{
						appServ.Start();
						//var logger = ServiceLocator.GetInstance<ILogger>();
						Console.WriteLine("Service started");
					}
					catch (Exception ex)
					{
						LogManager.GetLogger("Default").Error(
							"Error during Application Server initialization: ",
							ex);

						// HACK:Nie chcemy czekać 10 minut, jeśli w czasie ładowania serwera wyskoczy błąd, dlatego, jeśli wyskoczy - 
						// ubijamy proces. W logach jest oczywiście odpowiedni wpis
						Process.GetCurrentProcess().Kill();
					}
				});

			behaviour.WhenStopped(
				appServ =>
				{
					//var logger = ServiceLocator.GetInstance<ILogger>();
					Console.WriteLine("Service is stopping...");
					appServ.Stop();
					Console.WriteLine("Service stopped");
				});
		}
	}
}
