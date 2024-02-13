using DofusCrafter.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DofusCrafter.UI.Controls
{
    /// <summary>
    /// Custom control for displaying error messages based on a property of a <see cref="ModelBase"/> object.
    /// </summary>
    public class ErrorDisplay : Control
    {
        /// <summary>
        /// Static constructor for the <see cref="ErrorDisplay"/> class.
        /// </summary>
        static ErrorDisplay()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(ErrorDisplay), new FrameworkPropertyMetadata(typeof(ErrorDisplay)));
        }

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty
                .Register(
                    nameof(Model),
                    typeof(ModelBase),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null, OnModelPropertyChanged));

        /// <summary>
        /// Gets or sets the model associated with the error display.
        /// </summary>
        public ModelBase Model
        {
            get => (ModelBase)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PropertyName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty
                .Register(
                    nameof(PropertyName),
                    typeof(string),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the property for which the error message is displayed.
        /// </summary>
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ErrorMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty
                .Register(
                    nameof(ErrorMessage),
                    typeof(string),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the error message to be displayed.
        /// </summary>
        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        /// <summary>
        /// Event handler for the PropertyChanged event of the model property. 
        /// Subscribes to PropertyChanged event to monitor changes in the model.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the event handler is attached.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> containing event data.</param>
        private static void OnModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ErrorDisplay control = (ErrorDisplay)d;
            if (e.NewValue is ModelBase newModel)
            {
                // Subscribe to PropertyChanged event to monitor changes in the model
                newModel.PropertyChanged += control.OnModelPropertyChanged;
            }
        }

        /// <summary>
        /// Event handler for the PropertyChanged event of the model instance.
        /// Updates the error message when a property of the model changes.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> containing event data.</param>
        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                throw new NullReferenceException(nameof(e.PropertyName));
            }

            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (sender is not ModelBase modelBase)
            {
                throw new InvalidCastException(
                    $"{nameof(sender)} must be of type or inherit of type {typeof(ModelBase)}");
            }

            if (modelBase.IsValid)
            {
                ErrorMessage = string.Empty;
                return;
            }

            System.ComponentModel.DataAnnotations.ValidationResult? validationResult =
                Model.ValidationResults.FirstOrDefault(vr => vr.MemberNames.Contains(e.PropertyName));

            if (validationResult is not null)
            {
                string? errorMessage = validationResult.ErrorMessage;

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ErrorMessage = errorMessage;
                }
            }
        }
    }
}
