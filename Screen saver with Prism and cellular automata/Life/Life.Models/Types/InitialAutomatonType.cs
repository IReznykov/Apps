using System.ComponentModel;
using Ikc5.TypeLibrary;

namespace Ikc5.AutomataScreenSaver.Life.Models.Types
{
	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum InitialAutomatonType
	{
		[Description("Previous state")]
		Previous,

		[Description("Date & Time")]
		DateTime,

		[Description("UTC Date & Time")]
		UtcDateTime,

		[Description("Random")]
		Random,
	}
}
