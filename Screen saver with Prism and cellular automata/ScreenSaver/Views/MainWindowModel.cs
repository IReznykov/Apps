﻿using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using Ikc5.AutomataScreenSaver.Common.Models;
using Ikc5.AutomataScreenSaver.Models;
using Ikc5.AutomataScreenSaver.ViewModels;
using Ikc5.Prism.Settings;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Ikc5.AutomataScreenSaver.Views
{
	public class MainWindowModel : BindableBase, IMainWindowModel
	{
		private readonly IUnityContainer _container;
		private readonly ICommandProvider _commandProvider;
		private IRegionManager _regionManager;
		private IModuleCatalog _moduleCatalog;
		private readonly ILogger _logger;

		public MainWindowModel(IUnityContainer container, ISettings settings, ILogger logger)
		{
			container.ThrowIfNull(nameof(container));
			_container = container;
			_commandProvider = _container.Resolve<ICommandProvider>();

			settings.ThrowIfNull(nameof(settings));
			Settings = settings;

			_logger = logger;

			SettingsCommand = new DelegateCommand(ExecuteSettings);
			StatisticsCommand = new DelegateCommand(ExecuteStatistics);
			RestartCommand = new DelegateCommand(
				() => _commandProvider.RestartCommand.Execute(null),
				() => _commandProvider.RestartCommand.CanExecute(null));
			AboutCommand = new DelegateCommand(ExecuteAbout);
		}

		#region IMainWindowModel

		private ISettings _settings;

		/// <summary>
		/// Main window settings.
		/// </summary>
		public ISettings Settings
		{
			get { return _settings; }
			private set
			{
				var userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged -= UserSettingsOnPropertyChanged;

				SetProperty(ref _settings, value);
				userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged += UserSettingsOnPropertyChanged;
			}
		}

		private void UserSettingsOnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
			case nameof(Settings.ModuleName):
				SetActiveView();
				break;

			default:
				return;
			}
		}

		/// <summary>
		/// Command shows settings dialog.
		/// </summary>
		public ICommand SettingsCommand { get; }

		/// <summary>
		/// Command shows statistics dialog.
		/// </summary>
		public ICommand StatisticsCommand { get; }

		/// <summary>
		/// Command that reinit automaton.
		/// </summary>
		public ICommand RestartCommand { get; }

		/// <summary>
		/// Command shows about dialog.
		/// </summary>
		public ICommand AboutCommand { get; }

		private bool _showStatistics = false;

		public bool ShowStatistics
		{
			get { return _showStatistics; }
			set { SetProperty(ref _showStatistics, value); }
		}

		#endregion IMainWindowModel

		private void ExecuteSettings()
		{
			var settingsWindow = _container.Resolve<SettingsWindow>();
			settingsWindow.Owner = Application.Current.MainWindow;
			settingsWindow.ShowDialog();
		}

		private void ExecuteStatistics()
		{
			// OxyPlot failed in Dialog Window
			//var statisticsWindow = _container.Resolve<StatisticsWindow>();
			//statisticsWindow.Owner = Application.Current.MainWindow;
			//statisticsWindow.ShowDialog();
		}

		private void ExecuteAbout()
		{
			var aboutWindow = _container.Resolve<AboutWindow>();
			aboutWindow.Owner = Application.Current.MainWindow;
			aboutWindow.ShowDialog();
		}

		/// <summary>
		/// Make view from Settings.ModuleName active in the main region.
		/// </summary>
		private void SetActiveView()
		{
			if (_regionManager == null)
			{
				_regionManager = _container.Resolve<IRegionManager>();
				_moduleCatalog = _container.Resolve<IModuleCatalog>();
			}
			var message = $"Active view should be changed to view from module {Settings.ModuleName} in region {PrismNames.MainRegionName}.";
			// get main region
			var region = _regionManager.Regions.ContainsRegionWithName(PrismNames.MainRegionName)
							? _regionManager.Regions[PrismNames.MainRegionName] : null;
			if (region == null)
			{
				_logger?.Log($"{message} Region was not found.", Category.Error);
				return;
			}
			// get module by name
			var moduleInfo = _moduleCatalog.Modules.FirstOrDefault(module => module.ModuleName.Equals(Settings.ModuleName));
			if (moduleInfo == null)
			{
				_logger?.Log($"{message} Module was not found.", Category.Error);
				return;
			}
			// get assembly for module
			var pos = moduleInfo.ModuleType.IndexOf(',');
			var assemblyName = pos == -1 ? string.Empty : moduleInfo.ModuleType.Substring(pos + 1).Trim();
			foreach (var view in region.Views)
			{
				var type = view.GetType();
				if (type.Assembly.FullName.Equals(assemblyName))
				{
					region.Activate(view);
					return;
				}
			}
			_logger?.Log($"{message} View from mentioned module was not found.", Category.Error);
		}
	}
}