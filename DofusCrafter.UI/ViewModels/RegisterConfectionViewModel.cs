using DofusCrafter.UI.Commands;
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
    public class RegisterConfectionViewModel : ViewModelBase
    {
        private readonly DofusDBService _dofusDbService;

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

        private string _searchQuery = string.Empty;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SearchQueryChanged { get; set; }

        public RegisterConfectionViewModel(DofusDBService dofusDbService)
        {
            if (dofusDbService is null)
            {
                throw new ArgumentNullException(nameof(dofusDbService));
            }

            _dofusDbService = dofusDbService;

            SearchQueryChanged = new AsyncRelayCommand<TextChangedEventArgs>(OnSearchQueryChanged);
        }

        private async Task OnSearchQueryChanged(TextChangedEventArgs args)
        {
            var result = await _dofusDbService.SearchItemsAsync(((TextBox)args.Source).Text);

            Items = [.. result];
        }
    }
}
