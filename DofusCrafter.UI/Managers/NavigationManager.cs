using DofusCrafter.UI.Iterators;
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

        public List<Window> OpenedDialogs { get; private set; } = [];

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

            var currentAsseblies = assemblies
                .SingleOrDefault(a => !string.IsNullOrEmpty(a.FullName) && a.FullName.Contains(currentDomain.FriendlyName));

            if (currentAsseblies is null)
            {
                throw new NullReferenceException($"The asseblies doesn't contain a {currentDomain.FriendlyName}");
            }

            _currentAssembly = currentAsseblies;

            NavigationStack = new ViewIterator();
        }

        public void OpenDialog(string viewName)
        {
            if (viewName is null)
            {
                throw new ArgumentNullException(nameof(viewName));
            }

            var window = new Window()
            {
                Name = viewName,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
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

            window.Content = view;

            OpenedDialogs.Add(window);

            window.ShowDialog();
        }

        public void CloseDialog(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException(nameof(viewName));
            }

            var openedDialog = OpenedDialogs.SingleOrDefault(o => o.Name.Equals(viewName));

            if (openedDialog is not null)
            {
                openedDialog.Close();
                OpenedDialogs.Remove(openedDialog);
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
