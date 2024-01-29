using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class RegisterSaleViewModel : ViewModelBase
    {
        private SoldItemModel _soldItem = new();

        public SoldItemModel SoldItem
        {
            get { return _soldItem; }
            set
            {
                _soldItem = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveSaleCommand { get; set; }
        public ICommand CancelSaleCommand { get; set; }

        public RegisterSaleViewModel()
        {
            SaveSaleCommand = new GenericCommand(SaveSale);
            CancelSaleCommand = new GenericCommand(CancelSave);
        }

        private void CancelSave()
        {
            throw new NotImplementedException();
        }

        private void SaveSale()
        {
            throw new NotImplementedException();
        }
    }
}
