using System;
using System.Collections.Generic;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	public interface IAgeChartViewModel
	{
		IReadOnlyList<Tuple<string, int>> AgeCountList { get; }
	}
}