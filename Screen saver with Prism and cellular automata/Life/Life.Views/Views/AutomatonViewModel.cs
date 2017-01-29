using System;
using System.Collections.ObjectModel;
using Ikc5.AutomataScreenSaver.Life.Models;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Ikc5.AutomataScreenSaver.Common.Models;
using Ikc5.AutomataScreenSaver.Life.Models.Types;
using Ikc5.AutomataScreenSaver.Life.ViewModels;
using Ikc5.Math.CellularAutomata;
using Ikc5.Prism.Settings;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;
using Prism;
using Prism.Commands;
using FontStyle = System.Drawing.FontStyle;
using Point = System.Drawing.Point;

namespace Ikc5.AutomataScreenSaver.Life.Views
{
	public class AutomatonViewModel : BindableBase, IAutomatonViewModel, IActiveAware
	{
		private bool _postponedTimer = false;
		private readonly DispatcherTimer _resizeTimer;
		private readonly DispatcherTimer _iterateTimer;
		private readonly IChartRepository _chartRepository;
		private readonly ILogger _logger;

		public AutomatonViewModel(
			ICellLifeService cellLifeService,
			ISettings settings,
			IChartRepository chartRepository,
			ICommandProvider automatonCommandProvider,
			ILogger logger)
		{
			settings.ThrowIfNull(nameof(settings));
			Settings = settings;

			chartRepository.ThrowIfNull(nameof(chartRepository));
			_chartRepository = chartRepository;

			automatonCommandProvider.ThrowIfNull(nameof(automatonCommandProvider));

			logger.ThrowIfNull(nameof(logger));
			_logger = logger;

			cellLifeService.ThrowIfNull(nameof(cellLifeService));
			CellLifeService = cellLifeService;
			CellLifeService.LifePreset = KnownLifePresets.GetKnownLifePreset(Settings.KnownLifePreset);

			_resizeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100),
			};
			_resizeTimer.Tick += ResizeTimerTick;

