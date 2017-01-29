using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Models;
using Ikc5.AutomataScreenSaver.ViewModels;
using Ikc5.Prism.Settings;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;

namespace Ikc5.AutomataScreenSaver.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class EmptyWindow
	{
		private readonly ILogger _logger;

		public EmptyWindow(ILogger logger, IEmptyWindowModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
			Loaded += EmptyWindow_Loaded;

			logger.ThrowIfNull(nameof(logger));
			_logger = logger;

		}

		private void EmptyWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_logger.Log($"EmptyWindow OnLoaded, IsVisible={IsVisible}, IsActive={IsActive}");
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_logger.Log("Empty window - OnKeyDown method");
			Application.Current.Shutdown();
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			_logger.Log("Empty window - OnMouseLeftButtonDown method");
			Application.Current.Shutdown();
		}
	}
}
