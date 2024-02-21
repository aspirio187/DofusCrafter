using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Interfaces;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class RegisterSoldConfectionViewModel : ViewModelBase, IDialogWithParameters
    {
        /// <summary>
        /// The database service for the items sales
        /// </summary>
        private readonly ISalesService _saleService;

        /// <summary>
        /// The manager taking care of the navigation between the views
        /// </summary>
        private readonly NavigationManager _navigationManager;

        /// <summary>
        /// The selected confection in the previous view
        /// </summary>
        private ConfectionModel _selectedConfection = new();

        /// <summary>
        /// Gets or sets the selected confection
        /// </summary>
        public ConfectionModel SelectedConfection
        {
            get { return _selectedConfection; }
            set
            {
                _selectedConfection = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The quantity of items sold
        /// </summary>
        private string _quantity = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of item sold
        /// </summary>
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The unitary price of the sold item
        /// </summary>
        private string _price = string.Empty;

        /// <summary>
        /// Gets or sets the unitary price of the sold item
        /// </summary>
        public string Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The date and time at which the item was sold
        /// </summary>
        private DateTime _soldAt;

        /// <summary>
        /// Gets or sets the date and time at which the item was sold
        /// </summary>
        public DateTime SoldAt
        {
            get { return _soldAt; }
            set
            {
                _soldAt = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the time of the day at which the item was sold
        /// </summary>
        public string Time
        {
            get { return SoldAt.TimeOfDay.ToString("hh\\:mm"); }
            set
            {
                SoldAt = new DateTime(DateOnly.FromDateTime(SoldAt), TimeOnly.Parse(value));
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The command executed when the user click on the save button
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// The command executed when the user click on the cancel button
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Create an instance of the view model
        /// </summary>
        /// <param name="saleService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterSoldConfectionViewModel(ISalesService saleService, NavigationManager navigationManager)
        {
            if (saleService is null)
            {
                throw new ArgumentNullException(nameof(saleService));
            }

            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _saleService = saleService;
            _navigationManager = navigationManager;

            // Register commands
            SaveCommand = new GenericCommand(Save);
            CancelCommand = new GenericCommand(Cancel);
        }

        private void Save()
        {

        }

        /// <summary>
        /// Close the window and notify the caller view model that the data should not be reloaded
        /// </summary>
        private void Cancel()
        {
            _navigationManager.CloseDialog("RegisterSoldConfectionView", new Dictionary<string, object>
            {
                { "reload", false }
            });
        }

        /// <summary>
        /// Event handler executed when user navigates on this dialog
        /// </summary>
        /// <param name="parameters">The dictionnary containing all the parameters</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters["selectedConfection"] is not ConfectionModel confectionModel)
            {
                throw new InvalidCastException($"parameter selectedConfection is not of type {typeof(ConfectionModel)}");
            }

            SelectedConfection = confectionModel;
        }
    }
}
