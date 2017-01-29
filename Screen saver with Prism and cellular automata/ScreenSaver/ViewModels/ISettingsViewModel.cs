using System.Collections.ObjectModel;
using Ikc5.AutomataScreenSaver.Models;

namespace Ikc5.AutomataScreenSaver.ViewModels
{
	public interface ISettingsViewModel : ISettings
	{
		ObservableCollection<string> ModuleNameCollection { get; }
	}
}