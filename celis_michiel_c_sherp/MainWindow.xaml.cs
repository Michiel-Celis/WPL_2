using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMouseDown = false;
        private int cookieCount = 0;
        public class MenuItem
        {
            public string Logo { get; set; }
            public string Title { get; set; }
            public string Price { get; set; }
            public Visibility ButtonVisibility { get; set; }
            public bool ButtonIsEnabled { get; set; }
        }
        List<MenuItem> upgradesList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png"         , Title = "Cursor"      , Price = "100"     , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/grandma.png"        , Title = "Grandma"     , Price = "500"     , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = false },
            new MenuItem { Logo = "/res/farm.png"           , Title = "Farm"        , Price = "1000"    , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/mine.png"           , Title = "Mine"        , Price = "2000"    , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/factory.png"        , Title = "Factory"     , Price = "5000"    , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/bank.png"           , Title = "Bank"        , Price = "10000"   , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/temple.png"         , Title = "Temple"      , Price = "20000"   , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/wizard-tower.png"   , Title = "Wizard Tower", Price = "50000"   , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/shipment.png"       , Title = "Shipment"    , Price = "100000"  , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/alchemylab.png"     , Title = "Alchemy Lab" , Price = "200000"  , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true },
            new MenuItem { Logo = "/res/portal.png"         , Title = "Portal"      , Price = "500000"  , ButtonVisibility = Visibility.Visible, ButtonIsEnabled = true }
        };

        List<MenuItem> powerUpsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png", Title = "Cursor", Price = "100" },
            new MenuItem { Logo = "/res/grandma.png", Title = "Grandma", Price = "500" },
            new MenuItem { Logo = "/res/farm.png", Title = "Farm", Price = "1000" },
            new MenuItem { Logo = "/res/mine.png", Title = "Mine", Price = "2000" }
        };
        List<MenuItem> achievementsList = new List<MenuItem>
        {
            new MenuItem { Logo = "/res/cursor.png", Title = "Cursor", Price = "100" },
            new MenuItem { Logo = "/res/grandma.png", Title = "Grandma", Price = "500" },
            new MenuItem { Logo = "/res/farm.png", Title = "Farm", Price = "1000" },
            new MenuItem { Logo = "/res/mine.png", Title = "Mine", Price = "2000" }
        };


        public MainWindow()
        {
            InitializeComponent();

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
        }
        private void UpgradesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu("Upgrades", upgradesList);
        }

        private void PowerUpsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu("Power-ups", powerUpsList);
        }

        private void AchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowMenu("Achievements", achievementsList); 
        }

        private void ShowMenu(string header, List<MenuItem> items)
        {
            MenuHeader.Text = header;
            MenuItems.ItemsSource = items;
            Overlay.Visibility = Visibility.Visible;
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
