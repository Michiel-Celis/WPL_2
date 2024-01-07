using System;
using System.Windows;

namespace celis_michiel_c_sherp
{
    public partial class InputDialog : Window
    {
        public string Response { get; set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Input.Text))
            {
                Response = Input.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("The name cannot be empty or whitespace.");
            }
        }
    }
}