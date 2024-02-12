using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly NavigationManager _navigationManager;

        private ContentControl _currentView = new HomeView();

        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand NavigateSalesCommand { get; private set; }
        public ICommand NavigateBackCommand { get; private set; }

        public ContentControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _currentView = value;
                NotifyPropertyChanged();
            }
        }


        public ShellViewModel(NavigationManager navigationManager)
        {
            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _navigationManager = navigationManager;

            _navigationManager.OnCurrentViewChanged += CurrentViewChanged;

            NavigateHomeCommand = new GenericCommand(NavigateHome);
            NavigateSalesCommand = new GenericCommand(NavigateSales);
            NavigateBackCommand = new GenericCommand(NavigateBack); 

            NavigateHomeCommand.Execute(this);
        }

        private void NavigateHome()
        {
            _navigationManager.Navigate("HomeView", true);
        }

        private void NavigateSales()
        {
            _navigationManager.Navigate("SalesView", true);
        }

        private void CurrentViewChanged()
        {
            if (_navigationManager is null)
            {
                throw new NullReferenceException(nameof(_navigationManager));
            }

            if (_navigationManager.CurrentView is null)
            {
                throw new NullReferenceException($"{nameof(_navigationManager.CurrentView)} is null");
            }

            CurrentView = _navigationManager.CurrentView;
        }

        private void NavigateBack()
        {
            if (_navigationManager.CanNavigateBack())
            {
                _navigationManager.NavigateBack();
            }
        }
    }
}
