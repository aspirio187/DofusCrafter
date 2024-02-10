using DofusCrafter.UI.Models.DofusDb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models
{
    public class SoldItemModel : ModelBase
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => ValidateProperty(ref _name, value);
        }

        private string _quantity = string.Empty;

        public string Quantity
        {
            get => _quantity;
            set => ValidateProperty(ref _quantity, value);
        }

        private string _price = string.Empty;

        public string Price
        {
            get => _price;
            set => ValidateProperty(ref _price, value);
        }


        private DateTime _soldDate = DateTime.Now;

        public DateTime SoldDate
        {
            get => _soldDate;
            set => ValidateProperty(ref _soldDate, value);
        }

        private string _image = "/Assets/Images/unknown-item.jpeg";

        public string Image
        {
            get => _image;
            set => ValidateProperty(ref _image, value);
        }
    }
}
