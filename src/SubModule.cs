using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;

namespace UnlimitedWorkshops
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var harmony = new Harmony("br.com.eduard.bannerlord.unlimitedworkshops");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}