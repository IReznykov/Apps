using System.Windows.Controls;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	/// <summary>
	/// Interaction logic for AgeChartView.xaml
	/// https://ireznykov.com/2017/01/06/wpf-application-with-real-time-data-in-oxyplot-charts/
	/// </summary>
	public partial class AgeChartView : UserControl
	{
		public AgeChartView()
		{
			InitializeComponent();

			IndexAxis.LabelFormatter =
				index =>
				{
					var ratio = (int)System.Math.Round(AgeSeries.Items.Count / 10.0, 0);
					var label = (int)index + 1;
					return (ratio <= 1 || label % ratio == 1) ? label.ToString("D") : string.Empty;
				};
		}
	}
}
