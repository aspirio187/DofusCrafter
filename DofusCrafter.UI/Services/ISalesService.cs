using DofusCrafter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Services
{
    public interface ISalesService
    {
        bool SaveSale(SoldItemModel soldItemModel);
    }
}
