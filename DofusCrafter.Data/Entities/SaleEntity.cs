using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.Data.Entities
{
    public class SaleEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitaryPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
