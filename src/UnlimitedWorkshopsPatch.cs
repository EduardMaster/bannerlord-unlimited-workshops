using HarmonyLib;
using TaleWorlds.CampaignSystem;
using UnlimitedWorkshops;

namespace UnlimitedWorkshops.Patches
{
  [HarmonyPatch(typeof(Clan), "GetWorkshopLimit")]
  public static class WorkshopPatch
  {
    static bool Prefix(Clan __instance, ref int __result)
    {
      // Pega o valor do settings, padrão 5 se não estiver configurado
      int workshopsPerClanLevel = Settings.Instance?.WorkshopsPerClanLevel ?? 5;

      // Usa o nível do clã para calcular o limite
      __result = __instance.Tier * workshopsPerClanLevel;

      // Cancela o método original, usando nosso resultado
      return false;
    }
  }
}
