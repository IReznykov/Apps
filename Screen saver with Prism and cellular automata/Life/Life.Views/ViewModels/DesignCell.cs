using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ikc5.Math.CellularAutomata;
using Prism.Mvvm;

namespace Ikc5.AutomataScreenSaver.Life.ViewModels
{
	internal class DesignCell : BindableBase, ICell
	{
		public void AddVertSum(short delta)
		{
			throw new NotImplementedException();
		}

		public void ApplyChange()
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool State { get; } = true;
		public short VertSum { get; } = 1;
		public short Age { get; } = 3;
		public bool? NextState { get; set; } = null;
		public bool IsChanged { get; } = false;
		public short Delta { get; } = 0;
	}
}
