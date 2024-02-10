using DofusCrafter.Data;
using DofusCrafter.UI.Mappers;
using DofusCrafter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Services
{
    public class SalesService : ISalesService
    {
        private readonly DofusCrafterDbContext _dbContext;

        public SalesService(DofusCrafterDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public bool SaveSale(SoldItemModel soldItemModel)
        {
            if (soldItemModel is null)
            {
                throw new ArgumentNullException(nameof(soldItemModel));
            }

            if (!soldItemModel.IsValid)
            {
                return false;
            }

            var saleEntity = soldItemModel.MapToSaleEntity();

            if (saleEntity is null)
            {
                return false;
            }

            _dbContext.Sales.Add(saleEntity);

            return _dbContext.SaveChanges() > 0;
        }
    }
}
