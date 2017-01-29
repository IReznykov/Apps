using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.Math.CellularAutomata;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignSettings : ISettings
	{
		public int Width { get; set; } = 40;
		public int Height { get; set; } = 40;
		public KnownLifePreset KnownLifePreset { get; set; } = KnownLifePreset.Life;
		public int IterationDelay { get; set; } = 500;
		public InitialAutomatonType InitialAutomatonType { get; set; } = InitialAutomatonType.Random;
		public Color StartColor { get; set; } = Colors.AliceBlue;
		public Color FinishColor { get; set; } = Colors.DarkBlue;
		public Color BorderColor { get; set; } = Colors.DarkGray;
		public Color CellColor { get; set; } = Colors.Transparent;
		public bool ShowBorder { get; set; } = false;
		public bool ShowAge { get; set; } = true;
		public AnimationType AnimationType { get; set; } = AnimationType.Fade;
		public int AnimationDelay { get; set; } = 500;
		public Point[] CurrentPoints { get; set; } = null;
	}
}
