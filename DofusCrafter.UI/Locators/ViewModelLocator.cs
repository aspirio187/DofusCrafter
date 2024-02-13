using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DofusCrafter.UI.Locators
{
    /// <summary>
    /// A utility class for automatically connecting view models to views in WPF applications.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Represents a static field used to access the application's service provider for dependency injection.
        /// </summary>
        private static readonly IServiceProvider ServiceProvider = App.ServiceProvider;

        /// <summary>
        /// Gets the value of the AutoConnectedViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object to check.</param>
        /// <returns>The value of the AutoConnectedViewModel attached property.</returns>
        public static bool GetAutoConnectedViewModelProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoConnectedViewModelProperty);
        }

        /// <summary>
        /// Sets the value of the AutoConnectedViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object to set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetAutoConnectedViewModelProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoConnectedViewModelProperty, value);
        }

        /// <summary>
        /// The AutoConnectedViewModel attached property.
        /// </summary>
        public static readonly DependencyProperty AutoConnectedViewModelProperty = DependencyProperty
            .RegisterAttached(
                "AutoConnectedViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                new PropertyMetadata(false, AutoConnectedViewModelChanged)
            );

        /// <summary>
        /// Handles the change in the AutoConnectedViewModel property.
        /// </summary>
        /// <param name="obj">The dependency object whose property has changed.</param>
        /// <param name="e">The event arguments containing the property change information.</param>
        private static void AutoConnectedViewModelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            Type viewType = obj.GetType();

            if (viewType is null)
            {
                throw new NullReferenceException(nameof(viewType));
            }

            string viewTypeName = viewType.FullName ?? throw new NullReferenceException(nameof(viewType.FullName));
            string viewModelTypeName = string.Concat(viewTypeName.Split('.').Last(), "Model");

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            System.Reflection.Assembly? currentAssembly = assemblies
                .SingleOrDefault(a =>
                    a.FullName is not null &&
                    a.FullName.Contains(AppDomain.CurrentDomain.FriendlyName) &&
                    !a.FullName.Contains("resources", StringComparison.InvariantCultureIgnoreCase));

            if (currentAssembly is null)
            {
                throw new NullReferenceException(nameof(currentAssembly));
            }

            Type viewModelType = currentAssembly
                .DefinedTypes
                .SingleOrDefault(t => t.Name.Equals(viewModelTypeName)) ??
                    throw new NullReferenceException(nameof(viewModelTypeName));

            object viewModel = ServiceProvider.GetService(viewModelType) ??
                throw new NullReferenceException(nameof(viewModelType));

            ((FrameworkElement)obj).DataContext = viewModel;
        }
    }
}
