using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    /// <summary>
    /// View model response for the RegisterSale view <see cref="Views.RegisterSaleView"/>
    /// Manages the sales by registering a sold item
    /// </summary>
    public class RegisterSaleViewModel : ViewModelBase
    {
        /// <summary>
        /// The service accessing DofusDB api
        /// </summary>
        private readonly DofusDBService _dofusDbService;

        /// <summary>
        /// The navigation manager that handles navigation state in the app
        /// </summary>
        private readonly NavigationManager _navigationManager;

        /// <summary>
        /// The model representing the item being sold.
        /// </summary>
        private SoldItemModel _soldItem = new();

        /// <summary>
        /// Gets or sets the model representing the item being sold and notify the UI if
        /// a modification happened
        /// </summary>
        public SoldItemModel SoldItem
        {
            get { return _soldItem; }
            set
            {
                _soldItem = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The list of loaded items from DofusDB api based on the text entered in the sold item name
        /// </summary>
        private List<ItemModel> _items = [];

        /// <summary>
        /// Gets or sets the list of items available for sale. Notify the UI if a change occured to the property.
        /// If the list of items to set is greater than 0, set <see cref="ItemsPopupIsOpen"/> to true.
        /// </summary>
        public List<ItemModel> Items
        {
            get { return _items; }
            set
            {
                if (value.Count > 0)
                {
                    ItemsPopupIsOpen = true;
                }

                _items = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the list of items popup is open.
        /// </summary>
        private bool _itemsPopupIsOpen = false;

        /// <summary>
        /// Gets or sets a value indicating whether the items popup is open. Notify the UI if a change occured 
        /// to the property.
        /// </summary>
        public bool ItemsPopupIsOpen
        {
            get { return _itemsPopupIsOpen; }
            set
            {
                _itemsPopupIsOpen = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The index of the selected item in the list <see cref="Items"/>.
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Gets or sets the index of the selected item. Notify the UI if a change occured to the property.
        /// When setting the property, if <see cref="ItemsPopupIsOpen"/> is true, set it to false amd if the 
        /// new value is greater or equals to 0 or smaller than <see cref="Items"/> length, set the name and image
        /// of <see cref="SoldItem"/> to the values of the selected item.
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (ItemsPopupIsOpen)
                {
                    ItemsPopupIsOpen = false;
                }

                if (value >= 0 && value < Items.Count)
                {
                    ItemModel itemModel = Items[value];

                    if (itemModel is not null)
                    {
                        SoldItem.Name = itemModel.Slug.Fr;
                        SoldItem.Image = itemModel.Img;
                    }
                }

                _selectedIndex = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to save a sale
        /// </summary>
        public ICommand SaveSaleCommand { get; set; }

        /// <summary>
        /// Command to cancel the sale
        /// </summary>
        public ICommand CancelSaleCommand { get; set; }

        /// <summary>
        /// Command executed when a change occurs in the name text
        /// </summary>
        public ICommand ItemNameChangedCommand { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="RegisterSaleViewModel"/>
        /// </summary>
        /// <param name="dofusDbService">
        /// The service that interacts with the DofusDB website API
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterSaleViewModel(DofusDBService dofusDbService, NavigationManager navigationManager)
        {
            if (dofusDbService is null)
            {
                throw new ArgumentNullException(nameof(dofusDbService));
            }

            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _dofusDbService = dofusDbService;
            _navigationManager = navigationManager;

            // Initialize the different commands

            SaveSaleCommand = new GenericCommand(SaveSale);
            CancelSaleCommand = new GenericCommand(CancelSave);
            ItemNameChangedCommand = new AsyncRelayCommand<TextChangedEventArgs>(LoadItemsAsync);
        }

        /// <summary>
        /// Asynchronously loads items based on the text entered.
        /// </summary>
        /// <param name="args">
        /// The event arguments containing the text entered.
        /// </param>
        public async Task LoadItemsAsync(TextChangedEventArgs? args)
        {
            var result = await _dofusDbService.SearchItemsAsync(((TextBox)args.Source).Text);

            Items = [.. result];
        }

        /// <summary>
        /// Cancel the save by closing the current dialog window
        /// </summary>
        private void CancelSave()
        {
            _navigationManager.CloseDialog("RegisterSaleView");
        }

        private void SaveSale()
        {
            throw new NotImplementedException();
        }
    }
}
