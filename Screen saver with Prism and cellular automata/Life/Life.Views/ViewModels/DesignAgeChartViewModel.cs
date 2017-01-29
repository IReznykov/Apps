using System;
using System.Collections.Generic;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignAgeChartViewModel : IAgeChartViewModel
	{
		public IReadOnlyList<Tuple<string, int>> AgeCountList =>
			new List<Tuple<string, int>>(new []
			{
				new Tuple<string, int>("1", 5),
				new Tuple<string, int>("2", 10),
				new Tuple<string, int>("3", 7)
			});
	}
}
