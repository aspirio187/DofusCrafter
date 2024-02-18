using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Interfaces;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DofusCrafter.UI.ViewModels
{
    /// <summary>
    /// The view model associated to <seealso cref="Views.RegisterIngredientView"/>
    /// This is a dialog with parameters transfer
    /// </summary>
    public class RegisterIngredientViewModel : ViewModelBase, IDialogWithParameters
    {
        /// <summary>
        /// The navigation manager
        /// </summary>
        private readonly NavigationManager _navigationManager;

        /// <summary>
        /// The selected ingredient passed through from <see cref="RegisterConfectionViewModel"/>
        /// </summary>
        private IngredientDto _selectedIngredient = new();

        /// <summary>
        /// Gets or sets the selected ingredient
        /// </summary>
        public IngredientDto SelectedIngredient
        {
            get { return _selectedIngredient; }
            set
            {
                _selectedIngredient = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The quantity of the selected ingredient bought
        /// </summary>
        private string _quantity = string.Empty;

        /// <summary>
        /// Gets or sets the quantity
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
        /// The total price of the selected ingredient
        /// </summary>
        private string _totalPrice = string.Empty;

        /// <summary>
        /// Gets or sets the total price
        /// </summary>
        public string TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The command executed when the user click on the save button
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// The command executed when the user cancel the ingredient registration
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Create a new instance of the <see cref="RegisterIngredientViewModel"/>
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterIngredientViewModel(NavigationManager navigationManager)
        {
            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            _navigationManager = navigationManager;

            // Registering commands
            SaveCommand = new GenericCommand(Save);
            CancelCommand = new GenericCommand(Cancel);
        }

        /// <summary>
        /// Save the current registration of the ingredient by sending back to the caller view the 
        /// ingredient id, the quantity bought and the total price of the purchase
        /// </summary>
        private void Save()
        {
            if (!int.TryParse(Quantity, out int quantity))
            {
                throw new InvalidCastException(nameof(quantity));
            }

            if (!int.TryParse(TotalPrice, out int totalPrice))
            {
                throw new InvalidCastException(nameof(TotalPrice));
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "ingredientId", SelectedIngredient.Id },
                { "quantity", quantity },
                { "totalPrice", totalPrice },
            };

            _navigationManager.CloseDialog("RegisterIngredientView", parameters);
        }

        /// <summary>
        /// Cancel the registration of the ingredient and goes back to the caller view
        /// </summary>
        private void Cancel()
        {
            _navigationManager.CloseDialog("RegisterIngredientView");
        }

        /// <summary>
        /// Retrieves the parameter <paramref name="parameters"/> which must contain a key "ingredient"
        /// with its value being of type <see cref="IngredientDto"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters["ingredient"] is not IngredientDto ingredientParameter)
            {
                throw new InvalidCastException($"Parameter 'ingredient' is not of type {typeof(IngredientDto)}");
            }

            SelectedIngredient = ingredientParameter;
        }
    }
}
