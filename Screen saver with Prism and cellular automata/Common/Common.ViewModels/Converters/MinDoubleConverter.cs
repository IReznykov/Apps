using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Ikc5.AutomataScreenSaver.Common.ViewModels.Converters
{
	/// <summary>
	/// Returns minimal value of input double values.
	/// </summary>
	[ValueConversion(typeof(double[]), typeof(double))]
	public class MinDoubleConverter : ValuesGenericConverter<double>
	{
		protected override Func<object, CultureInfo, double> ConvertMethod =>
			(value, culture) =>
			{
				if ( value == null )
					return 0;
				var result = System.Convert.ToDouble(value);
				return double.IsNaN(result) ? 0 : result;
			};

		protected override Func<IEnumerable<double>, double> AggregateMethod => (enumerable) => enumerable.Min();

		protected override Func<double, double, double> ApplyParameterMethod => (a, b) => Math.Max(0, a + b);
	}
}
