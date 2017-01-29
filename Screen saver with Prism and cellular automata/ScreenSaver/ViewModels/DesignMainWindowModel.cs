using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Models;

namespace Ikc5.AutomataScreenSaver.ViewModels
{
	public class DesignMainWindowModel : IMainWindowModel
	{
		public ISettings Settings { get; } = null;
		public ICommand SettingsCommand { get; } = null;
		public ICommand StatisticsCommand { get; } = null;
		public ICommand RestartCommand { get; } = null;
		public ICommand AboutCommand { get; } = null;
		public bool ShowStatistics { get; } = true;
	}
}
