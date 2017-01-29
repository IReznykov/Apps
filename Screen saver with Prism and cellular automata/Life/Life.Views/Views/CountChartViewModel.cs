using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.TypeLibrary;
using OxyPlot;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	public class CountChartViewModel : BindableBase, ICountChartViewModel
	{
		private readonly IChartRepository _chartRepository;

		public CountChartViewModel(IChartRepository chartRepository)
		{
			chartRepository.ThrowIfNull(nameof(chartRepository));
			_chartRepository = chartRepository;
			chartRepository.PropertyChanged += ChartRepositoryPropertyChanged;
		}

		private void ChartRepositoryPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!nameof(IChartRepository.TotalCellCountList).Equals(e.PropertyName) &&
				!nameof(IChartRepository.BornedCellCountList).Equals(e.PropertyName) &&
				!nameof(IChartRepository.DiedCellCountList).Equals(e.PropertyName))
				return;

			OnPropertyChanged(e.PropertyName);
		}

		public IReadOnlyList<DataPoint> TotalCellCountList =>
			_chartRepository.TotalCellCountList.Select((value, index) => new DataPoint(index, value)).ToList();
		public IReadOnlyList<DataPoint> BornedCellCountList =>
			_chartRepository.BornedCellCountList.Select((value, index) => new DataPoint(index, value)).ToList();
		public IReadOnlyList<DataPoint> DiedCellCountList =>
			_chartRepository.DiedCellCountList.Select((value, index) => new DataPoint(index, value)).ToList();
	}
}
