using System;
using System.Configuration;
using System.IO;
using Microsoft.CSharp.RuntimeBinder;

namespace QuartzSampleFromConfig
{
	public class ApplicationServerEngine
	{
		//private CastleEngine _castleEngine;
		//private NHibernateEngine _nHibernateEngine;
		private IQuartzSchedulerEngine _quartzEngine;
		private IEmailEngine _emailEngine;
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
			
			Console.WriteLine("Starting Email engine...");
			_emailEngine = new EmailEngine(); //_castleEngine.Container.Resolve<IQuartzSchedulerEngine>();
			_emailEngine.Start();
			Console.WriteLine("Email engine started.");

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

			Console.WriteLine("stopping email engine...");
			_emailEngine.Stop();			
			Console.WriteLine("Email engine stopped.");

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