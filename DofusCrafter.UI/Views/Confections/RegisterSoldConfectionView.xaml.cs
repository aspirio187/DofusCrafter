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
    /// Logique d'interaction pour RegisterSoldConfectionView.xaml
    /// </summary>
    public partial class RegisterSoldConfectionView : ContentControl
    {
        public RegisterSoldConfectionView()
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

        private void OnPreviewTimeTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int originalCaretIndex = textBox.CaretIndex; // Save the original caret index

            string newText = string.Concat(textBox.Text.AsSpan(0, textBox.SelectionStart), e.Text, textBox.Text.AsSpan(textBox.SelectionStart + textBox.SelectionLength));

            // Check if the input contains any non-digit characters
            if (!newText.All(char.IsDigit) && !newText.Contains(':'))
            {
                e.Handled = true; // Prevent non-digit characters
                return;
            }

            // Automatically add colon at the correct position if necessary
            if (newText.Length == 2 && !newText.Contains(":"))
            {
                newText += ":";
            }
            else if (newText.Length == 3 && newText[2] != ':')
            {
                newText = newText.Insert(2, ":");
            }

            // Check if the time string exceeds 8 characters
            if (newText.Length > 8)
            {
                e.Handled = true;
                return;
            }

            // Split the time string into hours, minutes, and seconds
            string[] timeParts = newText.Split(':');

            // Check if each unit (hours, minutes, seconds) consists of only 2 characters
            if (timeParts.Any(part => part.Length > 2))
            {
                e.Handled = true;
                return;
            }

            // Update the TextBox text
            textBox.Text = newText;

            // If the next characters in the string are a colon, set the caret index after the colon
            if (textBox.Text.Length > originalCaretIndex && textBox.Text.Length > textBox.CaretIndex + 1 && textBox.Text[textBox.CaretIndex + 1] == ':')
            {
                textBox.CaretIndex = originalCaretIndex + 1;
            }
            else
            {
                textBox.CaretIndex = newText.Length;
            }

            e.Handled = true; // Prevent the original text from being processed
        }
    }
}
