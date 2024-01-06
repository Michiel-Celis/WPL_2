using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace celis_michiel_c_sherp
{
	public class Game
	{
		// Game state variables
		public string Title { get; set; }
		private int cookieCount;

		// Game entities
		public class MenuItem : INotifyPropertyChanged
		{
			private double _price;
			private int _purchaseCount;
			private bool _buttonIsEnabled;

			public string Logo { get; set; }
			public string Title { get; set; }
			public double BasePrice { get; set; }

			public double Price
			{
				get { return _price; }
				set
				{
					if (_price != value)
					{
						_price = value;
						OnPropertyChanged(nameof(Price));
					}
				}
			}

			public int PurchaseCount
			{
				get { return _purchaseCount; }
				set
				{
					if (_purchaseCount != value)
					{
						_purchaseCount = value;
						OnPropertyChanged(nameof(PurchaseCount));
					}
				}
			}

			public Visibility ButtonVisibility { get; set; }

			public bool ButtonIsEnabled
			{
				get { return _buttonIsEnabled; }
				set
				{
					if (_buttonIsEnabled != value)
					{
						_buttonIsEnabled = value;
						OnPropertyChanged(nameof(ButtonIsEnabled));
					}
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void OnPropertyChanged(string propertyName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			public void UpdatePrice()
			{
				Price = BasePrice * Math.Pow(1.15, PurchaseCount);
			}

			public void UpdateButtonIsEnabled(int cookieCount)
			{
				ButtonIsEnabled = cookieCount >= Price;
			}

			public event Action<MenuItem> PurchaseRequested = delegate { };

			public void OnPurchaseRequested()
			{
				PurchaseCount++;
				UpdatePrice();
				PurchaseRequested(this);
			}
		}
		private List<MenuItem> upgradesList;
		private List<MenuItem> powerUpsList;
		private List<MenuItem> achievementsList;

		// Constructor
		public Game()
		{
			// Initialize game state variables
			cookieCount = 0;

			// Initialize game entities
			upgradesList = new List<MenuItem>
			{
				new MenuItem { Logo = "/res/cursor.png"         , Title = "Cursor"          , BasePrice = 10    , Price = 10    , PurchaseCount = 0, ButtonVisibility = Visibility.Visible  , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/grandma.png"        , Title = "Grandma"         , BasePrice = 50    , Price = 50    , PurchaseCount = 0, ButtonVisibility = Visibility.Visible  , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/farm.png"           , Title = "Farm"            , BasePrice = 100   , Price = 100   , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/mine.png"           , Title = "Mine"            , BasePrice = 2000  , Price = 2000  , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/factory.png"        , Title = "Factory"         , BasePrice = 5000  , Price = 5000  , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/bank.png"           , Title = "Bank"            , BasePrice = 10000 , Price = 10000 , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/temple.png"         , Title = "Temple"          , BasePrice = 20000 , Price = 20000 , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/wizard-tower.png"   , Title = "Wizard Tower"    , BasePrice = 50000 , Price = 50000 , PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/shipment.png"       , Title = "Shipment"        , BasePrice = 100000, Price = 100000, PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/alchemylab.png"     , Title = "Alchemy Lab"     , BasePrice = 200000, Price = 200000, PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/portalmachine.png"  , Title = "Portal Machine"  , BasePrice = 500000, Price = 500000, PurchaseCount = 0, ButtonVisibility = Visibility.Hidden   , ButtonIsEnabled = false }
			};
			powerUpsList = new List<MenuItem>  
			{
				new MenuItem { Logo = "/res/cursor.png", Title = "Silver Cursor", BasePrice = 100, Price = 100, PurchaseCount = 0, ButtonVisibility = Visibility.Visible, ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/cursor.png", Title = "Silver Cursor", BasePrice = 100, Price = 100, PurchaseCount = 0, ButtonVisibility = Visibility.Visible, ButtonIsEnabled = false }
			};
			achievementsList = new List<MenuItem>
			{
				new MenuItem { Logo = "/res/cursor.png", Title = "Silver Cursor", BasePrice = 100, Price = 100, PurchaseCount = 0, ButtonVisibility = Visibility.Visible, ButtonIsEnabled = false },
				new MenuItem { Logo = "/res/cursor.png", Title = "Silver Cursor", BasePrice = 100, Price = 100, PurchaseCount = 0, ButtonVisibility = Visibility.Visible, ButtonIsEnabled = false }
			};
		}

		// Game methods
		public void ClickCookie()
		{
			AnimateClick(CookieImage);
			cookieCount++;
			CookieCounter.Text = cookieCount.ToString();
			this.Title = $"Cookie Clicker : {cookieCount}";

			ViewModel.CookieCount = cookieCount;

			foreach (var item in upgradesList)
			{
				item.UpdateButtonIsEnabled(cookieCount);
			}
			foreach (var item in powerUpsList)
			{
				item.UpdateButtonIsEnabled(cookieCount);
			}
			foreach (var item in achievementsList)
			{
				item.UpdateButtonIsEnabled(cookieCount);
			}
		}

		public void PurchaseUpgrade(MenuItem upgrade)
		{
			// Implement logic to purchase an upgrade
		}

		public void PurchasePowerUp(MenuItem powerUp)
		{
			// Implement logic to purchase a power-up
		}

		public void PurchaseAchievement(MenuItem achievement)
		{
			// Implement logic to purchase an achievement
		}

		public List<MenuItem> GetUpgradesList()
		{
			return upgradesList;
		}

		public List<MenuItem> GetPowerUpsList()
		{
			return powerUpsList;
		}

		public List<MenuItem> GetAchievementsList()
		{
			return achievementsList;
		}

		public int GetCookieCount()
		{
			return cookieCount;
		}
	}

	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
	public class YourViewModel : INotifyPropertyChanged
	{
		private Game _game;
		private Button lastClickedButton;
		private Dictionary<Button, Tuple<string, List<MenuItem>>> buttonParameters;


		public ICommand UpgradesCommand { get; private set; }
		public ICommand PowerUpsCommand { get; private set; }
		public ICommand AchievementsCommand { get; private set; }
		public ICommand OverlayCommand { get; private set; }
		public ICommand GridCommand { get; private set; }
		public ICommand CookieClickCommand { get; private set; }

		public event Action ImageClicked;

		public void OnImageClick()
		{
			ImageClicked?.Invoke();
		}

		public YourViewModel(Game game, List<MenuItem> powerUpsList, List<MenuItem> achievementsList)
		{
			_game = game;

			AllItems = new List<MenuItem>();
			AllItems.AddRange(powerUpsList);
			AllItems.AddRange(achievementsList);

			foreach (var item in AllItems)
			{
				item.PurchaseRequested += i =>
				{
					if (CookieCount >= i.Price)
					{
						CookieCount -= (int)i.Price;
					}
				};
			}

			// Initialize your variables
			lastClickedButton = null;
			buttonParameters = new Dictionary<Button, Tuple<string, List<MenuItem>>>();

			// Initialize your commands
			UpgradesCommand = new RelayCommand(param => ToggleMenu(UpgradesButton, "Upgrades", upgradesList));
			PowerUpsCommand = new RelayCommand(param => ToggleMenu(PowerUpsButton, "Power-ups", powerUpsList));
			AchievementsCommand = new RelayCommand(param => ToggleMenu(AchievementsButton, "Achievements", achievementsList));
			OverlayCommand = new RelayCommand(param => ToggleMenu(lastClickedButton, buttonParameters[lastClickedButton].Item1, buttonParameters[lastClickedButton].Item2));
			GridCommand = new RelayCommand(param => ToggleMenu(lastClickedButton, buttonParameters[lastClickedButton].Item1, buttonParameters[lastClickedButton].Item2));
			CookieClickCommand = new RelayCommand(param => ExecuteCookieClickCommand());	
		}

		private void ExecuteUpgradesCommand(object parameter)
		{
			_game.PurchaseUpgrade();
		}

		private void ExecutePowerUpsCommand(object parameter)
		{
			_game.PurchasePowerUp();
		}

		private void ExecuteAchievementsCommand(object parameter)
		{
			_game.PurchaseAchievement();
		}
		private void ExecuteCookieClickCommand()
		{
			CookieCount++;

			foreach (var item in UpgradesList)
			{
				item.UpdateButtonIsEnabled(CookieCount);
			}
			foreach (var item in PowerUpsList)
			{
				item.UpdateButtonIsEnabled(CookieCount);
			}
			foreach (var item in AchievementsList)
			{
				item.UpdateButtonIsEnabled(CookieCount);
			}
		}

		public int CookieCount
		{
			get { return _game.GetCookieCount(); }
		}

		public List<MenuItem> UpgradesList
		{
			get { return _game.GetUpgradesList(); }
		}

		public List<MenuItem> PowerUpsList
		{
			get { return _game.GetPowerUpsList(); }
		}

		public List<MenuItem> AchievementsList
		{
			get { return _game.GetAchievementsList(); }
		}

		public ICommand PurchaseUpgradeCommand
		{
			get { return new RelayCommand(param => _game.PurchaseUpgrade((MenuItem)param)); }
		}

		public ICommand PurchasePowerUpCommand
		{
			get { return new RelayCommand(param => _game.PurchasePowerUp((MenuItem)param)); }
		}

		public ICommand PurchaseAchievementCommand
		{
			get { return new RelayCommand(param => _game.PurchaseAchievement((MenuItem)param)); }
		}

		// Implement INotifyPropertyChanged interface...
	}
	public partial class MainWindow : Window
	{
		private YourViewModel _viewModel;

		public MainWindow()
		{
			InitializeComponent();

			// Instantiate ViewModel
			ViewModel = new YourViewModel(powerUpsList, achievementsList); // Initialize ViewModel property
			this.DataContext = ViewModel; // Use ViewModel here

			ViewModel.ImageClicked += () => AnimateClick(CookieImage);

			// Update UI when CookieCount changes
			ViewModel.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "CookieCount")
				{
					CookieCounter.Text = ViewModel.CookieCount.ToString();
					this.Title = $"Cookie Clicker : {ViewModel.CookieCount}";
				}
			};

			// Declarations
			lastClickedButton = hiddenButton;

			buttonParameters = new Dictionary<Button, (string, List<MenuItem>)>
			{
				{ UpgradesButton, ("Upgrades", upgradesList) },
				{ PowerUpsButton, ("Power-ups", powerUpsList) },
				{ AchievementsButton, ("Achievements", achievementsList) },
				{ hiddenButton, (null, null) }
			};

			// Event handlers
			CookieImage.MouseDown += (s, e) => { isMouseDown = true; ViewModel.CookieClickCommand.Execute(null); };
			CookieImage.MouseUp += (s, e) => isMouseDown = false;
			CookieImage.MouseEnter += (s, e) => { if (isMouseDown) ViewModel.CookieClickCommand.Execute(null); };
		}

		/// Event Actions
		/// CookieClick
		private void CookieButtonClick(object sender, RoutedEventArgs e)
		{
			AnimateClick(CookieImage);
			CookieCounter.Text = ViewModel.CookieCount.ToString();
			this.Title = $"Cookie Clicker : {ViewModel.CookieCount}";
		}
		/// MenuActions
		/// Menu Buttons
		private Button hiddenButton = new Button { Visibility = Visibility.Hidden };
		private Button lastClickedButton;
		private Dictionary<Button, (string, List<MenuItem>)> buttonParameters;
		/// Menu Items	
		private void ShowMenu(Button clickedButton,string header, List<MenuItem> items)
		{
			MenuHeader.Text = header;
			foreach (var item in items)
			{
				item.UpdateButtonIsEnabled(cookieCount);
			}
			MenuItems.ItemsSource = items;
			lastClickedButton = clickedButton;
			Overlay.Visibility = Visibility.Visible;
		}
		private void HideMenu()
		{
			lastClickedButton = hiddenButton;
			Overlay.Visibility = Visibility.Hidden;
		}
		private void ToggleMenu(Button clickedButton, string header = null, List<MenuItem> items = null)
		{
			if (lastClickedButton != clickedButton && buttonParameters.TryGetValue(clickedButton, out var parameters))
			{
				ShowMenu(clickedButton, parameters.Item1, parameters.Item2);
			}
			else
			{
				HideMenu();
			}
		}
		/// Animations
		private void AnimateClick(Image image)
		{
			var animation = new DoubleAnimation
			{
				From = 1.0,
				To = 0.9,
				Duration = new Duration(TimeSpan.FromMilliseconds(50)),
				AutoReverse = true
			};

			var transform = new ScaleTransform();
			image.RenderTransform = transform;
			image.RenderTransformOrigin = new Point(0.5, 0.5);
			transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
			transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
		}
	}
}
