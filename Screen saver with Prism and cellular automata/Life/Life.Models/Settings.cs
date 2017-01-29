using System;
using System.ComponentModel;
using System.Drawing;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.Math.CellularAutomata;
using Ikc5.Prism.Settings;
using Ikc5.Prism.Settings.Models;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Life.Models
{
	/// <summary>
	/// Settings for Life cellular automaton.
	/// Base class initiates properties with default values.
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

		// automaton settings
		private int _width;							// = 40;
		private int _height;						// = 40;
		private KnownLifePreset _knownLifePreset;	// = KnownLifePreset.Life;
		private int _iterationDelay;				// = 500;
		private InitialAutomatonType _initialAutomatonType;// = InitialAutomatonType.Random;

		// cell settings
		private Color _startColor;					// = Colors.AliceBlue;
		private Color _finishColor;					// = Colors.DarkBlue;
		private Color _borderColor;					// = Colors.Gray;
		private Color _cellColor;					// = Colors.Transparent;
		private bool _showBorder;					// = true;
		private bool _showAge;						// = false;
		private AnimationType _animationType;		// = AnimationType.Fade;
		private int _animationDelay;                // = 150;
		private Point[] _currentPoints = null;

		/// <summary>
		/// Desired width of automaton's cell.
		/// </summary>
		[DefaultValue(40)]
		public int Width
		{
			get { return _width; }
			set { SetProperty(ref _width, value); }
		}

		/// <summary>
		/// Desired height of automaton's cell.
		/// </summary>
		[DefaultValue(40)]
		public int Height
		{
			get { return _height; }
			set { SetProperty(ref _height, value); }
		}

		/// <summary>
		/// Life model.
		/// </summary>
		[DefaultValue(KnownLifePreset.Life)]
		public KnownLifePreset KnownLifePreset
		{
			get { return _knownLifePreset; }
			set { SetProperty(ref _knownLifePreset, value); }
		}

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		[DefaultValue(500)]
		public int IterationDelay
		{
			get { return _iterationDelay; }
			set { SetProperty(ref _iterationDelay, value); }
		}

		/// <summary>
		/// Initial automaton type.
		/// </summary>
		[DefaultValue(InitialAutomatonType.Random)]
		public InitialAutomatonType InitialAutomatonType
		{
			get { return _initialAutomatonType; }
			set { SetProperty(ref _initialAutomatonType, value); }
		}

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>
		[DefaultValue(typeof (Color), "#FFF0F8FF")]
		public Color StartColor
		{
			get { return _startColor; }
			set { SetProperty(ref _startColor, value); }
		}

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		[DefaultValue(typeof (Color), "#FF00008B")]
		public Color FinishColor
		{
			get { return _finishColor; }
			set { SetProperty(ref _finishColor, value); }
		}

		/// <summary>
		/// Color of cells.
		/// </summary>
		[DefaultValue(typeof (Color), "#00FFFFFF")]
		public Color CellColor
		{
			get { return _cellColor; }
			set { SetProperty(ref _cellColor, value); }
		}

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		[DefaultValue(typeof(Color), "#FF808080")]
		public Color BorderColor
		{
			get { return _borderColor; }
			set { SetProperty(ref _borderColor, value); }
		}

		/// <summary>
		/// Show border around cell if TRUE, otherwise hide border.
		/// </summary>
		[DefaultValue(false)]
		public bool ShowBorder
		{
			get { return _showBorder; }
			set { SetProperty(ref _showBorder, value); }
		}

		/// <summary>
		/// Show age of the cell if TRUE.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowAge
		{
			get { return _showAge; }
			set { SetProperty(ref _showAge, value); }
		}

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		[DefaultValue(AnimationType.Fade)]
		public AnimationType AnimationType
		{
			get { return _animationType; }
			set { SetProperty(ref _animationType, value); }
		}

		/// <summary>
		/// Delay of animation.
		/// </summary>
		[DefaultValue(150)]
		public int AnimationDelay
		{
			get { return _animationDelay; }
			set { SetProperty(ref _animationDelay, value); }
		}

		/// <summary>
		/// Current points of the automaton.
		/// </summary>
		[DefaultValue(null)]
		public Point[] CurrentPoints
		{
			get { return _currentPoints; }
			set { SetProperty(ref _currentPoints, value); }
		}

		#endregion
	}
}