﻿using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Mappers;
using DofusCrafter.UI.Models.DofusDb;
using DofusCrafter.UI.Models.Dtos;
using DofusCrafter.UI.Models.Forms;
using DofusCrafter.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    public class RegisterConfectionViewModel : ViewModelBase
    {
        /// <summary>
        /// The DofusDB service
        /// </summary>
        private readonly DofusDBService _dofusDbService;

        /// <summary>
        /// The navigation manager
        /// </summary>
        private readonly NavigationManager _navigationManager;

        /// <summary>
        /// Database service taking care of everything related to the confection
        /// </summary>
        private readonly IConfectionService _confectionService;

        /// <summary>
        /// The list of loaded items from DofusDB api based on the text entered in the sold item name
        /// </summary>
        private ObservableCollection<ItemModel> _items = [];

        /// <summary>
        /// Gets or sets the list of items available for sale. Notify the UI if a change occured to the property.
        /// If the list of items to set is greater than 0, set <see cref="ItemsPopupIsOpen"/> to true.
        /// </summary>
        public ObservableCollection<ItemModel> Items
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

        /// <summary>
        /// Represent the selected element in the list of items <see cref="Items"/>
        /// </summary>
        private int _selectedIndex;

        /// <summary>
        /// Gets or sets the selected index in the <see cref="Items"/> list
        /// </summary>
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

        /// <summary>
        /// The recipe of the currently selected item
        /// </summary>
        private RecipeDto _recipe = new();

        /// <summary>
        /// Gets or sets the recipe of the selected item
        /// </summary>
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
        /// The list of registered ingredients by the user
        /// </summary>
        private ObservableCollection<RegisteredIngredientDto> _registeredIngredients = [];

        /// <summary>
        /// Gets or sets the list of registered ingredients by the user
        /// </summary>
        public ObservableCollection<RegisteredIngredientDto> RegisteredIngredients
        {
            get { return _registeredIngredients; }
            set { _registeredIngredients = value; }
        }

        /// <summary>
        /// The quantity of item created
        /// </summary>
        private string _quantity;

        /// <summary>
        /// Gets or sets the quantity of item created
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
        /// The date and time at which the item is created
        /// </summary>
        private DateTime _createdAt = DateTime.Now;

        /// <summary>
        /// Gets or sets the date and time at which the item is created
        /// </summary>
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set
            {
                _createdAt = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The time at which the 
        /// </summary>
        private string _time = DateTime.Now.TimeOfDay.ToString("hh\\:mm");

        public string Time
        {
            get { return CreatedAt.TimeOfDay.ToString(); }
            set
            {
                CreatedAt = new DateTime(DateOnly.FromDateTime(CreatedAt) , TimeOnly.Parse(value));
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Command executed when the user click on register an ingredient
        /// </summary>
        public ICommand RegisterIngredientCommand { get; private set; }

        /// <summary>
        /// Command executed when the search query changes
        /// </summary>
        public ICommand SearchQueryChangedCommand { get; private set; }

        /// <summary>
        /// Command executed when the save confection button is clicked
        /// </summary>
        public ICommand SaveConfectionCommand { get; private set; }

        /// <summary>
        /// Command executed when the cancel button is clicked
        /// </summary>
        public ICommand CancelConfectionCommand { get; private set; }

        /// <summary>
        /// Create a new instance of the <see cref="RegisterConfectionViewModel"/> 
        /// </summary>
        /// <param name="dofusDbService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterConfectionViewModel(DofusDBService dofusDbService, NavigationManager navigationManager,
            IConfectionService confectionService)
        {
            if (dofusDbService is null)
            {
                throw new ArgumentNullException(nameof(dofusDbService));
            }

            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            if (confectionService is null)
            {
                throw new ArgumentNullException(nameof(confectionService));
            }

            // Registering injected services
            _dofusDbService = dofusDbService;
            _navigationManager = navigationManager;
            _confectionService = confectionService;

            // Registering commands
            SearchQueryChangedCommand = new AsyncRelayCommand<TextChangedEventArgs?>(OnSearchQueryChanged);
            RegisterIngredientCommand = new RelayCommand<int>(RegisterIngredient);
            SaveConfectionCommand = new GenericCommand(SaveConfection);
            CancelConfectionCommand = new GenericCommand(CancelConfection);
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

        /// <summary>
        /// Load the recipe of the selected object based on the index of the object in the list of Items
        /// </summary>
        /// <param name="index">The index of the selected item in the list of items <see cref="Items"/></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        private async void SelectRecipeAsync(int index)
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

        /// <summary>
        /// Open a new dialog to register the currently selected ingredient
        /// </summary>
        /// <param name="id">The id of the item</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private void RegisterIngredient(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            IngredientDto? selectedIngredient = Recipe.Ingredients.SingleOrDefault(i => i.Id == id);

            if (selectedIngredient is null)
            {
                throw new NullReferenceException(nameof(selectedIngredient));
            }

            var parameters = new Dictionary<string, object>()
            {
                { "ingredient", selectedIngredient }
            };

            _navigationManager.OpenDialog("RegisterIngredientView", this, parameters);
        }

        /// <summary>
        /// Retrieves the different parameters from <paramref name="parameters"/> (ingredient id, quantity, total price)
        /// and add the object to the list of registered ingredients <see cref="RegisteredIngredients"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public override void OnNavigatedFrom(Dictionary<string, object>? parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters["ingredientId"] is not int ingredientId)
            {
                throw new InvalidCastException($"parameter \"ingredientId\" is not of type int");
            }

            if (ingredientId < 0)
            {
                throw new IndexOutOfRangeException(nameof(ingredientId));
            }

            if (parameters["quantity"] is not int quantity)
            {
                throw new InvalidCastException($"parameter \"quantity\" is not of type int");
            }

            if (parameters["totalPrice"] is not int totalPrice)
            {
                throw new InvalidCastException($"parameter \"totalPrice\" is not of type int");
            }

            IngredientDto? registeredIngredient = Recipe.Ingredients.SingleOrDefault(i => i.Id == ingredientId);

            if (registeredIngredient is null)
            {
                throw new NullReferenceException(nameof(registeredIngredient));
            }

            RegisteredIngredientDto? existingRegisteredIngredient =
                RegisteredIngredients.SingleOrDefault(ri => ri.SelectedIngredient.Id == ingredientId);

            if (existingRegisteredIngredient is null)
            {
                RegisteredIngredientDto ingredientToRegister = new RegisteredIngredientDto()
                {
                    SelectedIngredient = registeredIngredient,
                    TotalPrice = totalPrice,
                    QuantityRegistered = quantity
                };

                RegisteredIngredients.Add(ingredientToRegister);
            }
            else
            {
                RegisteredIngredients.Remove(existingRegisteredIngredient);

                existingRegisteredIngredient.TotalPrice += totalPrice;
                existingRegisteredIngredient.QuantityRegistered += quantity;

                RegisteredIngredients.Add(existingRegisteredIngredient);
            }
        }

        /// <summary>
        /// Save the registered confection in the database and close this dialog. Tells the parent view to reload its list
        /// </summary>
        private void SaveConfection()
        {
            if (!int.TryParse(Quantity, out int quantity))
            {
                throw new InvalidCastException("The quantity should be an integer!");
            }

            ConfectionForm confectionForm = new ConfectionForm()
            {
                ItemId = Recipe.Id,
                Slug = Recipe.Slug,
                Quantity = quantity,
                CreatedAt = CreatedAt,
                ConfectionIngredients = RegisteredIngredients
                    .Select(r => new ConfectionIngredientForm()
                    {
                        ItemId = r.SelectedIngredient.Id,
                        Price = r.TotalPrice,
                        Quantity = r.QuantityRegistered
                    })
                    .ToList()
            };

            if (!_confectionService.SaveConfection(confectionForm))
            {
                return;
            }

            _navigationManager.CloseDialog("RegisterConfectionView", new()
            {
                { "reload", true }
            });
        }

        /// <summary>
        /// Cancel the confection and communicate to the caller view that the list of confections should not be reloaded
        /// </summary>
        private void CancelConfection()
        {
            _navigationManager.CloseDialog("RegisterConfectionView", new()
            {
                { "reload", false  }
            });
        }
    }
}
