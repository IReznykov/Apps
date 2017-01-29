using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Models;

namespace Ikc5.AutomataScreenSaver.ViewModels
{
	public interface IMainWindowModel
	{
		/// <summary>
		/// Main window settings.
		/// </summary>
		ISettings Settings { get; }

		/// <summary>
		/// Command shows settings dialog.
		/// </summary>
		ICommand SettingsCommand { get; }

		/// <summary>
		/// Command shows statistics dialog.
		/// </summary>
		ICommand StatisticsCommand { get; }

		/// <summary>
		/// Command that reinit automaton.
		/// </summary>
		ICommand RestartCommand { get; }

		/// <summary>
		/// Command shows about dialog.
		/// </summary>
		ICommand AboutCommand { get; }

		/// <summary>
		/// Charts are shown if value equals TRUE.
		/// </summary>
		bool ShowStatistics { get; }
	}
}