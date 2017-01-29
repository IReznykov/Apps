using System.Drawing;
using Ikc5.AutomataScreenSaver.Common.Models.Types;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.Math.CellularAutomata;
using Color = System.Windows.Media.Color;

namespace Ikc5.AutomataScreenSaver.Life.Models
{
	/// <summary>
	/// Settings for Life automaton module.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Desired width of automaton's cell.
		/// </summary>
		int Width { get; set; }

		/// <summary>
		/// Desired height of automaton's cell.
		/// </summary>
		int Height { get; set; }

		/// <summary>
		/// Life model.
		/// </summary>
		KnownLifePreset KnownLifePreset { get; set; }

		/// <summary>
		/// Delay between iterations.
		/// </summary>
		int IterationDelay { get; set; }

		/// <summary>
		/// Initial automaton type.
		/// </summary>
		InitialAutomatonType InitialAutomatonType { get; set; }

		/// <summary>
		/// Start, the lighter, color of cells.
		/// </summary>s
		Color StartColor { get; set; }

		/// <summary>
		/// Finish, the darker, color of cells.
		/// </summary>
		Color FinishColor { get; set; }

		/// <summary>
		/// Color of borders around cells.
		/// </summary>
		Color BorderColor { get; set; }

		/// <summary>
		/// Color of cells.
		/// </summary>
		Color CellColor { get; set; }

		/// <summary>
		/// Show border around cell if TRUE, otherwise hide border.
		/// </summary>
		bool ShowBorder { get; set; }

		/// <summary>
		/// Show age of the cell if TRUE.
		/// </summary>
		bool ShowAge { get; set; }

		/// <summary>
		/// Animation type when cell is appearing/disappearing.
		/// </summary>
		AnimationType AnimationType { get; set; }

		/// <summary>
		/// Delay of animation.
		/// </summary>
		int AnimationDelay { get; set; }

		/// <summary>
		/// Current points of the automaton.
		/// </summary>
		Point[] CurrentPoints { get; set; }
	}
}