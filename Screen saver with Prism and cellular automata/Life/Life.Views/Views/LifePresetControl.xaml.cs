using System;
using System.Windows;
using System.Windows.Controls;
using Ikc5.Math.CellularAutomata;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	/// <summary>
	/// Interaction logic for LifePresetControl
	/// </summary>
	public partial class LifePresetControl : UserControl
	{
		public LifePresetControl()
		{
			InitializeComponent();
			LifePreset = KnownLifePresets.GetKnownLifePreset(KnownLifePreset);
		}

		/// <summary>
		/// Life preset.
		/// </summary>
		public KnownLifePreset KnownLifePreset
		{
			get { return (KnownLifePreset)GetValue(KnownLifePresetProperty); }
			set
			{
				SetValue(KnownLifePresetProperty, value);
			}
		}

		public static readonly DependencyProperty KnownLifePresetProperty =
			DependencyProperty.Register(
				"KnownLifePreset",
				typeof(KnownLifePreset),
				typeof(LifePresetControl),
				new PropertyMetadata(KnownLifePreset.Life, OnKnownLifePresetPropertyChanged));

		private static void OnKnownLifePresetPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var lifePresetControl = dependencyObject as LifePresetControl;
			if(lifePresetControl!=null)
				lifePresetControl.LifePreset = KnownLifePresets.GetKnownLifePreset((KnownLifePreset)dependencyPropertyChangedEventArgs.NewValue);
		}

		private ILifePreset _lifePreset;

		public ILifePreset LifePreset
		{
			get
			{
				return _lifePreset;
			}
			internal set
			{
				_lifePreset = value;
				var types = new[]
				{
					new Tuple<string, Func<int,bool>>("Born", pos => _lifePreset.Born(pos)),
					new Tuple<string, Func<int,bool>>("Survive", pos => _lifePreset.Survive(pos))
				};
				foreach (var type in types)
				{
					for (var pos = 0; pos < 9; pos++)
					{
						var controlName = $"{type.Item1}{pos}";
						var checkBoox = FindName(controlName) as CheckBox;
						if (checkBoox != null)
						{
							checkBoox.IsChecked = type.Item2(pos);
							//checkBoox.InvalidateVisual();
						}
					}
				}
			}
		}
	}
}
