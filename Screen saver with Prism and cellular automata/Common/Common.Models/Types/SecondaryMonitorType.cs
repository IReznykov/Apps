using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.AutomataScreenSaver.Common.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum SecondaryMonitorType
	{
		[Description("Empty window")]
		Empty,

		[Description("Automaton")]
		MainWindow,
	}
}
