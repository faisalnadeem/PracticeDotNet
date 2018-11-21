using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.CSharp.RuntimeBinder;
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

	public class ApplicationServerEngine
	{
		//private CastleEngine _castleEngine;
		//private NHibernateEngine _nHibernateEngine;
		private IQuartzSchedulerEngine _quartzEngine;
		//private IWcfServerEngine _wcfServerEngine;
		private FileSystemWatcher _watcher;
		//public HttpSelfHostConfiguration ApiConfiguration { get; set; }

		public virtual void Start()
		{
			//_castleEngine = new CastleEngine();
			//_castleEngine.Start();
			//var logger = _castleEngine.Container.Resolve<ILogger>();
			//Console.WriteLine("Castle engine started.");

			//var configEncryptionAppSettings = _castleEngine.Container.Resolve<IConfigEncryptionAppSettings>();
			//if (configEncryptionAppSettings.Enabled)
			//{
			//	var configEncryptor = _castleEngine.Container.Resolve<IConfigEncryptor>();
			//	var whatToEncrypt = WhatToEncryptBuilder.Define()
			//		.ChoseEnvironment(WhatToEncrypt.EnvironmentType.Server)
			//		.EncryptTopLevelSection("appSettings")
			//		.EncryptTopLevelSection("connectionStrings")
			//		.EncryptNestedSection("system.serviceModel", "client")
			//		.EncryptNestedSection("system.net", "smtp")
			//		.Done();

			//	configEncryptor.Encrypt(whatToEncrypt);
			//	AppConfigHelper.ClearCachedConfigSettingsFromMemory();
			//}

			//var wcfServerEngineSettings = _castleEngine.Container.Resolve<IWcfServerEngineAppSettings>();
			//if (wcfServerEngineSettings.Enabled)
			//{
			//	Console.WriteLine("Starting WCF services...");
			//	_wcfServerEngine = new WcfServerEngine(_castleEngine.Container, logger, wcfServerEngineSettings);
			//	_wcfServerEngine.Start();
			//	Console.WriteLine("WCF services started.");
			//}
			//else
			//{
			//	Console.WriteLine("WCF engine skipped.");
			//}

			// this is workaround especially for RazorEngine which is used to rendering email in Mailer
			// becouse it seems that the Microsoft.CSharp.dll is delay-loaded when dynamic is actually used 
			// (not declared, but used effectively in the code).
			//
			// if you want more information plase look at http://razorengine.codeplex.com/discussions/242605
			// ReSharper disable UnusedVariable
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			bool loaded = typeof(Binder).Assembly != null;
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
			// ReSharper restore UnusedVariable
			Console.WriteLine("MicrosoftCSharpDll loaded.");

			//Console.WriteLine("Starting NHibernate engine...");
			//_nHibernateEngine = new NHibernateEngine();
			//_nHibernateEngine.Start();
			//Console.WriteLine("NHibernate engine started.");

			// This overwrites default dependency resolver used by System.Web.Http.SelfHost library (that is used to host API).
			//if (ApiConfiguration != null)
			//	ApiConfiguration.DependencyResolver = new WindsorHttpDependencyResolver(_castleEngine.Container);

			Console.WriteLine("Starting Quartz engine...");
			_quartzEngine = new QuartzSchedulerEngine(); //_castleEngine.Container.Resolve<IQuartzSchedulerEngine>();
			_quartzEngine.Start();
			Console.WriteLine("Quartz engine started.");

			_watcher = new FileSystemWatcher
			{
				Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
				NotifyFilter = NotifyFilters.LastWrite,
				Filter = "AppSettings.config",
				EnableRaisingEvents = true
			};

			_watcher.Changed += OnWatcherOnChanged;
			Console.WriteLine("AppSettings.config file is monitored for changes.");

			//ILog mailingLogger = LogManager.GetLogger("MultiLogger");
			//mailingLogger.LogInfoWithBasicAmbient("Server started.");
		}

		private void OnWatcherOnChanged(object sender, FileSystemEventArgs args)
		{
			ConfigurationManager.RefreshSection("appSettings");
			//var logger = _castleEngine.Container.Resolve<ILogger>();
			Console.WriteLine("AppSettings.config settings reloaded.");
		}

		public virtual void Stop()
		{
			//var logger = _castleEngine.Container.Resolve<ILogger>();
			//_watcher.Changed -= OnWatcherOnChanged;
			//_watcher.Dispose();
			//Console.WriteLine("Stoped watching appSettings");
			Console.WriteLine("stopping Quartz engine...");
			_quartzEngine.Stop();
			Console.WriteLine("Quartz engine stopped.");

			//Console.WriteLine("stopping NHibernate engine...");
			//_nHibernateEngine.Stop();
			//Console.WriteLine("NHibernate engine stopped.");

			//Console.WriteLine("stopping Castle engine...");
			//_castleEngine.Stop();
			//Console.WriteLine("Castle engine stopped.");

			//ILog mailingLogger = LogManager.GetLogger("MultiLogger");
			//mailingLogger.LogInfoWithBasicAmbient("Server stopped.");
		}
	}
}
