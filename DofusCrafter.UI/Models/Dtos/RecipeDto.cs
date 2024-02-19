using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.Dtos
{
    public class RecipeDto
    {
        /// <summary>
        /// The id of the item from DofusDB (itemId)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The imaage of the item
        /// </summary>
        public string Img { get; set; } = string.Empty;

        /// <summary>
        /// The title/name of the item
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// The description of the item
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The minimum required level for this item
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The list of ingredients to create this item
        /// </summary>
        public List<IngredientDto> Ingredients { get; set; } = [];
    }
}
