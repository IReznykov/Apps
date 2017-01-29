using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	/// <summary>
	/// Interaction logic for CellControl
	/// </summary>
	public partial class CellView : UserControl
	{
		public CellView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Start brush for gradient filling - the darkest color of the cell.
		/// </summary>
		public Color StartColor
		{
			get { return (Color)GetValue(StartColorProperty); }
			set { SetValue(StartColorProperty, value); }
		}

		public static readonly DependencyProperty StartColorProperty =
			DependencyProperty.Register(
				"StartColor",
				typeof(Color),
				typeof(CellView));

		/// <summary>
		/// Finish Color for gradient filling - the lightest color of the cell.
		/// </summary>
		public Color FinishColor
		{
			get { return (Color)GetValue(FinishColorProperty); }
			set { SetValue(FinishColorProperty, value); }
		}

		public static readonly DependencyProperty FinishColorProperty =
			DependencyProperty.Register(
				"FinishColor",
				typeof(Color),
				typeof(CellView));

	}

}
