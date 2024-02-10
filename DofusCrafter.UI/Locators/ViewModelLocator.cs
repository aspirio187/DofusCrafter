using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DofusCrafter.UI.Locators
{
    public class ViewModelLocator
    {
        private static readonly IServiceProvider ServiceProvider = App.ServiceProvider;

        public static bool GetAutoConnectedViewModelPRoperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoConnectedViewModelProperty);
        }

        public static void SetAutoConnectedViewModelProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoConnectedViewModelProperty, value);
        }

        public static readonly DependencyProperty AutoConnectedViewModelProperty = DependencyProperty
            .RegisterAttached(
                "AutoConnectedViewModel",
                    typeof(bool),
                    typeof(ViewModelLocator),
                    new PropertyMetadata(false, AutoConnectedViewModelChanged)
            );

        private static void AutoConnectedViewModelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
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
