using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.Math.CellularAutomata;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignCellViewModel : ICellViewModel
	{
		public ICell Cell { get; set; } = new DesignCell();
		public ISettings Settings { get; } = new DesignSettings();
	}
}
