using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class RecipeModel
    {
        public int[] IngredientIds { get; set; } = [];
        public int[] Quantities { get; set; } = [];
        public int ResultId { get; set; }
        public NameModel ResultName { get; set; } = new();
        public ItemModel Result { get; set; } = new();
        public ItemModel[] Ingredients { get; set; } = [];
    }
}
