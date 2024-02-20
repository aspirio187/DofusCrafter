using DofusCrafter.Data.Entities;
using DofusCrafter.UI.Models;
using DofusCrafter.UI.Models.Dtos;
using DofusCrafter.UI.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Services
{
    public interface IConfectionService
    {
        /// <summary>
        /// Retrieves all the confections from the local database
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ConfectionModel>> GetConfectionsAsync();

        /// <summary>
        /// Retrieves all the confections from the local database and searches if the list contains any
        /// element of <paramref name="searchQuery"/>
        /// </summary>
        /// <param name="searchQuery">The keys to search</param>
        /// <returns></returns>
        Task<IEnumerable<ConfectionModel>> GetConfectionsAsync(string[] searchQuery);

        /// <summary>
        /// Save the confection of an item in the local database
        /// </summary>
        /// <param name="itemId">The item of the crafted item</param>
        /// <param name="quantity">The quantity crafted</param>
        /// <param name="ingredients">The list of ingredients to craft the item(s)</param>
        /// <returns>
        /// true If the craft was successfully saved. false Otherwise
        /// </returns>
        bool SaveConfection(ConfectionForm confection);
    }
}
