using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;

namespace UnlimitedWorkshops
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "UnlimitedWorkshopsSettings_v1";
        public override string DisplayName => "Unlimited Workshops";
        public override string FolderName => "UnlimitedWorkshops";

        [SettingPropertyInteger("Workshops por Nível de Clã", 1, 20, HintText = "Define quantos workshops são permitidos por nível do clã.", RequireRestart = false, Order = 0)]
        [SettingPropertyGroup("Configurações")]
        public int WorkshopsPerClanLevel { get; set; } = 5;
    }
}