using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class ItemTypeModel
    {
        public string _id { get; set; }
        public int Id { get; set; }
        public int NameId { get; set; }
        public NameModel? Name { get; set; }
        public int SuperTypeId { get; set; }
        public int CategoryId { get; set; }
        public bool IsInEncyclopedia { get; set; }
        public bool Plural { get; set; }
        public int Gender { get; set; }
        public string? RawZone { get; set; }
        public bool Mimickable { get; set; }
        public int CraftXpRatio { get; set; }
        public int EvolutiveTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int __v { get; set; }
        public List<string>? _include { get; set; }
        public SuperTypeModel? SuperType { get; set; }
    }
}
