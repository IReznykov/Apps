using System.Drawing;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.Math.CellularAutomata;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.ViewModels;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	/// <summary>
	/// View model for module's settings
	/// https://ireznykov.com/2016/10/16/examples-of-using-ikc5-prism-settings/
	/// https://ireznykov.com/2016/10/15/nuget-package-ikc5-prism-settings/
	/// </summary>
	public class SettingsViewModel : UserSettingsViewModel<ISettings>, ISettingsViewModel
	{
		public SettingsViewModel(ISettings settingsModel, IUserSettingsService userSettingsService)
			: base(settingsModel as IUserSettings, userSettingsService)
		{
		}

		#region ISettings

		// automaton settings
		private int _width;
		private int _height;
		private KnownLifePreset _knownLifePreset;
		private int _iterationDelay;
		private InitialAutomatonType _initialAutomatonType;

		// cell settings
		private Color _startColor;
		private Color _finishColor;
		private Color _borderColor;
		private Color _cellColor;
		private bool _showBorder;
		private bool _showAge;
		private AnimationType _animationType;
		private int _animationDelay;

		#endregion ISettings

		#region ISettingsViewModel

		/// <summary>
		/// Width of automaton.
		/// </summary>
		public int Width
		{
			get { return _width; }
			set { SetProperty(ref _width, value); }
		}

		/// <summary>
		/// Height of automaton.
		/// </summary>
		public int Height
		{
			get { return _height; }
			set { SetProperty(ref _height, value); }
		}

		/// <summary>
		/// Life model.
		/// </summary>
		public KnownLifePreset KnownLifePreset
		{
			get { return _knownLifePreset; }
			set { SetProperty(ref _knownLifePreset, value); }
		}

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		public int IterationDelay
		{
			get { return _iterationDelay; }
			set { SetProperty(ref _iterationDelay, value); }
		}

		/// <summary>
		/// Initial automaton type.
		/// </summary>
		public InitialAutomatonType InitialAutomatonType
		{
			get { return _initialAutomatonType; }
			set { SetProperty(ref _initialAutomatonType, value); }
		}

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>
		public Color StartColor
		{
			get { return _startColor; }
			set { SetProperty(ref _startColor, value); }
		}

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		public Color FinishColor
		{
			get { return _finishColor; }
			set { SetProperty(ref _finishColor, value); }
		}

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		public Color BorderColor
		{
			get { return _borderColor; }
			set { SetProperty(ref _borderColor, value); }
		}

		/// <summary>
		/// Color of cells.
		/// </summary>
		public Color CellColor
		{
			get { return _cellColor; }
			set { SetProperty(ref _cellColor, value); }
		}

		/// <summary>
		/// Show border around cell if TRUE, otherwise hide border.
		/// </summary>
		public bool ShowBorder
		{
			get { return _showBorder; }
			set { SetProperty(ref _showBorder, value); }
		}

		/// <summary>
		/// Show age of the cell if TRUE.
		/// </summary>
		public bool ShowAge
		{
			get { return _showAge; }
			set { SetProperty(ref _showAge, value); }
		}

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		public AnimationType AnimationType
		{
			get { return _animationType; }
			set { SetProperty(ref _animationType, value); }
		}

		/// <summary>
		/// Delay of animation.
		/// </summary>
		public int AnimationDelay
		{
			get { return _animationDelay; }
			set { SetProperty(ref _animationDelay, value); }
		}

		public Point[] CurrentPoints { get; set; } = null;

		#endregion ISettingsViewModel
	}
}
