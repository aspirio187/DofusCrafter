using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly DofusDBService _dofusDBService;
        private readonly NavigationManager _navigationManager;

        public HomeViewModel(DofusDBService dofusDBService, NavigationManager navigationManager)
        {
            if (dofusDBService is null)
            {
                throw new ArgumentNullException(nameof(dofusDBService));
            }

            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _dofusDBService = dofusDBService;
            _navigationManager = navigationManager;
        }
    }
}
