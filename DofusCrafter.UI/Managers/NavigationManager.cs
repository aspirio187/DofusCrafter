using DofusCrafter.UI.Interfaces;
using DofusCrafter.UI.Iterators;
using DofusCrafter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DofusCrafter.UI.Managers
{
    public class NavigationManager
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Assembly _currentAssembly;

        public event Action? OnCurrentViewChanged;

        public ViewIterator NavigationStack { get; private set; }

        private ContentControl? _currentView;

        public ContentControl? CurrentView
        {
            get => _currentView;

            private set
            {
                _currentView = value;
                OnCurrentViewChanged?.Invoke();
            }
        }

        public Dictionary<ViewModelBase, Window> OpenedDialogs { get; private set; } = [];

        public NavigationManager(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (AppDomain.CurrentDomain is null)
            {
                throw new NullReferenceException(nameof(AppDomain.CurrentDomain));
            }

            _serviceProvider = serviceProvider;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = currentDomain.GetAssemblies();

            Assembly? currentAssembly = assemblies
                .SingleOrDefault(a =>
                    a.FullName is not null &&
                    a.FullName.Contains(AppDomain.CurrentDomain.FriendlyName) &&
                    !a.FullName.Contains("resources", StringComparison.InvariantCultureIgnoreCase));

            if (currentAssembly is null)
            {
                throw new NullReferenceException($"The asseblies doesn't contain a {currentDomain.FriendlyName}");
            }

            _currentAssembly = currentAssembly;

            NavigationStack = new ViewIterator();
        }

        public void OpenDialog(string viewName, ViewModelBase caller, Dictionary<string, object>? parameters = null)
        {
            if (viewName is null)
            {
                throw new ArgumentNullException(nameof(viewName));
            }

            if (caller is null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            var window = new Window()
            {
                Name = viewName,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };


            ContentControl? view = NavigationStack[viewName];

            if (view is null)
            {
                Type? viewType = _currentAssembly.DefinedTypes.SingleOrDefault(t => t.Name.Equals(viewName));

                if (viewType is null)
                {
                    throw new NullReferenceException(nameof(_currentAssembly));
                }

                view = Activator.CreateInstance(viewType) as ContentControl;
            }

            if (view is null)
            {
                throw new NullReferenceException(nameof(view));
            }

            var datacontext = view.DataContext;

            if (parameters is not null && datacontext is not null && datacontext is IDialogWithParameters dialogWithParameters)
            {
                dialogWithParameters.OnNavigatedTo(parameters);
            }

            window.Content = view;

            OpenedDialogs.Add(caller, window);

            window.ShowDialog();
        }

        public void CloseDialog(string viewName, Dictionary<string, object>? parameters = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException(nameof(viewName));
            }

            KeyValuePair<ViewModelBase, Window> openedDialog = OpenedDialogs.SingleOrDefault(o => o.Value.Name.Equals(viewName));

            if (openedDialog.Value is not null)
            {
                var window = openedDialog.Value;
                var viewModel = openedDialog.Key;

                if (viewModel is null)
                {
                    throw new ArgumentNullException(nameof(viewModel));
                }

                if (window.Content is not ContentControl contentControl)
                {
                    throw new NullReferenceException($"The content control of the opened view {viewName} is null and should never be null");
                }

                viewModel.OnNavigatedFrom(parameters);

                window.Close();
                OpenedDialogs.Remove(openedDialog.Key);
            }
        }

        public void Navigate(string viewName, bool save = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException(viewName, nameof(viewName));
            }

            ContentControl? view = NavigationStack[viewName];

            if (view is null)
            {
                Type? viewType = _currentAssembly.DefinedTypes.SingleOrDefault(t => t.Name.Equals(viewName));

                if (viewType is null)
                {
                    throw new NullReferenceException(nameof(_currentAssembly));
                }

                view = Activator.CreateInstance(viewType) as ContentControl;
            }

            if (view is null)
            {
                throw new NullReferenceException(nameof(view));
            }

            if (save && !NavigationStack.Any(v => v.ToString().Equals(viewName)))
            {
                NavigationStack.Add(view);
                NavigationStack.MoveNext();

                CurrentView = view;
            }
        }

        public bool CanNavigateBack()
        {
            if (NavigationStack.Length == 0)
            {
                return false;
            }

            if (NavigationStack.Position == 0)
            {
                return false;
            }

            return true;
        }

        public void NavigateBack()
        {
            if (CanNavigateBack())
            {
                NavigationStack.MovePrevious();
                CurrentView = NavigationStack.Current;
            }
        }

        public bool CanNavigateNext()
        {
            if (NavigationStack.Length == 0)
            {
                return false;
            }

            if (NavigationStack.Position == NavigationStack.Length - 1)
            {
                return false;
            }

            return true;
        }

        public void NavigateNext()
        {
            if (CanNavigateNext())
            {
                NavigationStack.MoveNext();
                CurrentView = NavigationStack.Current;
            }
        }
    }
}
