using DofusCrafter.UI.Interfaces;
using DofusCrafter.UI.Iterators;
using DofusCrafter.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DofusCrafter.UI.Managers
{
    /// <summary>
    /// Manages navigation and dialog operations within the application.
    /// </summary>
    public class NavigationManager
    {
        /// <summary>
        /// Provides access to a container for service objects.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Represents the currently executing assembly.
        /// </summary>
        private readonly Assembly _currentAssembly;

        /// <summary>
        /// Event fired when the current view changes.
        /// </summary>
        public event Action? OnCurrentViewChanged;

        /// <summary>
        /// Iterator containing all the navigation page registered in the stack
        /// </summary>
        public ViewIterator NavigationStack { get; private set; }

        /// <summary>
        /// The currently selected view
        /// </summary>
        private ContentControl? _currentView;

        /// <summary>
        /// Gets or sets the curent view. Invoke <see cref="OnCurrentViewChanged"/> when the Current
        /// View changes
        /// </summary>
        public ContentControl? CurrentView
        {
            get => _currentView;

            private set
            {
                _currentView = value;
                OnCurrentViewChanged?.Invoke();
            }
        }

        /// <summary>
        /// Dictionary to hold open dialogs associated with their view models.
        /// </summary>
        public Dictionary<ViewModelBase, Window> OpenedDialogs { get; private set; } = [];

        /// <summary>
        /// Construct a new instance of the Navigation Manager
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
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

        /// <summary>
        /// Open a new dialog by the view name. Register it in <see cref="OpenedDialogs"/> with its view model caller and
        /// allow the sending of parameters to the dialog.
        /// </summary>
        /// <param name="viewName">The name of the view to open in the dialog</param>
        /// <param name="caller">The view model calling the view</param>
        /// <param name="parameters">The dictionnary of parameters to pass to the dialog</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
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

            window.Closing += delegate (object? sender, CancelEventArgs e)
            {
                CloseDialog(viewName);
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

            object? datacontext = view.DataContext;

            if (parameters is not null && datacontext is not null && datacontext is IDialogWithParameters dialogWithParameters)
            {
                dialogWithParameters.OnNavigatedTo(parameters);
            }

            if (datacontext is ViewModelBase viewModelBase)
            {
                viewModelBase.OnInit();
            }

            window.Content = view;

            OpenedDialogs.Add(caller, window);

            window.ShowDialog();
        }

        /// <summary>
        /// Close the given dialog by its name and passes parameters to its caller view model if the dictionnary
        /// of parameters is not null
        /// </summary>
        /// <param name="viewName">The name of the view in the dialog</param>
        /// <param name="parameters">The dictionnary of parameters</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
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

                if (parameters is not null)
                {
                    viewModel.OnNavigatedFrom(parameters);
                }

                try
                {
                    window.Close();
                }
                catch (InvalidOperationException e)
                {
                    // TODO: add logging saying window is already closed
                }

                OpenedDialogs.Remove(openedDialog.Key);
            }
        }

        /// <summary>
        /// Changes the current view and optionnaly add the new view in the view stack
        /// </summary>
        /// <param name="viewName">The name of the view to open</param>
        /// <param name="save">Chose to save the view in the navigation stack. false by default</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
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

                ViewModelBase currentViewModel = (ViewModelBase)CurrentView.DataContext;

                currentViewModel.OnInit();
            }
        }

        /// <summary>
        /// Verify if it is possible to navigate back in the navigation stack
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Navigate back in the navigation stack
        /// </summary>
        public void NavigateBack()
        {
            if (CanNavigateBack())
            {
                NavigationStack.MovePrevious(true);
                CurrentView = NavigationStack.Current;
            }
        }

        /// <summary>
        /// Verify if it is possible to navigate to the next view in the navigation stack
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Navigate to the next view in the navigation stack
        /// </summary>
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
