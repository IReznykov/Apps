using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using System.Drawing;
using System.Drawing.Imaging;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Models
{
	/// <summary>
	/// Common settings for Screen Saver.
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	[Serializable]
	public class Settings : UserSettings, ISettings
	{
		public Settings(IUserSettingsService userSettingsService, IUserSettingsProvider<Settings> userSettingsProvider)
			: base(userSettingsService, userSettingsProvider)
		{
		}

		#region ISettings

		private string _moduleName;
		private SecondaryMonitorType _secondaryMonitorType;
		private BackgroundType _backgroundType;
		private Color _backgroundColor;
		private Bitmap _backgroundImage;

		/// <summary>
		/// Name of the automaton that imitate visualization.
		/// </summary>
		[DefaultValue(null)]
		public string ModuleName
		{
			get { return _moduleName; }
			set { SetProperty(ref _moduleName, value); }
		}

		/// <summary>
		/// Type of the window at secondary monitors.
		/// </summary>
		[DefaultValue(SecondaryMonitorType.Empty)]
		public SecondaryMonitorType SecondaryMonitorType
		{
			get { return _secondaryMonitorType; }
			set { SetProperty(ref _secondaryMonitorType, value); }
		}

		/// <summary>
		/// Background type: image, solid color.
		/// </summary>
		[DefaultValue(BackgroundType.SolidColor)]
		public BackgroundType BackgroundType
		{
			get { return _backgroundType; }
			set { SetProperty(ref _backgroundType, value); }
		}

		/// <summary>
		/// Color of the background.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF000000")]
		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set { SetProperty(ref _backgroundColor, value); }
		}

		/// <summary>
		/// Object with image.
		/// </summary>
		[XmlIgnore]
		public Bitmap BackgroundImage
		{
			get { return _backgroundImage; }
			set { SetProperty(ref _backgroundImage, value); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[XmlElement("BackgroundImage")]
		public byte[] BackgroundImageSerialized
		{
			get
			{
				// serialize
				if (BackgroundImage == null) return null;
				using (var ms = new MemoryStream())
				{
					BackgroundImage.Save(ms, ImageFormat.Bmp);
					return ms.ToArray();
				}
			}
			set
			{
				// deserialize
				if (value == null)
				{
					BackgroundImage = null;
				}
				else
				{
					using (var ms = new MemoryStream(value))
					{
						BackgroundImage = new Bitmap(ms);
					}
				}
			}
		}

		#endregion

	}
}