			_iterateTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(Settings.IterationDelay),
			};
			_iterateTimer.Tick += IterateTimerTick;
			_logger.Log("AutomatonViewModel constructor is completed");

			Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

			// create commands
			IterateCommand = new DelegateCommand(Iterate, () => CanIterate)
			{ IsActive = IsActive };
			StartIteratingCommand = new DelegateCommand(StartTimer, () => CanStartIterating)
			{ IsActive = IsActive };
			StopIteratingCommand = new DelegateCommand(StopTimer, () => CanStopIterating)
			{ IsActive = IsActive };
			RestartCommand = new DelegateCommand(SetInitialAutomaton, () => CanRestart)
			{ IsActive = IsActive };

			automatonCommandProvider.IterateCommand.RegisterCommand(IterateCommand);
			automatonCommandProvider.StartIteratingCommand.RegisterCommand(StartIteratingCommand);
			automatonCommandProvider.StopIteratingCommand.RegisterCommand(StopIteratingCommand);
			automatonCommandProvider.RestartCommand.RegisterCommand(RestartCommand);

			SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.None);
		}

		#region Settings model

		private ISettings _settings;

		public ISettings Settings
		{
			get { return _settings; }
			private set
			{
				var userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged -= UserSettingsOnPropertyChanged;

				SetProperty(ref _settings, value);
				userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged += UserSettingsOnPropertyChanged;
			}
		}

		private void UserSettingsOnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (Automaton == null)
				return;

			PauseIteration();

			switch (args.PropertyName)
			{
			case nameof(Settings.Width):
			case nameof(Settings.Height):
				ImplementNewSize();
				break;

			case nameof(Settings.KnownLifePreset):
				CellLifeService.LifePreset = KnownLifePresets.GetKnownLifePreset(Settings.KnownLifePreset);
				break;

			case nameof(Settings.InitialAutomatonType):
				if (Settings.InitialAutomatonType != InitialAutomatonType.Previous)
				{
					Automaton.Clear();
					AutomatonSetPoints(InitialAutomaton());
				}
				break;

			case nameof(Settings.IterationDelay):
				_iterateTimer.Interval = TimeSpan.FromMilliseconds(Settings.IterationDelay);
				break;
			}

			ContinueIteration();
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			Settings.CurrentPoints = Automaton?.GetPoints().ToArray();
		}

		#endregion Settings model

		#region Initialization and recreating

		/// <summary>
		/// Add points to empty automaton.
		/// </summary>
		/// <param name="points"></param>
		private void AutomatonSetPoints(IEnumerable<Point> points)
		{
			var statistics = new Statistics();
			Automaton.SetPoints(points, ref statistics);
			UpdateChartRepository(statistics);
		}

		private ICellLifeService CellLifeService { get; }

		private IEnumerable<Point> InitialAutomaton()
		{
			_logger.LogStart("Calculates points");
			IEnumerable<Point> points;

			switch (Settings.InitialAutomatonType)
			{
			case InitialAutomatonType.Previous:
				if (Settings.CurrentPoints == null || Settings.CurrentPoints.Length == 0)
				{
					points = GenerateRandomPoints();
				}
				else
				{
					points = new List<Point>(Settings.CurrentPoints);
					// presave random points
					Settings.CurrentPoints = GenerateRandomPoints().ToArray();
				}
				break;

			case InitialAutomatonType.DateTime:
			case InitialAutomatonType.UtcDateTime:
				{
					var dateTime = Settings.InitialAutomatonType == InitialAutomatonType.DateTime ? DateTime.Now : DateTime.UtcNow;
					var values = new[] { dateTime.ToShortDateString(), dateTime.ToLongTimeString() };

					using (var textFont = new Font("Lucida Console", 10, FontStyle.Bold))
					using (var textImage = GraphicsHelpers.DrawTextImage(values, AutomatonWidth, AutomatonHeight,
						textFont, Color.Black, Color.White))
					{
						points = GraphicsHelpers.ConvertToPoints(textImage);
					}
				}
				break;

			case InitialAutomatonType.Random:
			default:
				points = GenerateRandomPoints();
				break;
			}

			_logger.LogEnd("Completed");
			return points;
		}

		private IEnumerable<Point> GenerateRandomPoints()
		{
			var listPoints = new List<Point>();
			var random = new Random();
			for (var y = 0; y < AutomatonHeight; y++)
			{
				var rowBytes = new byte[AutomatonWidth];
				random.NextBytes(rowBytes);
				for (var x = 0; x < AutomatonWidth; x++)
				{
					if (rowBytes[x] % 7 == 0)
						listPoints.Add(new Point(x, y));
				}
			}
			return listPoints;
		}

		private CancellationTokenSource _cancellationSource = null;

		private async void CreateOrUpdateCellViewModels()
		{
			_logger.LogStart("Start");

			// stop previous tasks that creates viewModels
			if (_cancellationSource != null && _cancellationSource.Token.CanBeCanceled)
			{
				_cancellationSource.Cancel();
			}

			if (Cells == null)
			{
				Cells = new ObservableCollection<ObservableCollection<ICellViewModel>>();
			}

			Borders = new bool[AutomatonHeight][];
			for (var posRow = 0; posRow < Borders.Length; posRow++)
			{
				Borders[posRow] = new bool[AutomatonWidth];
			}

			try
			{
				_cancellationSource = new CancellationTokenSource();
				await CreateCellViewModelsAsync(0, _cancellationSource.Token).ConfigureAwait(false);
			}
			catch (OperationCanceledException ex)
			{
				_logger.Exception(ex);
			}
			catch (AggregateException ex)
			{
				foreach (var innerException in ex.InnerExceptions)
				{
					_logger.Exception(innerException);
				}
			}
			finally
			{
				_cancellationSource = null;
			}
			_logger.LogEnd("Completed - but adding is asynchronous");
		}

		private Task CreateCellViewModelsAsync(int rowNumber, CancellationToken cancellationToken)
		{
			return Task.Run(async () =>
			{
				cancellationToken.ThrowIfCancellationRequested();
				_logger.Log($"Process {rowNumber} row of cells");
				var positionToProcess = rowNumber;

				if (rowNumber >= Automaton.Size.Height)
				{
					// either "check" call for the last+1 row, or call for
					// old rows that should be deleted 
					if (rowNumber < Cells.Count)
					{
						_logger.Log($"Remove {rowNumber} row of cells");
						// In order to have responsive GUI, ApplicationIdle is quite good
						// Other priority could be used making adding rows quicker.
						Application.Current.Dispatcher.Invoke(
							() => Cells.RemoveAt(positionToProcess),
							DispatcherPriority.ApplicationIdle,
							cancellationToken);
					}
					else
					{
						_logger.Log($"Empty check of {rowNumber} row of cells");
					}
				}
				else if (rowNumber < Cells.Count)
				{
					_logger.Log($"Update {rowNumber} row of cells");
					// call for rows that already created and should be reInit
					// and maybe changed in their size
					Application.Current.Dispatcher.Invoke(
						() => UpdateCellViewModelRow(positionToProcess),
						DispatcherPriority.ApplicationIdle,
						cancellationToken);
				}
				else
				{
					_logger.Log($"Add {rowNumber} row of cells");
					// In order to have responsive GUI, ApplicationIdle is quite good
					// Other priority could be used making adding rows quicker.
					Application.Current.Dispatcher.Invoke(
						() => CreateCellViewModelRow(positionToProcess),
						DispatcherPriority.ApplicationIdle,
						cancellationToken);
				}

				// create asynchronous task for processing next row
				if (rowNumber < Automaton.Size.Height)
				{
					await CreateCellViewModelsAsync(++rowNumber, cancellationToken).ConfigureAwait(false);
				}
				else if (rowNumber < Cells.Count)
				{
					await CreateCellViewModelsAsync(rowNumber, cancellationToken).ConfigureAwait(false);
				}

			}, cancellationToken);
		}

		private void CreateCellViewModelRow(int rowNumber)
		{
			_logger.Log($"Create {rowNumber} row of cells");
			var row = new ObservableCollection<ICellViewModel>();
			for (var x = 0; x < Automaton.Size.Width; x++)
			{
				var cellViewModel = new CellViewModel(Settings, Automaton.GetCell(x, rowNumber));
				row.Add(cellViewModel);
			}

			_logger.Log($"{rowNumber} row of cells is ready for rendering");
			Cells.Add(row);
		}

		private void UpdateCellViewModelRow(int rowNumber)
		{
			var row = Cells[rowNumber];
			// delete additional cells
			while (row.Count > Automaton.Size.Width)
				row.RemoveAt(Automaton.Size.Width);
			for (var pos = 0; pos < Automaton.Size.Width; pos++)
			{
				// create new ViewModel or update existent one
				var cell = Automaton.GetCell(pos, rowNumber);
				if (pos < row.Count)
					row[pos].Cell = cell;
				else
				{
					var cellViewModel = new CellViewModel(Settings, cell);
					row.Add(cellViewModel);
				}
			}
		}

		#endregion

		#region IAutomatonViewModel

		private int _viewWidth = 0;
		private int _viewHeight = 0;
		private IAutomaton _automaton;
		private ObservableCollection<ObservableCollection<ICellViewModel>> _cells;
		private bool[][] _borders;
		private int _automatonWidth = 0;
		private int _automatonHeight = 0;

		[DefaultValue(0)]
		public int ViewWidth
		{
			get { return _viewWidth; }
			set { SetProperty(ref _viewWidth, value); }
		}

		[DefaultValue(0)]
		public int ViewHeight
		{
			get { return _viewHeight; }
			set { SetProperty(ref _viewHeight, value); }
		}

		public IAutomaton Automaton
		{
			get { return _automaton; }
			set { SetProperty(ref _automaton, value); }
		}

		public ObservableCollection<ObservableCollection<ICellViewModel>> Cells
		{
			get { return _cells; }
			set { SetProperty(ref _cells, value); }
		}

		public bool[][] Borders
		{
			get { return _borders; }
			set { SetProperty(ref _borders, value); }
		}

		[DefaultValue(0)]
		public int AutomatonWidth
		{
			get { return _automatonWidth; }
			private set { SetProperty(ref _automatonWidth, value); }
		}

		[DefaultValue(0)]
		public int AutomatonHeight
		{
			get { return _automatonHeight; }
			private set { SetProperty(ref _automatonHeight, value); }
		}

		/// <summary>
		/// Iterates automaton at one cycle.
		/// </summary>
		public ICommand IterateCommand { get; }

		/// <summary>
		/// Command starts iterating.
		/// </summary>
		public ICommand StartIteratingCommand { get; }

		/// <summary>
		/// Command stops iterating.
		/// </summary>
		public ICommand StopIteratingCommand { get; }

		/// <summary>
		/// Command set initial automaton.
		/// </summary>
		public ICommand RestartCommand { get; }

		#region CanExecute commands

		/// <summary>
		/// Defines that IterateCommand could be executed.
		/// </summary>
		protected bool CanIterate { get; set; }

		/// <summary>
		/// Defines that StartIteratingCommand could be executed.
		/// </summary>
		protected bool CanStartIterating { get; set; }

		/// <summary>
		/// Defines that StopIteratingCommand could be executed.
		/// </summary>
		protected bool CanStopIterating { get; set; }

		/// <summary>
		/// Defines that RestartCommand could be executed.
		/// </summary>
		protected bool CanRestart { get; set; }

		#endregion
		#endregion

		#region IActiveAware

		private bool _isActive;

		/// <summary>
		/// Active view.
		/// </summary>
		[DefaultValue(false)]
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (_isActive == value)
					return;

				_isActive = value;
				OnIsActiveChanged();

				((DelegateCommand)IterateCommand).IsActive = _isActive;
				((DelegateCommand)StartIteratingCommand).IsActive = _isActive;
				((DelegateCommand)StopIteratingCommand).IsActive = _isActive;
				((DelegateCommand)RestartCommand).IsActive = _isActive;
			}
		}

		public event EventHandler IsActiveChanged = delegate { };

		protected virtual void OnIsActiveChanged()
		{
			IsActiveChanged.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Timer and iteration

		private void Iterate()
		{
			if (Automaton == null)
				return;

			using (Application.Current.Dispatcher.DisableProcessing())
			{
				var statistics = new Statistics();
				var result = Automaton.Iterate(ref statistics);

				UpdateChartRepository(statistics);

				// if there are no cells that change their state - reinit automaton
				if (result && statistics.Changed == 0)
				{
					Automaton.Clear();
					AutomatonSetPoints(InitialAutomaton());
				}
			}
		}

		protected void PauseIteration()
		{
			if (_iterateTimer.IsEnabled)
			{
				_postponedTimer = true;
				_iterateTimer.Stop();
			}
		}

		protected void ContinueIteration(bool updateCommands = false)
		{
			if (_postponedTimer)
				StartTimer();
			else if (updateCommands)
			{
				SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.Init);
			}
		}

		/// <summary>
		/// Start the timer for automaton iterations. But method could have not effect
		/// if automaton still waits for all necessary data for creating. Then timer will
		/// start after automaton has been created.
		/// </summary>
		private void StartTimer()
		{
			if (Automaton == null || Cells == null)
				_postponedTimer = true;
			else
			{
				_iterateTimer.Start();
				_postponedTimer = false;
				SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.Iterating);
			}
		}

		/// <summary>
		/// Stop the timer for automaton iterations.
		/// </summary>
		private void StopTimer()
		{
			_iterateTimer.Stop();
			SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.Init);
		}

		private void IterateTimerTick(object sender, EventArgs e)
		{
#if DEBUG
			_logger.Log("Timer ticks");
#endif
			Iterate();
		}

		private void UpdateChartRepository(Statistics statistics)
		{
			// update chart repository
			_chartRepository.AddTotalCellCount(Automaton.Count);
			_chartRepository.AddBornedCellCount(statistics.Borned);
			_chartRepository.AddDiedCellCount(statistics.Died);
			_chartRepository.UpdateAges(Automaton.AgeSeries);
		}

		private void SetAutomatonCommandProviderMode(AutomatonCommandProviderMode mode)
		{
			switch (mode)
			{
			case AutomatonCommandProviderMode.Init:
				CanIterate = true;
				CanStartIterating = true;
				CanStopIterating = false;
				CanRestart = true;
				break;

			case AutomatonCommandProviderMode.Iterating:
				CanIterate = false;
				CanStartIterating = false;
				CanStopIterating = true;
				CanRestart = true;
				break;

			case AutomatonCommandProviderMode.None:
			default:
				CanIterate = false;
				CanStartIterating = false;
				CanStopIterating = false;
				CanRestart = false;
				break;
			}
		}

		private enum AutomatonCommandProviderMode
		{
			None = 0,
			Init,
			Iterating,
		}

		/// <summary>
		/// Set initial automaton.
		/// </summary>
		private void SetInitialAutomaton()
		{
			// prevent timer's start until automaton will be reinit
			SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.None);
			PauseIteration();

			using (Application.Current.Dispatcher.DisableProcessing())
			{
				Automaton.Clear();
				AutomatonSetPoints(InitialAutomaton());
			}

			// post recreating actions
			ContinueIteration(true);
		}

		#endregion

		#region Resizing

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (string.Equals(propertyName, nameof(ViewHeight), StringComparison.InvariantCultureIgnoreCase) ||
				string.Equals(propertyName, nameof(ViewWidth), StringComparison.InvariantCultureIgnoreCase))
			{
				ImplementNewSize();
			}
		}

		/// <summary>
		/// Start timer when one of the view's dimensions is changed and wait for another.
		/// </summary>
		private void ImplementNewSize()
		{
			if (ViewHeight == 0 || ViewWidth == 0)
				return;

			if (_resizeTimer.IsEnabled)
				_resizeTimer.Stop();

			_resizeTimer.Start();
		}

		/// <summary>
		/// Method change automaton size due to change of view size.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizeTimerTick(object sender, EventArgs e)
		{
			_resizeTimer.Stop();

			if (ViewHeight == 0 || ViewWidth == 0)
				return;

			var automatonWidth = System.Math.Max(1, (int)System.Math.Ceiling((double)ViewWidth / Settings.Width));
			var automatonHeight = System.Math.Max(1, (int)System.Math.Ceiling((double)ViewHeight / Settings.Height));
			if (Automaton != null &&
				Automaton.Size.Width == automatonWidth &&
				Automaton.Size.Height == automatonHeight)
			{
				// the same size, nothing to do
				return;
			}

			// prevent timer's start until automaton will be recreate
			SetAutomatonCommandProviderMode(AutomatonCommandProviderMode.None);
			PauseIteration();

			// preserve current points
			var currentPoints = Automaton?.GetPoints().Where(point => point.X < automatonWidth && point.Y < automatonHeight);
			Automaton = new Automaton(automatonWidth, automatonHeight, CellLifeService, _logger);
			AutomatonWidth = Automaton.Size.Width;
			AutomatonHeight = Automaton.Size.Height;

			AutomatonSetPoints(currentPoints ?? InitialAutomaton());
			CreateOrUpdateCellViewModels();

			// post recreating actions
			ContinueIteration(true);
		}

		#endregion
	}
}
