using DofusCrafter.Data.Entities;
using DofusCrafter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Mappers
{
    public static class Mapper
    {
        public static SaleEntity MapToSaleEntity(this SoldItemModel soldItemModel)
        {
            return new SaleEntity
            {
                ItemName = soldItemModel.Name,
                Quantity = int.Parse(soldItemModel.Quantity),
                UnitaryPrice = decimal.Parse(soldItemModel.Price),
                SaleDate = soldItemModel.SoldDate
            };
        }
    }
}
