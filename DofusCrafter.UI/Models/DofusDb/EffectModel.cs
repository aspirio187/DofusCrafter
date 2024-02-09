using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class EffectModel
    {
        [JsonPropertyName("__id")]
        public string DoubleUnderscoreId { get; set; } = string.Empty;
        public int From { get; set; }
        public int To { get; set; }
        public int Characteristic { get; set; }
        public int Category { get; set; }
        public int ElementId { get; set; }
    }
}
