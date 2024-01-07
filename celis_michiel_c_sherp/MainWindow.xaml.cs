using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace celis_michiel_c_sherp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
		/// Declarations
		/// Timers
		DispatcherTimer timerCalc = new DispatcherTimer();
		DispatcherTimer timerAnim = new DispatcherTimer();
		private Stopwatch gameTimer = new Stopwatch();
		/// Constants
		private const double PriceIncreaseFactor = 1.15;
		public const double InitialCookies = 0;
		/// Properties
		// private Label TotalCookiesGeneratedLabel;
		private double totalCookiesEverCollected;
		public double TotalCookiesEverCollected
		{
			get { return totalCookiesEverCollected; }
			set
			{
				if (totalCookiesEverCollected != value)
				{
					totalCookiesEverCollected = value;
					OnPropertyChanged(nameof(TotalCookiesEverCollected));
				}
			}
		}
		private double cookieCount;
		public double CookieCount
		{
			get { return cookieCount; }
			set
			{
				if (cookieCount != value)
				{
					cookieCount = value;
					OnPropertyChanged(nameof(CookieCount));
				}
			}
		}
        private bool isMouseDown = false;
		/// Property Changed
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		/// Methods
		public void PurchaseUpgrade(MenuItem menuItem)
		{
			// Check if menuItem is null
			if (menuItem == null)
			{
				// If menuItem is null, log an error message and return
				Console.WriteLine("Error: No MenuItem found with the given title.");
				return;
			}

			if (CookieCount >= menuItem.Price)
			{
				// Reduce the cookie count by the price of the upgrade.
				CookieCount -= menuItem.Price;

				// Increase the purchased amount of the upgrade.
				menuItem.Purchased++;

				// Increase the price of the upgrade.
				menuItem.Price = (int)Math.Ceiling(menuItem.InitPrice * Math.Pow(PriceIncreaseFactor, menuItem.Purchased));

				// Update the enabled state of the buttons.
				UpdateButtonStates();

				// Update the visibility of the menu items.
            	UpdateMenuItemVisibility();

				UpdateCounters();
			}
		}
		public void PurchasePowerup(MenuItem menuItem)
		{
			// If this MenuItem has already been purchased, return without doing anything.
			if (menuItem.Purchased > 0)
			{
				return;
			}

			if (CookieCount >= menuItem.Price)
			{
				// Reduce the cookie count by the price of the powerup.
				CookieCount -= menuItem.Price;

				// Increase the purchased amount of the powerup.
				menuItem.Purchased++;

				// Setter to hide the button
				menuItem.IsPurchasable = false;

				// Update the enabled state of the buttons.
				UpdateButtonStates();

				// Update the visibility of the menu items.
				UpdateMenuItemVisibility();

				UpdateCounters();

				// Hide the powerup after it's bought.
				var grid = this.FindName(menuItem.Title + "Grid") as Grid;
				if (grid != null)
				{
					grid.Visibility = Visibility.Collapsed;
				}
			}
		}
		
		/// Menu
		/// Update the visibility of the menu items.
		public void UpdateMenuItemVisibility()
		{
			// Update the visibility of the first item.
			MenuItem firstItem = (MenuItem)MenuItems.Items[0];
			firstItem.ButtonVisibility = firstItem.IsPurchasable ? Visibility.Visible : Visibility.Collapsed;

			// Update the visibility of the rest of the items.
			for (int i = 1; i < MenuItems.Items.Count; i++)
			{
				MenuItem previousItem = (MenuItem)MenuItems.Items[i - 1];
				if (previousItem.Purchased > 0)
				{
					MenuItem currentItem = (MenuItem)MenuItems.Items[i];
					currentItem.ButtonVisibility = currentItem.IsPurchasable ? Visibility.Visible : Visibility.Collapsed;
				}
			}

			// Refresh the ItemsSource of the ListBox.
			MenuItems.Items.Refresh();
		}
        /// Menu Buttons
        private Button hiddenButton = new Button { Visibility = Visibility.Hidden };
        private Button lastClickedButton;
        private Dictionary<Button, (string, List<MenuItem>)> buttonParameters;

        /// Menu Items
		public class MenuItem : INotifyPropertyChanged
		{
			public string Logo { get; set; }
			public string Title { get; set; }
			private string description;
			public string Description
			{
				get { return description; }
				set
				{
					if (description != value)
					{
						description = value;
						OnPropertyChanged(nameof(Description));
					}
				}
			}
			private string descriptionTooltip;
			public string DescriptionTooltip
			{
				get { return descriptionTooltip; }
				set
				{
					if (descriptionTooltip != value)
					{
						descriptionTooltip = value;
						OnPropertyChanged(nameof(DescriptionTooltip));
					}
				}
			}

			public double CookiesPerSecond { get; set; }
			public int InitPrice { get; set; }
			private int price;
			public int Price
			{
				get { return price; }
				set
				{
					if (price != value)
					{
						price = value;
						OnPropertyChanged(nameof(Price));
					}
				}
			}
			private bool isPurchasable = true;
			public bool IsPurchasable
			{
				get { return isPurchasable; }
				set
				{
					if (isPurchasable != value)
					{
						isPurchasable = value;
						OnPropertyChanged(nameof(IsPurchasable));
					}
				}
			}
			private int purchased;
			public int Purchased
			{
				get { return purchased; }
				set
				{
					if (purchased != value)
					{
						purchased = value;
						OnPropertyChanged(nameof(Purchased));
					}
				}
			}
			private bool isPowerup = false;
			public bool IsPowerup
			{
				get { return isPowerup; }
				set
				{
					if (isPowerup != value)
					{
						isPowerup = value;
						OnPropertyChanged(nameof(IsPowerup));
					}
				}
			}

			private bool isAchievement = false;
			public bool IsAchievement
			{
				get { return isAchievement; }
				set
				{
					if (isAchievement != value)
					{
						isAchievement = value;
						OnPropertyChanged(nameof(IsAchievement));
					}
				}
			}

			private Visibility buttonVisibility;
			public Visibility ButtonVisibility
			{
				get { return buttonVisibility; }
				set
				{
					if (buttonVisibility != value)
					{
						buttonVisibility = value;
						OnPropertyChanged(nameof(ButtonVisibility));
					}
				}
			}
			private bool buttonIsEnabled;
			public bool ButtonIsEnabled
			{
				get { return buttonIsEnabled; }
				set
				{
					if (buttonIsEnabled != value)
					{
						buttonIsEnabled = value;
						OnPropertyChanged(nameof(ButtonIsEnabled));
					}
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;
			protected virtual void OnPropertyChanged(string propertyName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// Menu Lists
		/// Upgrades
		/// Upgrades can be bought multiple times.	
        List<MenuItem> upgradesList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"         , Title = "Cursor"      	, CookiesPerSecond = 0.1	, InitPrice = 15		, Price = 15		, Purchased = 0 	, ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"        , Title = "Grandma"     	, CookiesPerSecond = 1		, InitPrice = 100		, Price = 100		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"           , Title = "Farm"        	, CookiesPerSecond = 8		, InitPrice = 1100		, Price = 1100		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"           , Title = "Mine"        	, CookiesPerSecond = 47		, InitPrice = 12000		, Price = 12000		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/factory.png"        , Title = "Factory"     	, CookiesPerSecond = 260	, InitPrice = 130000	, Price = 130000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/bank.png"           , Title = "Bank"        	, CookiesPerSecond = 1400	, InitPrice = 1400000	, Price = 200000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/temple.png"         , Title = "Temple"      	, CookiesPerSecond = 7800	, InitPrice = 20000000	, Price = 500000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/wizardtower.png"  	, Title = "WizardTower"		, CookiesPerSecond = 9000	, InitPrice = 1000000	, Price = 1000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/shipment.png"       , Title = "Shipment"    	, CookiesPerSecond = 10000	, InitPrice = 20000000	, Price = 20000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/alchemylab.png"     , Title = "AlchemyLab" 		, CookiesPerSecond = 20000	, InitPrice = 50000000 	, Price = 50000000 	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/portal.png"         , Title = "Portal"      	, CookiesPerSecond = 500000	, InitPrice = 100000000	, Price = 100000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };
		/// Power-ups
		/// Power-ups can only be bought once.
		/// they appear when their corresponding Upgrade has been bought 50 times.
        List<MenuItem> powerUpsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"			, Title = "Parkinson Clicks", Description = "Cursors are twice as Speedy"		, Price = 150 		, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"		, Title = "Gym Grandmas"	, Description = "Grandmas are twice as powerfull"	, Price = 1000 		, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"			, Title = "Robot Farms"		, Description = "Farms are twice as Cookiefull"		, Price = 11000 	, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"			, Title = "Miner Grandmas"	, Description = "Mines are twice as rich"			, Price = 120000 	, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/factory.png"		, Title = "Slave Factories"	, Description = "Factory are twice as productive"	, Price = 1300000 	, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/bank.png"			, Title = "Inflation scammu", Description = "Banks are twice as Effective"		, Price = 20000000 	, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/temple.png"			, Title = "The Holy Book"	, Description = "Temples are twice as Subdueing"	, Price = 50000000	, Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/wizard-tower.png"	, Title = "Magic Powder"	, Description = "Wizards are twice as Hyped"		, Price = 100000000 , Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/shipment.png"		, Title = "Amazon.com"		, Description = "Shipments are twice as fast"		, Price = 200000000 , Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/alchemylab.png"		, Title = "Love Molecule"	, Description = "Labs are twice as wonderfull"		, Price = 500000000 , Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/portal.png"			, Title = "Stargate Crew"	, Description = "Portals are twice as Portaly"		, Price = 1000000000 , Purchased = 0 	, IsPowerup = true , ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };
		/// Achievements
		/// Achievements can not be bought.
		/// they appear when requirements are met.
        List<MenuItem> achievementsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/smiley.png"			, Title = "Code cheater"	, IsAchievement = true		, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/smiley.png"			, Title = "Fast Clicker"	, IsAchievement = true		, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };

		/// Constructor
        public MainWindow()
        {
            InitializeComponent();

			// gameTimer
			gameTimer.Start();

			// Use this to cheat the game
			cookieCount = InitialCookies;

			DataContext = this;
            /// Menu button Declaration
            lastClickedButton = hiddenButton;

            buttonParameters = new Dictionary<Button, (string, List<MenuItem>)>
            {
                { UpgradesButton, ("Upgrades", upgradesList) },
                { PowerUpsButton, ("Power-ups", powerUpsList) },
                { AchievementsButton, ("Achievements", achievementsList) },
                { hiddenButton, (null, null) }
            };
			UpdateCounters();
            /// Event handlers
            /// Cookie click valid click lambda handler
            CookieImage.MouseDown += (s, e) => { isMouseDown = true; CookieClick(); };
            CookieImage.MouseUp += (s, e) => isMouseDown = false;
            CookieImage.MouseEnter += (s, e) => { if (isMouseDown) CookieClick(); };

			// Set the timer interval to one second
			timerCalc.Interval = TimeSpan.FromMilliseconds(10);
			timerAnim.Interval = TimeSpan.FromSeconds(1);

			// Add an event handler for the timer tick event
			timerCalc.Tick += Timer_Tick;
			timerAnim.Tick += Timer_Click;

			// Start the timer
			timerCalc.Start();
			timerAnim.Start();

        }


        /// General Event Actions		
		/// MenuActions
        private void ShowMenu(Button clickedButton,string header, List<MenuItem> items)
        {
            MenuHeader.Text = header;
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
        private void UpgradesButton_Click(object sender, RoutedEventArgs e){ToggleMenu(UpgradesButton, "Upgrades", upgradesList);}
        private void PowerUpsButton_Click(object sender, RoutedEventArgs e){ToggleMenu(PowerUpsButton, "Power-ups", powerUpsList);}
        private void AchievementsButton_Click(object sender, RoutedEventArgs e){ToggleMenu(AchievementsButton, "Achievements", achievementsList);}
        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e){ToggleMenu(lastClickedButton, buttonParameters[lastClickedButton].Item1,buttonParameters[lastClickedButton].Item2);}
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e){ToggleMenu(lastClickedButton, buttonParameters[lastClickedButton].Item1,buttonParameters[lastClickedButton].Item2);}
		private void UpdateButtonStates()
		{
			foreach (var pair in buttonParameters)
			{
				var button = pair.Key;
				var menuItems = pair.Value.Item2;

				if (menuItems != null)
				{
					foreach (var menuItem in menuItems)
					{
						menuItem.ButtonIsEnabled = cookieCount >= menuItem.Price;
					}
				}
			}
		}
		/// Purchase handler
		private void PurchaseItem(object sender, RoutedEventArgs e)
		{
			// Get the button that was clicked.
			var button = (Button)sender;

			// Get the corresponding menu item.
			var menuItem = (MenuItem)button.DataContext;

			// Check if menuItem is null
			if (menuItem == null)
			{
				// If menuItem is null, log an error message and return
				Console.WriteLine("Error: No MenuItem found with the given title.");
				return;
			}

			// Check if the menuItem is in the powerUpsList
			if (powerUpsList.Contains(menuItem))
			{
				PurchasePowerup(menuItem);
			}
			else
			{
				PurchaseUpgrade(menuItem);
			}
		}
		
		/// 3x3 Grid Functions
		private void GridButton_Click(object sender, RoutedEventArgs e)
		{
			// Get the button that was clicked.
			var button = (Button)sender;

			// Get the title of the corresponding menu item from the button's tag.
			var title = (string)button.Tag;

			// Find the corresponding menu item in the list.
			var menuItem = upgradesList.FirstOrDefault(item => item.Title == title);

			// Increment the Purchased property
			PurchaseUpgrade(menuItem);

			// Find the Grid that contains the button.
			var grid = button.Parent as Grid;
			if (grid != null)
			{
				// Find the Image within the Grid.
				var image = grid.FindName(title + "Image") as Image;
				if (image != null)
				{
					// Animate the Image.
					AnimateClick(image);
				}
			}
		}
		public void UpdateCounters()
		{
			foreach (var menuItem in upgradesList)
			{
				var grid = this.FindName(menuItem.Title + "Grid") as Grid;
				if (grid != null)
				{
					grid.Visibility = menuItem.Purchased > 0 ? Visibility.Visible : Visibility.Collapsed;

					// Find the TextBlock within the Grid and update its Text property.
					var counter = grid.FindName(menuItem.Title + "Counter") as TextBlock;
					if (counter != null)
					{
						counter.Text = menuItem.Purchased.ToString();
					}
				}
			}
		}
		
		/// Timer actions
		private void Timer_Tick(object sender, EventArgs e)
		{
			// Calculate the total amplification factor from the power-ups
			int amplificationFactor = CalculateAmplificationFactor(powerUpsList);

			// Perform the calculation for each item in the upgrades list
			CalculateCookies(upgradesList, amplificationFactor);
		}
		private void Timer_Click(object sender, EventArgs e)
		{
			// Calculate the total amplification factor from the power-ups
			int amplificationFactor = CalculateAmplificationFactor(powerUpsList);

			// Calculate the total cookies per second considering both the upgrades purchased and the amplification factor
			double totalCookiesPerSecond = upgradesList.Sum(item => item.Purchased * item.CookiesPerSecond * amplificationFactor);
			TotalCookiesPerSecondLabel.Content = FormatLargeNumber(totalCookiesPerSecond).ToString();

			TotalCookiesGeneratedLabel.Content = FormatLargeNumber(TotalCookiesEverCollected).ToString();

			// Check if any item in upgradesList has been purchased
			if (upgradesList.Any(item => item.Purchased > 0))
			{
				AnimateClick(CookieImage);
			}

			CheckAchievements();
		}
        
		/// Result of actions
		/// Manual CookieClick
		private void CookieClick()
		{
			// Get the number of cursors purchased
			int cursorsPurchased = upgradesList[0].Purchased;

			// Multiply the number of cookies added by the number of cursors purchased
			CookieClick(1 + (int)(cursorsPurchased / 10));
			AnimateClick(CookieImage);
		}
		// Timer CookieClick
		private void CookieClick(double cookiesToAdd)
		{
			// Increase the number of cookies
			cookieCount += cookiesToAdd;

			// Increase the total number of cookies ever collected
			TotalCookiesEverCollected += cookiesToAdd;

			// Update the UI
			CookieCounter.Text = FormatLargeNumber(Math.Round(cookieCount,2)).ToString();
			this.Title = $"Cookie Clicker : {Math.Round(cookieCount,2)}";

			// Update the visibility of the investment buttons
			UpdateButtonStates();
		}
		/// Calculations
		private int CalculateAmplificationFactor(List<MenuItem> items)
		{
			int amplificationFactor = 1;

			foreach (var item in items)
			{
				// Add 2 to the amplification factor for each purchased power-up-
				amplificationFactor += item.Purchased * 2;
			}

			return amplificationFactor;
		}

		private void CalculateCookies(List<MenuItem> items, int amplificationFactor)
		{
			foreach (var item in items)
			{
				if (item.Purchased > 0)
				{
					// Calculate the number of cookies a single purchase adds per second
					double cookiesPerSecondSingle = item.CookiesPerSecond * amplificationFactor;
					double cookiesPer10msSingle = cookiesPerSecondSingle/100;
					// Add this number to the total cookie count
					CookieClick(cookiesPer10msSingle * item.Purchased);

					// Calculate the total number of cookies this item is currently adding per second
					double cookiesPerSecondTotal = cookiesPerSecondSingle * item.Purchased;

					// Update the item's description
					item.Description = $"Add {FormatLargeNumber(cookiesPerSecondSingle)} cookies/second";
					item.DescriptionTooltip = $"Generates {FormatLargeNumber(cookiesPerSecondTotal)} cookies per second.";
				}
			}
		}

		private void CheckAchievements()
		{
			// Check if the player has achieved more than 1000 cookies in the first 10 seconds.
			if (CookieCount > 1000 && gameTimer.Elapsed.TotalSeconds <= 10)
			{
				// Find the achievement in the achievementsList.
				MenuItem achievement = achievementsList.FirstOrDefault(a => a.Title == "Code cheater");

				// If the achievement was found, update its properties.
				if (achievement != null)
				{
					achievement.ButtonVisibility = Visibility.Visible;
					achievement.ButtonIsEnabled = true;
					
				}
			}

			if (CookieCount > 10000 && gameTimer.Elapsed.TotalSeconds <= 600)
			{
				// Find the achievement in the achievementsList.
				MenuItem achievement = achievementsList.FirstOrDefault(a => a.Title == "Fast Clicker");

				// If the achievement was found, update its properties.
				if (achievement != null)
				{
					achievement.ButtonVisibility = Visibility.Visible;
					achievement.ButtonIsEnabled = true;
				}
			}
		}
        
		public static string FormatLargeNumber(double number)
		{
			string[] suffixes = { "", " Miljoen", " Miljard", " Biljoen", " Biljard", " Triljoen" };

			if (number < 1000000 && number > 1000)
			{
				return string.Format("{0:n0}", number).Replace(",", " ");
			}

			int suffixIndex = 0;
			while (number >= 1000 && suffixIndex < suffixes.Length - 1)
			{
				number /= 1000;
				suffixIndex++;
			}

			return $"{number:0.##}{suffixes[suffixIndex]}";
		}

		private void BakeryTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var dialog = new InputDialog();

			if (dialog.ShowDialog() == true)
			{
				BakeryTitle.Text = dialog.Response;
			}
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// Calculate the new height to maintain the aspect ratio
			double newHeight = this.Width * (500.0 / 800.0);

			// Set the new height
			this.Height = newHeight;
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
