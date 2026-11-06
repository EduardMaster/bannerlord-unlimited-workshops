
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace UnlimitedWorkshops.Models
{

    public class DropAllDeadsEquipmentModel : DefaultBattleRewardModel
    {
        void teste()
        {
            MapEvent mapEven = null;
        }

    }

    public class EquipmentPartyWageModel : DefaultPartyWageModel
    {
        public override int GetCharacterWage(CharacterObject character)
        {
                int num = character.Tier switch
                {
                    0 => 1,
                    1 => 1,
                    2 => 2,
                    3 => 4,
                    4 => 8,
                    5 => 16,
                    6 => 32,
                    _ => 64,
                };
                if (character.Occupation == Occupation.Mercenary)
                {
                    num = (int)((float)num * 1.5f);
                }

                return num;
            }

        public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            ExplainedNumber normalPricing = base.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
            // 2. Cálculo do Custo Real dos Equipamentos
                if (!withoutItemCost && troop.Equipment != null)
                {
                    float totalEquipmentValue = 0f;
                    // Percorre todos os slots de equipamento (Armas, Armaduras, Cavalo, Barda)
                    for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
                    {
                        EquipmentElement equipmentElement = troop.Equipment[i];
                        if (equipmentElement.Item != null)
                        {
                            // Soma o valor de mercado (Value) do item equipado
                            totalEquipmentValue += equipmentElement.Item.Value;
                        }
                    }
                    // Adiciona o valor total de itens ao resultado
                    if (totalEquipmentValue > 0f)
                    {
                        normalPricing.Add(totalEquipmentValue, new TextObject("{=!}Equipment Cost"));
                    }
                }
            return normalPricing;
        }
    }
}