using System.ComponentModel;
using System.Windows.Input;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.Math.CellularAutomata;
using Ikc5.TypeLibrary;
using Prism.Commands;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	public class CellViewModel : BindableBase, ICellViewModel
	{
		public CellViewModel(ISettings settings)
		{
			settings.ThrowIfNull(nameof(settings));
			Settings = settings;
		}

		public CellViewModel(ISettings settings, ICell cell)
			: this(settings)
		{
			if (cell != null)
			{
				Cell = cell;
			}
		}

		#region Cell model

		private ICell _cell;

		public ICell Cell
		{
			get { return _cell; }
			set { SetProperty(ref _cell, value); }
		}

		#endregion Cell model

		#region Settings model

		private ISettings _settings;

		public ISettings Settings
		{
			get { return _settings; }
			private set { SetProperty(ref _settings, value); }
		}

		#endregion

	}
}
