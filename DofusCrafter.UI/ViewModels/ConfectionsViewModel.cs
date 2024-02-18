using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class ConfectionsViewModel : ViewModelBase
    {
        private readonly NavigationManager _navigationManager;

        public ICommand RegisterConfectionCommand { get; private set; }

        public ConfectionsViewModel(NavigationManager navigationManager)
        {
            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _navigationManager = navigationManager;

            RegisterConfectionCommand = new GenericCommand(RegisterConfection);
        }

        private void RegisterConfection()
        {
            _navigationManager.OpenDialog("RegisterConfectionView", this);
        }
    }
}
