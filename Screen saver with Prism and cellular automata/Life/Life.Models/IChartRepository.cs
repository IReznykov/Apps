using System.Collections.Generic;
using System.ComponentModel;

namespace Ikc5.AutomataScreenSaver.Life.Models
{
	/// <summary>
	/// Chart repository receives data from services, parses and provides it to consumers.
	/// https://ireznykov.com/2017/01/06/wpf-application-with-real-time-data-in-oxyplot-charts/
	/// </summary>
	public interface IChartRepository : INotifyPropertyChanged
	{
		IReadOnlyList<int> TotalCellCountList { get; }
		IReadOnlyList<int> BornedCellCountList { get; }
		IReadOnlyList<int> DiedCellCountList { get; }
		IReadOnlyList<int> AgeCountList { get; }

		void AddTotalCellCount(int newValue);
		void AddBornedCellCount(int newValue);
		void AddDiedCellCount(int newValue);
		void UpdateAges(IEnumerable<int> ageSeries);
	}
}
