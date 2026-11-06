using Bannerlord.ButterLib.ObjectSystem.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
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
   
    [HarmonyPatch(typeof(DefaultWorkshopModel), "DefaultWorkshopCountInSettlement", MethodType.Getter)]
    public static class DefaultWorkshopCountInSettlementPatch
    {
        static bool Prefix(ref int __result)
        {
            int workshopsPerCity = Settings.Instance?.WorkshopsPerCity ?? 4;
            __result = workshopsPerCity;
            return false;
        }
    }

    [HarmonyPatch(typeof(DefaultWorkshopModel), "DailyExpense", MethodType.Getter)]
    public static class DailyExpensePatch
    {
        static bool Prefix(ref int __result)
        {
            int dailyWorkersPayment = Settings.Instance?.DailyWorkersPayment ?? 100;
            __result = dailyWorkersPayment;
            return false;
        }
    }


    [HarmonyPatch(typeof(MapEvent), "LootCasualtyCharacter")]
    public class LootCasualtyCharacterPatch
    {
        // Usamos Prefix para interceptar a execução antes do método original rodar
        // Se retornarmos 'false', o método original é ignorado e nós controlamos 100% da lógica.
        public static bool Prefix(
            CharacterObject casualtyCharacter, 
            MapEventParty winnerParty, 
            MapEventParty defeatedParty, 
            float aiTradePenalty, 
            int maxLootedItemsPerBodyForMainParty, 
            ItemRoster mainPartyLootFromCasualties)
        {
            Hero leaderHero = winnerParty.Party.LeaderHero;
            if (leaderHero == null)
            {
                return false; // Interrompe se não houver líder
            }

            // Se o líder NÃO for o jogador (IA), mantemos a lógica original de ganho de ouro por comércio/valor esperado
            if (leaderHero != Hero.MainHero)
            {
                float expectedLootedItemValueFromCasualty = Campaign.Current.Models.BattleRewardModel.GetExpectedLootedItemValueFromCasualty(leaderHero, casualtyCharacter);
                if (!expectedLootedItemValueFromCasualty.ApproximatelyEqualsTo(0f))
                {
                    int num = (int)((float)MathF.Round(expectedLootedItemValueFromCasualty) * aiTradePenalty);
                    if (num > 0)
                    {
                        winnerParty.Party.MobileParty.PartyTradeGold += num;
                        SkillLevelingManager.OnAIPartyLootCasualties(num, leaderHero, defeatedParty.Party);
                    }
                }
            }
            else
            {
                // ==========================================
                // AQUI É O LOOT DO JOGADOR (100% DOS ITENS)
                // ==========================================
                if (maxLootedItemsPerBodyForMainParty <= 0)
                {
                    return false;
                }

                // Pega os equipamentos de batalha do soldado morto
                Equipment equipment = casualtyCharacter.BattleEquipments?.GetRandomElementInefficiently();
                if (equipment != null)
                {
                    // Varre todos os 12 slots de equipamento da tropa (armas, armaduras, montaria, etc.)
                    for (int slot = 0; slot < 12; slot++)
                    {
                        EquipmentElement eqElement = equipment[slot];
                        
                        // Se o item existe e pode ser comercializado (não é de quest/invisível)
                        if (eqElement.Item != null && !eqElement.Item.NotMerchandise)
                        {
                            // Adiciona diretamente ao inventário de loot da batalha do jogador
                            mainPartyLootFromCasualties.AddToCounts(eqElement, 1);
                        }
                    }
                }
            }

            // Retorna 'false' para impedir que o jogo execute o método original por cima do nosso
            return false;
        }
    }
   
}
