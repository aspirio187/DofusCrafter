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
    public class SalesViewModel : ViewModelBase
    {
        private readonly NavigationManager _navigationManager;

        public ICommand RegisterSaleCommand { get; private set; }

        public SalesViewModel(NavigationManager navigationManager)
        {
            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _navigationManager = navigationManager;

            RegisterSaleCommand = new GenericCommand(RegisterSale);
        }

        public void RegisterSale()
        {
            _navigationManager.OpenDialog("RegisterSaleView", this);
        }
    }
}
