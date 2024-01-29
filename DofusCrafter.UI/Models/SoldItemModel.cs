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

        private DateTime _soldDate;

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
