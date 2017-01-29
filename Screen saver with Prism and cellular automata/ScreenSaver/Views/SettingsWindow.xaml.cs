using System.Windows;
using Ikc5.Prism.Settings;
using Ikc5.TypeLibrary;

namespace Ikc5.AutomataScreenSaver.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		private readonly IUserSettingsService _userSettingsService;

		public SettingsWindow(IUserSettingsService userSettingsService)
		{
			userSettingsService.ThrowIfNull(nameof(userSettingsService));
			_userSettingsService = userSettingsService;

			InitializeComponent();
			CommandGrid.DataContext = _userSettingsService;
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SaveButtonClick(object sender, RoutedEventArgs e)
		{
			// serialize settings
			var serializedData = new object();

			if (_userSettingsService.SaveCommand.CanExecute(serializedData))
			{
				_userSettingsService.SaveCommand.Execute(serializedData);
				if (_userSettingsService.SerializeCommand.CanExecute(serializedData))
					_userSettingsService.SerializeCommand.Execute(serializedData);
			}

			Close();
		}
	}
}
