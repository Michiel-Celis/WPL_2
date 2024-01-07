using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace celis_michiel_c_sherp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// Declarations
		DispatcherTimer timerCalc = new DispatcherTimer();
		DispatcherTimer timerAnim = new DispatcherTimer();
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

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void PurchaseUpgrade(MenuItem menuItem)
		{
			if (CookieCount >= menuItem.Price)
			{
				// Reduce the cookie count by the price of the upgrade.
				CookieCount -= menuItem.Price;

				// Increase the purchased amount of the upgrade.
				menuItem.Purchased++;

				// Increase the price of the upgrade.
				menuItem.Price = (int)Math.Ceiling(menuItem.initPrice * Math.Pow(1.15, menuItem.Purchased));

				// Update the enabled state of the buttons.
				UpdateButtonStates();

				// Update the visibility of the menu items.
            	UpdateMenuItemVisibility();
			}
		}
		public void UpdateMenuItemVisibility()
		{
			for (int i = 1; i < MenuItems.Items.Count; i++)
			{
				MenuItem previousItem = (MenuItem)MenuItems.Items[i - 1];
				if (previousItem.Purchased > 0)
				{
					MenuItem currentItem = (MenuItem)MenuItems.Items[i];
					currentItem.ButtonVisibility = Visibility.Visible;
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
			public int initPrice;
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
		/// Upgrades
		/// Upgrades can be bought multiple times.	
        List<MenuItem> upgradesList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"         , Title = "Cursor"      	, CookiesPerSecond = 0.1	, initPrice = 15		, Purchased = 0 	, ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"        , Title = "Grandma"     	, CookiesPerSecond = 1		, initPrice = 100		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"           , Title = "Farm"        	, CookiesPerSecond = 8		, initPrice = 1100		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"           , Title = "Mine"        	, CookiesPerSecond = 47		, initPrice = 12000		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/factory.png"        , Title = "Factory"     	, CookiesPerSecond = 260	, initPrice = 130000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/bank.png"           , Title = "Bank"        	, CookiesPerSecond = 500	, initPrice = 200000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/temple.png"         , Title = "Temple"      	, CookiesPerSecond = 1000	, initPrice = 500000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/wizard-tower.png"   , Title = "Wizard Tower"	, CookiesPerSecond = 5000	, initPrice = 1000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/shipment.png"       , Title = "Shipment"    	, CookiesPerSecond = 10000	, initPrice = 20000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/alchemylab.png"     , Title = "Alchemy Lab" 	, CookiesPerSecond = 20000	, initPrice = 50000000 	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/portal.png"         , Title = "Portal"      	, CookiesPerSecond = 500000	, initPrice = 100000000	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };
		/// Power-ups
		/// Power-ups can only be bought once.
		/// they appear when their corresponding Upgrade has been bought 50 times.
        List<MenuItem> powerUpsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"			, Title = "Parkinson Clicks", Description = "Cursors are twice as Speedy"		, Price = 10 		, Purchased = 0 	, ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"		, Title = "Gym Grandmas"	, Description = "Grandmas are twice as powerfull"	, Price = 50 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"			, Title = "Robot Farms"		, Description = "Farms are twice as Cookiefull"		, Price = 100 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"			, Title = "Miner Grandmas"	, Description = "Mines are twice as rich"			, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/factory.png"		, Title = "Slave Factories"	, Description = "Factory are twice as productive"	, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/bank.png"			, Title = "Inflation scammu", Description = "Banks are twice as Effective"		, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/temple.png"			, Title = "The Holy Book"	, Description = "Temples are twice as Subdueing"	, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/wizard-tower.png"	, Title = "Magic Powder"	, Description = "Wizards are twice as Hyped"		, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/shipment.png"		, Title = "Amazon.com"		, Description = "Shipments are twice as fast"		, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/alchemylab.png"		, Title = "Love Molecule"	, Description = "Labs are twice as wonderfull"		, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
			new MenuItem { Logo = "/res/portal.png"			, Title = "Stargate Crew"	, Description = "Portals are twice as Portaly"		, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };
		/// Achievements
		/// Achievements can not be bought.
		/// they appear when requirements are met.
        List<MenuItem> achievementsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"			, Title = "Cursor"		, initPrice = 10	, Price = 10 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"		, Title = "Grandma"		, initPrice = 10	, Price = 100 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"			, Title = "Farm"		, initPrice = 10	, Price = 100 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"			, Title = "Mine"		, initPrice = 10	, Price = 200 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };


        public MainWindow()
        {
            InitializeComponent();

            /// Declarations
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

        /// Event Actions
        /// Manual CookieClick
		private void CookieClick()
		{
			// Get the number of cursors purchased
			int cursorsPurchased = upgradesList[0].Purchased;

			// Multiply the number of cookies added by the number of cursors purchased
			CookieClick(1 + (int)(cursorsPurchased / 10));
			AnimateClick(CookieImage);
		}
		// Time CookieClick
        private void CookieClick(double cookiesToAdd)
        {
            cookieCount += cookiesToAdd;
            CookieCounter.Text = Math.Round(cookieCount,2).ToString();
            this.Title = $"Cookie Clicker : {Math.Round(cookieCount,2)}";

			UpdateButtonStates();
        }
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
		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Button clicked...");
			// Get the button that was clicked.
			var button = (Button)sender;

			// Get the corresponding menu item.
			var menuItem = (MenuItem)button.DataContext;

			// Purchase the upgrade.
			PurchaseUpgrade(menuItem);
			Debug.WriteLine("Button click handled.");
		}
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
			TotalCookiesPerSecondLabel.Content = totalCookiesPerSecond.ToString();

			// Check if any item in upgradesList has been purchased
			if (upgradesList.Any(item => item.Purchased > 0))
			{
				AnimateClick(CookieImage);
			}
		}
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
					item.Description = $"Add {cookiesPerSecondSingle} cookies/second";
					item.DescriptionTooltip = $"Generates {cookiesPerSecondTotal} cookies per second.";
				}
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
