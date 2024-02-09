using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class SuperTypeModel
    {
        [JsonPropertyName("__id")]
        public string DoubleUnderscoreId { get; set; }
        public List<int> Positions { get; set; } = new List<int>();
        public int Id { get; set; }
        public NameModel? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("__v")]
        public int DoubleUnderscoreV { get; set; }
    }
}
