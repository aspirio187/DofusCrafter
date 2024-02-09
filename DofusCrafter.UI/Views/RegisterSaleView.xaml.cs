using DofusCrafter.UI.ViewModels;
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
    /// Logique d'interaction pour RegisterSaleView.xaml
    /// </summary>
    public partial class RegisterSaleView : ContentControl
    {
        public RegisterSaleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for previewing text changes in the quantity TextBox.
        /// Ensures that only positive integers are allowed as input.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing the text composition data.</param>
        private void OnQuantityPreviewTextChanged(object sender, TextCompositionEventArgs e)
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

        /// <summary>
        /// Event handler for previewing text changes in the price TextBox.
        /// Ensures that only positive decimal numbers are allowed as input.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing the text composition data.</param>
        private void OnPricePreviewTextChanged(object sender, TextCompositionEventArgs e)
        {
            // Ensure sender is not null
            TextBox textBox = sender as TextBox ?? throw new NullReferenceException(nameof(sender));

            // Combine the current text with the previewed text
            string newText = textBox.Text + e.Text;

            // Check if the resulting text is a positive decimal number
            if (!decimal.TryParse(newText, out decimal result) || result <= 0)
            {
                e.Handled = true;
            }
        }
    }
}
