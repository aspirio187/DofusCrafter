using DofusCrafter.UI.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace DofusCrafter.UI.Controls
{
    /// <summary>
    /// Represents a custom TextBox control with suggestion functionality.
    /// </summary>
    public class SuggestionTextBox : Control
    {
        /// <summary>
        /// Initializes static members of the <see cref="SuggestionTextBox"/> class.
        /// </summary>
        static SuggestionTextBox()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(
                    typeof(SuggestionTextBox),
                    new FrameworkPropertyMetadata(typeof(SuggestionTextBox)));
        }

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty
                .Register(
                    nameof(Text),
                    typeof(string),
                    typeof(SuggestionTextBox),
                    new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the text displayed in the SuggestionTextBox.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Identifies the SuggestionType dependency property.
        /// </summary>
        public static readonly DependencyProperty SuggestionTypeProperty =
            DependencyProperty.Register(
                nameof(SuggestionType),
                typeof(Type),
                typeof(SuggestionTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the type of suggestions displayed in the SuggestionTextBox.
        /// </summary>
        public Type SuggestionType
        {
            get => (Type)GetValue(SuggestionTypeProperty);
            set => SetValue(SuggestionTypeProperty, value);
        }

        /// <summary>
        /// Identifies the Suggestions dependency property.
        /// </summary>
        public static readonly DependencyProperty SuggestionsProperty =
            DependencyProperty
                .Register(
                    nameof(Suggestions),
                    typeof(IList),
                    typeof(SuggestionTextBox),
                    new FrameworkPropertyMetadata(
                        null,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the list of suggestions displayed in the SuggestionTextBox.
        /// </summary>
        public IList Suggestions
        {
            get => (IList)GetValue(SuggestionsProperty);
            set => SetValue(SuggestionsProperty, value);
        }

        /// <summary>
        /// Identifies the SelectedIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty
                .Register(
                    nameof(SelectedIndex),
                    typeof(int),
                    typeof(SuggestionTextBox),
                    new FrameworkPropertyMetadata(
                        -1,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        OnSelectedIndexChanged));

        /// <summary>
        /// Gets or sets the index of the selected suggestion in the SuggestionTextBox.
        /// </summary>
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        /// Identifies the IsOpen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty
                .Register(
                    nameof(IsOpen),
                    typeof(bool),
                    typeof(SuggestionTextBox),
                    new FrameworkPropertyMetadata(
                        false,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets a value indicating whether the suggestion popup is open.
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        /// <summary>
        /// Identifies the TextChangedCommand dependency property.
        /// </summary>
        public static readonly DependencyProperty TextChangedCommandProperty =
            DependencyProperty.Register(
                nameof(TextChangedCommand),
                typeof(ICommand),
                typeof(SuggestionTextBox));

        /// <summary>
        /// Gets or sets the command that is executed when the text changes.
        /// </summary>
        public ICommand TextChangedCommand
        {
            get => (ICommand)GetValue(TextChangedCommandProperty);
            set => SetValue(TextChangedCommandProperty, value);
        }

        /// <summary>
        /// Identifies the PlaceHolder dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty
                .Register(
                    nameof(PlaceHolder),
                    typeof(DataTemplate),
                    typeof(SuggestionTextBox));

        /// <summary>
        /// Gets or sets the data template used for the placeholder in the SuggestionTextBox.
        /// </summary>
        public DataTemplate PlaceHolder
        {
            get => (DataTemplate)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionTextBox"/> class.
        /// </summary>
        public SuggestionTextBox()
        {
            Loaded += new RoutedEventHandler(OnSuggestionTextBoxLoaded);
        }

        /// <summary>
        /// Called when a dependency property value has changed.
        /// </summary>
        /// <param name="e">The event data for the dependency property change event.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SuggestionTypeProperty)
            {
                // If the suggestion type changes, update the Suggestions property type
                if (SuggestionType != null)
                {
                    var genericListType = typeof(List<>).MakeGenericType(SuggestionType);
                    SetValue(SuggestionsProperty, Activator.CreateInstance(genericListType));
                }
            }
        }

        /// <summary>
        /// Event handler invoked when the selected index property changes.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SuggestionTextBox control = (SuggestionTextBox)d;

            if (e.NewValue is int newValue)
            {
                control.SetCurrentValue(IsOpenProperty, false);
            }
        }

        /// <summary>
        /// Event handler invoked when the custom control finished loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSuggestionTextBoxLoaded(object sender, RoutedEventArgs args)
        {
            if (GetTemplateChild("Suggestions") is TextBox textBox)
            {
                textBox.LostFocus += OnSuggestionTextBoxLostFocus;

                textBox.TextChanged += OnTextChanged;
            }

            if (GetTemplateChild("SuggestionsPopup") is Popup popup)
            {
                Window w = Window.GetWindow(popup); 

                w.Deactivated += delegate (object? s, EventArgs args)
                {
                    popup.SetCurrentValue(IsOpenProperty, false);
                };

                w.Activated += delegate (object? s, EventArgs args)
                {
                    popup.SetCurrentValue(IsOpenProperty, true);
                };
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                IsOpen = false;
            }
        }

        /// <summary>
        /// Event handler invoked when the text of the text box Suggestions changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedCommand?.Execute(e);
        }

        /// <summary>
        /// Event handler invoked when the text box Suggestions lose focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuggestionTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}
