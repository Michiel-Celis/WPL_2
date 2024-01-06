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

namespace celis_michiel_c_sherp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// Declarations
		private int cookieCount;
		public int CookieCount
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
				menuItem.Price = (int)(menuItem.initPrice * Math.Pow(1.15, menuItem.Purchased));

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
        List<MenuItem> upgradesList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"         , Title = "Cursor"      , initPrice = 10	, Price = 10     	, Purchased = 0 	, ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"        , Title = "Grandma"     , initPrice = 50	, Price = 50    	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"           , Title = "Farm"        , initPrice = 100	, Price = 100   	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"           , Title = "Mine"        , initPrice = 200	, Price = 200   	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/factory.png"        , Title = "Factory"     , initPrice = 500	, Price = 500    	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/bank.png"           , Title = "Bank"        , initPrice = 1000	, Price = 1000   	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/temple.png"         , Title = "Temple"      , initPrice = 2000	, Price = 2000   	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/wizard-tower.png"   , Title = "Wizard Tower", initPrice = 5000	, Price = 5000   	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/shipment.png"       , Title = "Shipment"    , initPrice = 10000	, Price = 10000  	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/alchemylab.png"     , Title = "Alchemy Lab" , initPrice = 20000	, Price = 20000  	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/portal.png"         , Title = "Portal"      , initPrice = 50000	, Price = 50000  	, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };

        List<MenuItem> powerUpsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"			, Title = "Cursor"		, initPrice = 10	, Price = 10 		, Purchased = 0 	, ButtonVisibility = Visibility.Visible	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/grandma.png"		, Title = "Grandma"		, initPrice = 10	, Price = 50 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"			, Title = "Farm"		, initPrice = 10	, Price = 100 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/mine.png"			, Title = "Mine"		, initPrice = 10	, Price = 2000 		, Purchased = 0 	, ButtonVisibility = Visibility.Hidden	, ButtonIsEnabled = false }
        };
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
			DataContext = this;
            /// Declarations
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

        }

        /// Event Actions
        /// CookieClick
        private void CookieClick()
        {
            AnimateClick(CookieImage);
            cookieCount++;
            CookieCounter.Text = cookieCount.ToString();
            this.Title = $"Cookie Clicker : {cookieCount}";

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
