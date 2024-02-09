using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class PossibleEffectModel
    {
        [JsonPropertyName("__id")]
        public string DoubleUnderscoreId { get; set; } = string.Empty;
        public string TargetMask { get; set; } = string.Empty;
        public int DiceNum { get; set; }
        public bool VisibleInBuffUi { get; set; }
        public int BaseEffectId { get; set; }
        public bool VisibleInFightLog { get; set; }
        public int TargetId { get; set; }
        public int EffectElement { get; set; }
        public int EffectUid { get; set; }
        public int Dispellable { get; set; }
        public string Triggers { get; set; } = string.Empty;
        public int SpellId { get; set; }
        public int Duration { get; set; }
        public int Random { get; set; }
        public int EffectId { get; set; }
        public int Delay { get; set; }
        public int DiceSide { get; set; }
        public bool VisibleOnTerrain { get; set; }
        public bool VisibleInTooltip { get; set; }
        public string RawZone { get; set; } = string.Empty;
        public bool ForClientOnly { get; set; }
        public int Value { get; set; }
        public int Order { get; set; }
        public int Group { get; set; }
    }
}
