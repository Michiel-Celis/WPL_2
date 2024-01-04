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
            CookieCounter.Text = (int.Parse(CookieCounter.Text) + 1).ToString();
        }

        /// Animations
        private void AnimateClick(Image image)
        {
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.9,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
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
