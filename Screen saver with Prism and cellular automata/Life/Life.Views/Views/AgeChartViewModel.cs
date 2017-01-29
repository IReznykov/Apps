using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ikc5.AutomataScreenSaver.Life.Models;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.TypeLibrary;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	public class AgeChartViewModel : BindableBase, IAgeChartViewModel
	{
		private readonly IChartRepository _chartRepository;

		public AgeChartViewModel(IChartRepository chartRepository)
		{
			chartRepository.ThrowIfNull(nameof(chartRepository));
			_chartRepository = chartRepository;
			chartRepository.PropertyChanged += ChartRepositoryPropertyChanged;
		}

		private void ChartRepositoryPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!nameof(IChartRepository.AgeCountList).Equals(e.PropertyName))
				return;

			OnPropertyChanged(e.PropertyName);
		}

		/// <summary>
		/// All ages except empty cells - for some life presets
		/// they occupied too much space.
		/// </summary>
		public IReadOnlyList<Tuple<string, int>> AgeCountList =>
			_chartRepository.AgeCountList.
			Select((value, index) => new Tuple<string, int>(index.ToString("D"), value)).
			ToList();
	}
}
