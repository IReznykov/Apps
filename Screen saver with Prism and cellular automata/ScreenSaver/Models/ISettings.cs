using System.Drawing;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Models
{
	/// <summary>
	/// Common settings for Screen Saver.
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Name of the automaton that imitate visualization.
		/// </summary>
		string ModuleName { get; set; }

		/// <summary>
		/// Type of the window at secondary monitors.
		/// </summary>
		SecondaryMonitorType SecondaryMonitorType { get; set; }

		/// <summary>
		/// Background type: image, solid color.
		/// </summary>
		BackgroundType BackgroundType { get; set; }

		/// <summary>
		/// Color of the background.
		/// </summary>
		Color BackgroundColor { get; set; }

		/// <summary>
		/// Object with image.
		/// </summary>
		Bitmap BackgroundImage { get; set; }
	}
}