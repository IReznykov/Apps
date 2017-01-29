using System.ComponentModel;
using Prism.Commands;

namespace Ikc5.AutomataScreenSaver.Common.Models
{
	public interface ICommandProvider
	{
		CompositeCommand IterateCommand { get; }
		CompositeCommand StartIteratingCommand { get; }
		CompositeCommand StopIteratingCommand { get; }
		CompositeCommand RestartCommand { get; }
	}
}
