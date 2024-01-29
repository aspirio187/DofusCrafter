using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class SuperTypeModel
    {
        public string _id { get; set; }
        public List<int> Positions { get; set; }
        public int Id { get; set; }
        public NameModel Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int __v { get; set; }
    }
}
