using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Services;
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
    /// <summary>
    /// The view model bound to ConfectionView
    /// </summary>
    public class ConfectionsViewModel : ViewModelBase
    {
        /// <summary>
        /// The navigation manager service
        /// </summary>
        private readonly NavigationManager _navigationManager;

        /// <summary>
        /// The confectio nservice
        /// </summary>
        private readonly IConfectionService _confectionService;

        /// <summary>
        /// The list of confections registered in the database
        /// </summary>
        private ObservableCollection<ConfectionModel> _confections = [];

        /// <summary>
        /// Gets or sets the list of confections
        /// </summary>
        public ObservableCollection<ConfectionModel> Confections
        {
            get { return _confections; }
            set
            {
                _confections = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The search query that the user enters in the search text box
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
        /// Command executed when the user click on register a new confection
        /// </summary>
        public ICommand RegisterConfectionCommand { get; private set; }

        /// <summary>
        /// Command executed when the user changes the text in the search query textbox
        /// </summary>
        public ICommand SearchQueryChangedCommand { get; private set; }

        /// <summary>
        /// Command executed when the user decide to register a confection as sold
        /// </summary>
        public ICommand RegisterItemSoldCommand { get; private set; }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="confectionService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfectionsViewModel(NavigationManager navigationManager, IConfectionService confectionService)
        {
            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            if (confectionService is null)
            {
                throw new ArgumentNullException(nameof(confectionService));
            }

            _navigationManager = navigationManager;
            _confectionService = confectionService;

            // Commands registration
            RegisterConfectionCommand = new GenericCommand(RegisterConfection);
            SearchQueryChangedCommand = new AsyncRelayCommand<string>(OnSearchQueryChanged);
            RegisterItemSoldCommand = new AsyncRelayCommand<int>(RegisterItemSoldAsync);
        }

        /// <summary>
        /// Initialize the view model
        /// </summary>
        public override void OnInit()
        {
            Task.Factory.StartNew(LoadConfections);
        }

        /// <summary>
        /// Asynchronously load all the confections from the database
        /// </summary>
        /// <returns></returns>
        private async Task LoadConfections()
        {
            Confections = new ObservableCollection<ConfectionModel>
                ((await _confectionService
                    .GetConfectionsAsync())
                    .OrderByDescending(c => c.CreatedAt));
        }

        /// <summary>
        /// If the search query contains a non-empty value, it will search the list of confections and retrieve
        /// all the values where the slug or the name contains any word of the search query. Otherwise, reload the 
        /// confections list
        /// </summary>
        private async Task OnSearchQueryChanged(string? query)
        {
            if (query is null || query.Trim().Length <= 0)
            {
                await LoadConfections();
                return;
            }

            string searchQuery = query.Trim();

            string[] queryWords = searchQuery.Split(' ');

            SearchQuery = query;

            Confections = new ObservableCollection<ConfectionModel>
                ((await _confectionService
                    .GetConfectionsAsync(queryWords))
                    .OrderByDescending(c => c.CreatedAt));
        }

        /// <summary>
        /// Open the dialog to register a confection
        /// </summary>
        private void RegisterConfection()
        {
            _navigationManager.OpenDialog("RegisterConfectionView", this);
        }

        /// <summary>
        /// Event handler for <see cref="RegisterItemSoldCommand"/>.
        /// Register a confection as sold
        /// </summary>
        /// <param name="confectionId"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task RegisterItemSoldAsync(int confectionId)
        {
            if (confectionId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(confectionId));
            }

            ConfectionModel? selectedConfection = await _confectionService.GetConfectionAsync(confectionId);

            if (selectedConfection is null)
            {
                // TODO: Log information or send info to the user
                return;
            }

            _navigationManager.OpenDialog("RegisterSoldConfectionView", this, new()
            {
                { "selectedConfection",  selectedConfection}
            });
        }

        /// <summary>
        /// Event handler executed when a dialog opened from this view model is closed and contains parameters
        /// </summary>
        /// <param name="parameters">The dictionnary with parameters transfered from the dialog to the view model</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public override void OnNavigatedFrom(Dictionary<string, object>? parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters["reload"] is not bool reload)
            {
                throw new InvalidCastException("Parameter 'reload' in the dictionnary is not a boolean");
            }

            if (reload)
            {
                Task.Factory.StartNew(LoadConfections);
            }
        }
    }
}
