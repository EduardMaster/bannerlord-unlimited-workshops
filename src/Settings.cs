
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
//using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.Base.Global;
namespace UnlimitedWorkshops
{
    public class Settings : AttributeGlobalSettings<Settings> // AttributePerSaveSettings
    {
        public override string Id => "Unlimited_Workshops_v1";
        public override string DisplayName => "Unlimited Workshops";
        public override string FolderName => "UnlimitedWorkshops";

        //public override string FormatType => "json"; // Works only for global settings

        [SettingPropertyInteger("Workshops per Clan Level", 2, 30, HintText = "Set how many workshops slots available per Clan level.", RequireRestart = false, Order = 0)]
        [SettingPropertyGroup("Main")]
        public int WorkshopsPerClanLevel { get; set; } = 5;

        [SettingPropertyInteger("Workshops per City", 4, 11, HintText = "Set how many workshops are available per city/settlement.", RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("Main")]
        public int WorkshopsPerCity { get; set; } = 4;

        [SettingPropertyInteger("Daily Workers Payment", 20, 1000, HintText = "Set how much is paid daily to workshop workers.", RequireRestart = false, Order = 2)]
        [SettingPropertyGroup("Main")]
        public int DailyWorkersPayment { get; set; } = 100;

        [SettingPropertyInteger("Workshop Werehouse Capacity", 6000, 100000, HintText = "Set how many items can stock on wherehouse.", RequireRestart = false, Order = 0)]
        [SettingPropertyGroup("Main")]
        public int WorkshopWerehouseCapacity { get; set; } = 6000;
    }
}