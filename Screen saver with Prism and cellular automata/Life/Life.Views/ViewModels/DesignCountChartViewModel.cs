using System.Collections.Generic;
using OxyPlot;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignCountChartViewModel : ICountChartViewModel
	{
		public DesignCountChartViewModel()
		{
			TotalCellCountList = new List<DataPoint>(new[] { new DataPoint(0, 10), new DataPoint(1, 15), new DataPoint(2, 10), new DataPoint(3, 7), });
			BornedCellCountList = new List<DataPoint>(new[] { new DataPoint(0, 10), new DataPoint(1, 8), new DataPoint(2, 1), new DataPoint(3, 2), });
			DiedCellCountList = new List<DataPoint>(new[] { new DataPoint(0,  0), new DataPoint(1, 3), new DataPoint(2, 6), new DataPoint(3, 5), });
		}
		public IReadOnlyList<DataPoint> TotalCellCountList { get; }
		public IReadOnlyList<DataPoint> BornedCellCountList { get; }
		public IReadOnlyList<DataPoint> DiedCellCountList { get; }
	}
}
