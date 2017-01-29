using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ikc5.AutomataScreenSaver.Common.ViewModels.Converters
{
	/// <summary>
	/// Makes color darker depends on their values.
	/// </summary>
	public class DarkerColorConverter : IMultiValueConverter
	{
		/// <summary>
		/// Input parameters should be:
		/// 0 - color
		/// 1 - bool, if FALSE, do nothing, if TRUE returns darker color
		/// 2 - short, weight of darkness
		/// 3? - short, max value
		/// </summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(Color))
				return DependencyProperty.UnsetValue;

			try
			{

				var endColor = Colors.Black;
				var originColor = (Color)values[0];
				var modify = System.Convert.ToBoolean(values[1]);
				var intWeight = System.Convert.ToInt32(values[2]);

				if (!modify || intWeight <= 0)
					return originColor;

				var weight = Math.Log10(Math.Min(intWeight, 100)) / 2;
				var darkerColor = Color.FromRgb(
								(byte)Math.Round(originColor.R * (1 - weight) + endColor.R * weight),
								(byte)Math.Round(originColor.G * (1 - weight) + endColor.G * weight),
								(byte)Math.Round(originColor.B * (1 - weight) + endColor.B * weight));

				return darkerColor;
			}
			catch (Exception)
			{
				return DependencyProperty.UnsetValue;
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes,
			   object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
