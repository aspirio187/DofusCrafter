using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.Data.Entities
{
    public class ConfectionIngredientEntity
    {
        public int Id { get; set; }
        public int ConfectionId { get; set; }
        public ConfectionEntity Confection { get; set; } = new ConfectionEntity();
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
