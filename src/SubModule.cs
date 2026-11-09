using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;

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

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject){
            base.OnGameStart(game, gameStarterObject);

            if (gameStarterObject is CampaignGameStarter campaignStarter)
            {
              
            }
        }
    }
}
// <PackageReference Include="Bannerlord.ReferenceAssemblies.Core" Version="1.2.12.54620" />