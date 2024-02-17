using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Mappers;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Models.Dtos;
using DofusCrafter.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;
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

        /// <summary>
        /// Represent the item search query entered by the user
        /// </summary>
        private string _searchQuery = string.Empty;

        /// <summary>
        /// Gets or sets the search query
        /// </summary>
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;

                if (SelectedIndex != -1 && Items.Count > 0)
                {
                    SelectRecipeAsync(_selectedIndex);
                }

                NotifyPropertyChanged();
            }
        }

        private RecipeDto _recipe = new();

        public RecipeDto Recipe
        {
            get { return _recipe; }
            set
            {
                _recipe = value;
                NotifyPropertyChanged();
            }
        }



        /// <summary>
        /// Command executed when the search query changes
        /// </summary>
        public ICommand SearchQueryChangedCommand { get; set; }

        /// <summary>
        /// Create a new instance of the <see cref="RegisterConfectionViewModel"/> 
        /// </summary>
        /// <param name="dofusDbService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterConfectionViewModel(DofusDBService dofusDbService)
        {
            if (dofusDbService is null)
            {
                throw new ArgumentNullException(nameof(dofusDbService));
            }

            _dofusDbService = dofusDbService;

            SearchQueryChangedCommand = new AsyncRelayCommand<TextChangedEventArgs?>(OnSearchQueryChanged);
        }

        /// <summary>
        /// Event handler invoked when <see cref="SearchQueryChangedCommand"/> is executed
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task OnSearchQueryChanged(TextChangedEventArgs? args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            ItemModel[] result = await _dofusDbService.SearchItemsAsync(((TextBox)args.Source).Text);

            Items = [.. result];
        }

        private async Task SelectRecipeAsync(int index)
        {
            if (index < 0)
            {
                return;
            }

            ItemModel item = Items[index];

            if (item is null)
            {
                throw new NullReferenceException(nameof(item));
            }

            int itemId = item.Id;

            RecipeModel? recipeFromService = await _dofusDbService.GetItemRecipeAsync(itemId);

            if (recipeFromService is null)
            {
                return;
            }

            Recipe = recipeFromService.MapToRecipeDto();
        }
    }
}
