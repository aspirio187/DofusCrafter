using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Xml.Linq;

namespace DofusCrafter.UI.Models.DofusDb
{
    public class ItemModel
    {
        [JsonPropertyName("__id")]
        public string DoubleUnderscoreId { get; set; } = string.Empty;
        public bool Cursed { get; set; }
        public bool Usable { get; set; }
        public bool Targetable { get; set; }
        public bool Exchangeable { get; set; }
        public bool TwoHanded { get; set; }
        public bool Etheral { get; set; }
        public int ItemSetId { get; set; }
        public string Criteria { get; set; } = string.Empty;
        public object CriteriaTarget { get; set; } = string.Empty;
        public bool HideEffects { get; set; }
        public bool Enhanceable { get; set; }
        public bool NonUsableOnAnother { get; set; }
        public int AppearanceId { get; set; }
        public bool SecretRecipe { get; set; }
        public List<object> RecipeIds { get; set; } = new List<object>();
        public List<object> DropMonsterIds { get; set; } = new List<object>();
        public List<object> DropTemporisMonsterIds { get; set; } = new List<object>();
        public bool ObjectIsDisplayOnWeb { get; set; }
        public bool BonusIsSecret { get; set; }
        public List<PossibleEffectModel> PossibleEffects { get; set; } = new List<PossibleEffectModel>();
        public List<object> EvolutiveEffectIds { get; set; } = new List<object>();
        public List<object> FavoriteSubAreas { get; set; } = new List<object>();
        public int FavoriteSubAreasBonus { get; set; }
        public int CraftXpRatio { get; set; }
        public bool NeedUseConfirm { get; set; }
        public bool IsDestructible { get; set; }
        public bool IsSaleable { get; set; }
        public List<List<double>> NuggetsBySubarea { get; set; } = new List<List<double>>();
        public List<object> ContainerIds { get; set; } = new List<object>();
        public List<object> ResourcesBySubarea { get; set; } = new List<object>();
        public bool IsLegendary { get; set; }
        public object? CraftConditional { get; set; }
        public int CriticalFailureProbability { get; set; }
        public string CraftVisible { get; set; } = string.Empty;
        public int CriticalHitBonus { get; set; }
        public int ImportantNoticeId { get; set; }
        public ImportantNoticeModel? ImportantNotice { get; set; }
        public object? ChangeVersion { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }
        public int IconId { get; set; }
        public object? Visibility { get; set; }
        public int Level { get; set; }
        public int UseAnimationId { get; set; }
        public int NameId { get; set; }
        public NameModel Name { get; set; }
        public int TypeId { get; set; }
        public int RecipeSlots { get; set; }
        public int MinRange { get; set; }
        public int CriticalHitProbability { get; set; }
        public int Range { get; set; }
        public bool CastInLine { get; set; }
        public int ApCost { get; set; }
        public bool CastInDiagonal { get; set; }
        public int DescriptionId { get; set; }
        public DescriptionModel? Description { get; set; }
        public string CraftFeasible { get; set; } = string.Empty;
        public bool CastTestLos { get; set; }
        public int RealWeight { get; set; }
        public object? RooltipExpirationDate { get; set; }
        public int MaxCastPerTurn { get; set; }
        public List<EffectModel> Effects { get; set; } = new List<EffectModel>();
        public SlugModel Slug { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Img { get; set; }
        public List<ImgSetModel> imgset { get; set; } = new List<ImgSetModel>();
        public object? ItemSet { get; set; }
        public ItemTypeModel? Type { get; set; }
        public object? Appearance { get; set; }
    }
}
