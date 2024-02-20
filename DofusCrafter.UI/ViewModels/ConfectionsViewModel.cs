﻿using DofusCrafter.UI.Commands;
using DofusCrafter.UI.Managers;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            SearchQueryChangedCommand = new GenericCommand(OnSearchQueryChanged);
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
        private void OnSearchQueryChanged()
        {
            if (SearchQuery is null || SearchQuery.Trim().Length <= 0)
            {
                Task.Factory.StartNew(LoadConfections);
                return;
            }

            string searchQuery = SearchQuery.Trim();

            string[] queryWords = searchQuery.Split(' ');

            List<ConfectionModel> tempConfection = new List<ConfectionModel>();

            foreach (string word in queryWords)
            {
                IEnumerable<ConfectionModel> result = Confections.Where(c => c.Slug.Contains(word) || c.Name.Contains(word));

                tempConfection.AddRange(result);
            }

            Confections = new ObservableCollection<ConfectionModel>(tempConfection);
        }

        /// <summary>
        /// Open the dialog to register a confection
        /// </summary>
        private void RegisterConfection()
        {
            _navigationManager.OpenDialog("RegisterConfectionView", this);
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
