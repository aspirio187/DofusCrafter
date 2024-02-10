﻿using DofusCrafter.UI.Models.DofusDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Services
{
    public class DofusDBService
    {
        private HttpClient _httpClient;

        public DofusDBService(IHttpClientFactory httpClientFactory)
        {
            if (httpClientFactory is null)
            {
                throw new ArgumentNullException(nameof(httpClientFactory));
            }

            _httpClient = httpClientFactory.CreateClient("dofusdb");
        }

        public async Task GetItemsAsync()
        {
            var items = await _httpClient.GetAsync("/items");
        }

        public async Task<IEnumerable<ItemTypeModel>> GetItemTypesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/item-types");

            var result = await response.Content.ReadFromJsonAsync<ItemTypeResultModel>();

            return result.Data;
        }

        public async Task<ItemModel[]> SearchItemsAsync(string searchQuery)
        {
            if (searchQuery is null)
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"/items?slug.fr[$search]={searchQuery}&$limit=50");

            var result = await response.Content.ReadFromJsonAsync<ResultModel<ItemModel>>();

            if (result is null)
            {
                return [];
            }

            return result.Data;
        }
    }
}