using Bannerlord.ButterLib.ObjectSystem.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using UnlimitedWorkshops;

namespace UnlimitedWorkshops.Patches
{
    [HarmonyPatch(typeof(DefaultWorkshopModel), "GetMaxWorkshopCountForClanTier")]
    public static class WorkshopLimitPatch
    {
        static bool Prefix(ref int __result, int tier)
        {
            int workshopsPerClanLevel = Settings.Instance?.WorkshopsPerClanLevel ?? 5;
            __result = tier * workshopsPerClanLevel;
            if (__result <= 1)
            {
                __result = tier + 1;
            }
            return false; // impede o método original
        }
    }

    [HarmonyPatch(typeof(DefaultWorkshopModel), "WarehouseCapacity", MethodType.Getter)]
    public static class WorkshopWerehouseCapacityPatch
    {
        static bool Prefix(ref int __result)
        {
            int workshopsWerehouseCapacity = Settings.Instance?.WorkshopWerehouseCapacity ?? 6000;
            __result = workshopsWerehouseCapacity;
            
            return false;
        }
    }
   
}
