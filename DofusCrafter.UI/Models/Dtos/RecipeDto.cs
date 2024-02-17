using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Img { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = [];
    }
}
