using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Common.Models;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.Math.CellularAutomata;
using Ikc5.TypeLibrary.Logging;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignAutomatonViewModel : IAutomatonViewModel
	{
		public DesignAutomatonViewModel()
		{
			var statistics = new Statistics();
			Automaton = new Automaton(12, 10, new MooreCellLifeService(KnownLifePresets.Life), new EmptyLogger());
			Automaton.SetPoints(new[]
			{
				new Point(1,1),
				new Point(3,3),
				new Point(5,5),
				new Point(7,7),
				new Point(9,9),
				new Point(1,9),
				new Point(3,7),
				new Point(5,5),
				new Point(7,3),
				new Point(9,1),
			}, ref statistics);

			Borders = new bool[AutomatonHeight][];
			for (var posRow = 0; posRow < Borders.Length; posRow++)
			{
				Borders[posRow] = new bool[AutomatonWidth];
			}
		}

		public int ViewWidth { get; set; } = 300;

		public int ViewHeight { get; set; } = 200;

		public IAutomaton Automaton { get; }

		public ObservableCollection<ObservableCollection<ICellViewModel>> Cells => null;

		public bool[][] Borders { get; }

		public int AutomatonWidth { get; } = 12;

		public int AutomatonHeight { get; } = 10;

		
		public ISettings Settings { get; } = null;

		public ICommand IterateCommand { get; } = null;

		public ICommand StartIteratingCommand { get; } = null;
		public ICommand StopIteratingCommand { get; } = null;

		public ICommand RestartCommand { get; } = null;
	}
}
