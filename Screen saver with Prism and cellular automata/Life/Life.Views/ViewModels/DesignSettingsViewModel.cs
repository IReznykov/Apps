using System.Drawing;
using System.Windows.Media;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.Math.CellularAutomata;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignSettingsViewModel : ISettingsViewModel
	{
		public int Width { get; set; } = 10;
		public int Height { get; set; } = 10;
		public KnownLifePreset KnownLifePreset { get; set; } = KnownLifePreset.Life;
		public int IterationDelay { get; set; } = 1000;
		public InitialAutomatonType InitialAutomatonType { get; set; } = InitialAutomatonType.Random;

		public Color StartColor { get; set; } = Colors.AliceBlue;
		public Color FinishColor { get; set; } = Colors.DarkBlue;
		public Color BorderColor { get; set; } = Colors.DarkGray;
		public Color CellColor { get; set; } = Colors.Transparent;
		public bool ShowBorder { get; set; } = true;
		public bool ShowAge { get; set; } = true;
		public AnimationType AnimationType { get; set; } = AnimationType.None;
		public int AnimationDelay { get; set; } = 100;
		public Point[] CurrentPoints { get; set; } = null;
	}
}
