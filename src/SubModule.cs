using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace UnlimitedWorkshops
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            InformationManager.DisplayMessage(new InformationMessage("[UnlimitedWorkshops] Workshops systems changed successfully."));
            // InformationManager.DisplayMessage(new InformationMessage("UnlimitedWorkshops mod carregado com sucesso!"));
            var harmony = new Harmony("br.com.eduard.bannerlord.unlimitedworkshops");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
// <PackageReference Include="Bannerlord.ReferenceAssemblies.Core" Version="1.2.12.54620" />