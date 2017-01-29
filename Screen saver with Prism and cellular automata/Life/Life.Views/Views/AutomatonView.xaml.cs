using System;
using System.Windows.Controls;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Prism;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	/// <summary>
	/// Presentation of dynamical data structure:
	/// https://ireznykov.com/2017/01/04/grid-with-dynamic-number-of-rows-and-columns-part-2/
	/// </summary>
	public partial class AutomatonView : UserControl, IActiveAware
	{
		public AutomatonView()
		{
			Loaded += AutomatonView_Loaded;
			InitializeComponent();
		}

		private void AutomatonView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			(DataContext as IAutomatonViewModel)?.StartIteratingCommand.Execute(null);
		}

		#region IActiveAware implementation

		private bool _isActive;

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive == value)
					return;
				_isActive = value;

				var viewModelActiveAware = DataContext as IActiveAware;
				if (viewModelActiveAware != null)
					viewModelActiveAware.IsActive = value;

				OnIsActiveChanged();
			}
		}

		public event EventHandler IsActiveChanged = delegate { };

		protected virtual void OnIsActiveChanged()
		{
			IsActiveChanged.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}
