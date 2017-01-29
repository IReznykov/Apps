using System.Collections.ObjectModel;
using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.Math.CellularAutomata;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	public interface IAutomatonViewModel
	{
		/// <summary>
		/// Width of current view - expected to be bound to view's actual
		/// width in OneWay binding.
		/// </summary>
		int ViewWidth { get; set; }

		/// <summary>
		/// Height of current view - expected to be bound to view's actual
		/// height in OneWay binding.
		/// </summary>
		int ViewHeight { get; set; }

		/// <summary>
		/// Automaton.
		/// </summary>
		IAutomaton Automaton { get; }

		/// <summary>
		/// 2-dimensional collections for CellViewModels.
		/// </summary>
		ObservableCollection<ObservableCollection<ICellViewModel>> Cells { get; }

		/// <summary>
		/// 2-dimensional collections of booleans, used as background grid.
		/// </summary>
		bool[][] Borders { get; }

		/// <summary>
		/// Property that raises event when automaton width is changed.
		/// It means, new automaton is created.
		/// </summary>
		int AutomatonWidth { get; }

		/// <summary>
		/// Property that raises event when automaton height is changed.
		/// It means, new automaton is created.
		/// </summary>
		int AutomatonHeight { get; }

		/// <summary>
		/// Module settings.
		/// </summary>
		ISettings Settings { get; }

		/// <summary>
		/// Iterates automaton at one cycle.
		/// </summary>
		ICommand IterateCommand { get; }

		/// <summary>
		/// Command starts iterating.
		/// </summary>
		ICommand StartIteratingCommand { get; }

		/// <summary>
		/// Command stops iterating.
		/// </summary>
		ICommand StopIteratingCommand { get; }

		/// <summary>
		/// Command set initial automaton.
		/// </summary>
		ICommand RestartCommand { get; }
	}

}