using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.Data.Entities
{
    public class ConfectionEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public List<ConfectionIngredientEntity> ConfectionIngredients { get; set; } = [];
    }
}
