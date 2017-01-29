using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Ikc5.AutomataScreenSaver.Common.Models;
using Ikc5.AutomataScreenSaver.Logging;
using Ikc5.AutomataScreenSaver.Models;
using Ikc5.AutomataScreenSaver.ViewModels;
using Ikc5.AutomataScreenSaver.Views;
using Ikc5.Prism.Common.Logging;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Providers;
using Ikc5.Prism.Settings.Services;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using LoggerFacadeAdapter = Ikc5.Prism.Common.Logging.LoggerFacadeAdapter;

namespace Ikc5.AutomataScreenSaver
{
	public class Bootstrapper : UnityBootstrapper
	{
		/// <summary>
		/// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
		/// type mappings required by the application.
		/// </summary>
		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
			// create singleton of UserSettingsService
			Container
				.RegisterType<IUserSettingsService, UserSettingsService>(new ContainerControlledLifetimeManager())
				.RegisterType<ILiteObjectService, LiteObjectService>(new ContainerControlledLifetimeManager())
				.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager())
				.RegisterType<IMainWindowModel, MainWindowModel>()
				.RegisterType<IEmptyWindowModel, EmptyWindowModel>()
				.RegisterType<ILogger, LoggerFacadeAdapter>()
				.RegisterType<ICommandProvider, CommandProvider>(new ContainerControlledLifetimeManager());

			Container.RegisterType(
				typeof(IUserSettingsProvider<>),
				typeof(PersonalXmlUserSettingsProvider<>),
				new ContainerControlledLifetimeManager());
		}

		/// <summary>
		/// Creates the shell or main window of the application.
		/// </summary>
		/// <returns>The shell of the application.</returns>
		/// <remarks>
		/// If the returned instance is a <see cref="DependencyObject"/>, the
		/// <see cref="Bootstrapper"/> will attach the default <seealso cref="IRegionManager"/> of
		/// the application in its <see cref="RegionManager.RegionManagerProperty"/> attached property
		/// in order to be able to add regions by using the <seealso cref="RegionManager.RegionNameProperty"/>
		/// attached property from XAML.
		/// </remarks>
		protected override DependencyObject CreateShell()
		{
			Window mainWindow = null;
			try
			{
				mainWindow = Container.Resolve<MainWindow>();
			}
			catch (Exception ex)
			{
				var logger = Container.Resolve<ILogger>();
				logger?.Exception(ex);
			}
			return mainWindow;
		}

		/// <summary>
		/// Initializes the shell.
		/// </summary>
		/// <remarks>
		/// The base implemention ensures the shell is composed in the container.
		/// </remarks>
		protected override void InitializeShell()
		{
			// change default mappings to mapping to view model in the same folder/namespace
			// view should be Something.ThisView, and view model is Something.ThisViewModel
			// in the assembly
			ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
				viewType =>
				{
					var viewName = viewType.FullName;
					var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
					var viewModelName = string.Format(CultureInfo.InvariantCulture, $"{viewName}Model, {viewAssemblyName}");
					return Type.GetType(viewModelName);
				});

			// add some views to region adapter
			var regionManager = Container.Resolve<IRegionManager>();
			regionManager.RegisterViewWithRegion(RegionNames.AppSettingsRegion, typeof(SettingsView));

			// don't show window now - application may runs in settings mode
			//Application.Current.MainWindow.Show();
		}

		protected override void ConfigureModuleCatalog()
		{
			var catalog = (ModuleCatalog)ModuleCatalog;
			// add all catalogs that will contain different types of automata
			catalog.AddModule("Cellular Automaton", typeof(Life.LifeModule).AssemblyQualifiedName);
		}

		protected override ILoggerFacade CreateLogger()
		{
			return new Log4NetLogger();
		}
	}
}
