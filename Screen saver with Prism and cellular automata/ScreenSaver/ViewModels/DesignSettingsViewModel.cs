using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.ViewModels
{
	public class DesignSettingsViewModel : ISettingsViewModel
	{
		public DesignSettingsViewModel()
		{
			ModuleNameCollection = new ObservableCollection<string>(new[] { "Module1", "Module2", "Module3" });
		}

		public ObservableCollection<string> ModuleNameCollection { get; }
		public string ModuleName { get; set; } = "Module1";
		public SecondaryMonitorType SecondaryMonitorType { get; set; } = SecondaryMonitorType.Empty;
		public BackgroundType BackgroundType { get; set; } = BackgroundType.SolidColor;
		public Color BackgroundColor { get; set; } = Colors.Black;
		public Bitmap BackgroundImage { get; set; } = null;
	}
}
