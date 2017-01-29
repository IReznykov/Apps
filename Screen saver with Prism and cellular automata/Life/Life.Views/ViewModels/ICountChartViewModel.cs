using System.Collections.Generic;
using OxyPlot;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	public interface ICountChartViewModel
	{
		IReadOnlyList<DataPoint> TotalCellCountList { get; }
		IReadOnlyList<DataPoint> BornedCellCountList { get; }
		IReadOnlyList<DataPoint> DiedCellCountList { get; }
	}
}