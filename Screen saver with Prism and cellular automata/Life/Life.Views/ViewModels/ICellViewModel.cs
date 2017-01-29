using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.Math.CellularAutomata;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	public interface ICellViewModel
	{
		ICell Cell { get; set; }

		ISettings Settings { get; }
	}
}