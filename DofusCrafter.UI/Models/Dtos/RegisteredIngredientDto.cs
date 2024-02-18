using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.Dtos
{
    public class RegisteredIngredientDto
    {
        public IngredientDto SelectedIngredient { get; set; } = new();
        public int QuantityRegistered { get; set; }
        public int TotalPrice { get; set; }
    }
}
