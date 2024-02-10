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
    public class ErrorDisplay : Control
    {
        static ErrorDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorDisplay), new FrameworkPropertyMetadata(typeof(ErrorDisplay)));
        }

        // DependencyProperty for the model property to monitor for errors
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty
                .Register(
                    nameof(Model),
                    typeof(ModelBase),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null, OnModelPropertyChanged));

        // Model property to monitor for errors
        public ModelBase Model
        {
            get => (ModelBase)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        // DependencyProperty for the error message
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty
                .Register(
                    nameof(PropertyName),
                    typeof(string),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null));

        // Error message property
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        // DependencyProperty for the error message
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty
                .Register(
                    nameof(ErrorMessage),
                    typeof(string),
                    typeof(ErrorDisplay),
                    new PropertyMetadata(null));

        // Error message property
        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        // Callback method when the model property changes
        private static void OnModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ErrorDisplay)d;
            if (e.NewValue is ModelBase newModel)
            {
                // Subscribe to PropertyChanged event to monitor changes in the model
                newModel.PropertyChanged += control.Model_PropertyChanged;
            }
        }

        // Event handler for PropertyChanged event of the model
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                throw new NullReferenceException(nameof(e.PropertyName));
            }

            if (!Model.IsValid)
            {
                System.ComponentModel.DataAnnotations.ValidationResult? validationResult =
                    Model.ValidationResults.FirstOrDefault(vr => vr.MemberNames.Contains(e.PropertyName));

                if (validationResult is not null)
                {
                    string? errorMessage = validationResult.ErrorMessage;

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ((TextBlock)sender).Text = errorMessage;
                    }
                }
            }
        }
    }
}
