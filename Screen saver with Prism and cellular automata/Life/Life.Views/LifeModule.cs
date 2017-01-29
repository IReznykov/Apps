using System.ComponentModel;
using Ikc5.AutomataScreenSaver.Common.Models;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.AutomataScreenSaver.Life.Views;
using Ikc5.Math.CellularAutomata;
using Ikc5.Prism.Settings;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Ikc5.AutomataScreenSaver.Life
{
	[Description("Cellular automaton")]
	public class LifeModule : IModule
	{
		private readonly IRegionManager _regionManager;

		private readonly IUnityContainer _container;

		public LifeModule(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
			ConfigureContainer();
		}

		private void ConfigureContainer()
		{
			_container.
				RegisterType<ICell, Cell>().
				RegisterType<IAutomaton, Automaton>().
				RegisterType<ICellViewModel, CellViewModel>().
				RegisterType<IAutomatonViewModel, AutomatonViewModel>().
				RegisterType<ICellLifeService, MooreCellLifeService>().
				RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager()).
				RegisterType<IChartRepository, ChartRepository>(new ContainerControlledLifetimeManager());

			_container.RegisterInstance<ILifePreset>(KnownLifePresets.Life);
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion(PrismNames.MainRegionName, typeof(AutomatonView));
			_regionManager.RegisterViewWithRegion($"{GetType().Name}{RegionNames.ModuleSettingsRegion}", typeof(SettingsView));

			for (var pos = 0; pos < 3; pos++)
			{
				var index = pos;
				var regionName = $"{PrismNames.SummarizeRegionName}{index}";
				_regionManager.RegisterViewWithRegion(regionName, () => CreateSummarizeView(index));
			}
		}

		private object CreateSummarizeView(int pos)
		{
			switch (pos)
			{
			case 0:
				{
					var view = _container.Resolve<AgeChartView>();
					return view;
				}

			case 1:
				{
					var view = _container.Resolve<CountChartView>();
					return view;
				}

			default:
				return null;
			}
		}

	}
}
