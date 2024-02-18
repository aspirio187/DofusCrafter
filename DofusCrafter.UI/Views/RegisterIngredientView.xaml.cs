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

namespace DofusCrafter.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour RegisterIngredientView.xaml
    /// </summary>
    public partial class RegisterIngredientView : ContentControl
    {
        public RegisterIngredientView()
        {
            InitializeComponent();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Ensure sender is not null
            TextBox textBox = sender as TextBox ?? throw new NullReferenceException(nameof(sender));

            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }

            // Combine the current text with the previewed text
            string newText = textBox.Text + e.Text;

            // Check if the resulting text is a positive integer
            if (!int.TryParse(newText, out int result) || result <= 0)
            {
                e.Handled = true;
            }
        }
    }
}
