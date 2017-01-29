using System;
using Ikc5.AutomataScreenSaver.Common.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Models
{
	public class CommandProvider : ICommandProvider
	{
		public CompositeCommand IterateCommand { get; }
		= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand StartIteratingCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand StopIteratingCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
		public CompositeCommand RestartCommand { get; }
			= new CompositeCommand(monitorCommandActivity: true);
	}
}
