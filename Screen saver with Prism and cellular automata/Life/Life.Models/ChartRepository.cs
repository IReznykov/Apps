using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Life.Models
{
	/// <summary>
	/// Chart repository receives data from services, parses and provides it to consumers.
	/// https://ireznykov.com/2017/01/06/wpf-application-with-real-time-data-in-oxyplot-charts/
	/// </summary>
	public class ChartRepository : BindableBase, IChartRepository
	{

		#region Lists

		private readonly List<int> _totalCellCountList = new List<int>(100);
		private readonly List<int> _bornedCellCountList = new List<int>(100);
		private readonly List<int> _diedCountList = new List<int>(100);
		private readonly List<int> _ageCountList = new List<int>(100);

		#endregion

		#region IChartRepository

		public IReadOnlyList<int> TotalCellCountList => _totalCellCountList;
		public IReadOnlyList<int> BornedCellCountList => _bornedCellCountList;
		public IReadOnlyList<int> DiedCellCountList => _diedCountList;
		public IReadOnlyList<int> AgeCountList => _ageCountList;

		public void AddTotalCellCount(int newValue)
		{
			_totalCellCountList.Add(newValue);
			if (_totalCellCountList.Count > 100)
				_totalCellCountList.RemoveAt(0);
			OnPropertyChanged(nameof(TotalCellCountList));
		}

		public void AddBornedCellCount(int newValue)
		{
			_bornedCellCountList.Add(newValue);
			if (_bornedCellCountList.Count > 100)
				_bornedCellCountList.RemoveAt(0);
			OnPropertyChanged(nameof(BornedCellCountList));
		}

		public void AddDiedCellCount(int newValue)
		{
			_diedCountList.Add(newValue);
			if (_diedCountList.Count > 100)
				_diedCountList.RemoveAt(0);
			OnPropertyChanged(nameof(DiedCellCountList));
		}

		public void UpdateAges(IEnumerable<int> ageSeries)
		{
			// takes value for 1 -- 49 ages
			_ageCountList.Clear();
			var ageList = ageSeries.ToList();
			_ageCountList.AddRange(ageList.Skip(1).Take(49));
			// add value for all cells older than 50
			var oldest = ageList.Skip(49).Sum();
			if (oldest > 0)
				_ageCountList.Add(oldest);

			OnPropertyChanged(nameof(AgeCountList));
		}

		#endregion
	}
}
