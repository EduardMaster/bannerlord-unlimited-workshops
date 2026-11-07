
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
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

    public class ManipulateItemsPricesModel : DefaultTradeItemPriceFactorModel
    {
        public override float GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue)
        {
            float currentUnits = inStoreValue;
            if (isSelling)
            {
                currentUnits += (float)transferValue;
            }
            if (currentUnits <= 0f)
            {
                return 10f;
            }
            float factor;
            float maxScarcityThreshold = 1000f;
            float oversupplyThreshold = 1500f;
            if (currentUnits <= maxScarcityThreshold)
            {
                factor = 1f + 9f * (1f - (currentUnits / maxScarcityThreshold));
            }
            else if (currentUnits <= oversupplyThreshold)
            {
                factor = 1f;
            }
            else
            {
                float excessUnits = currentUnits - oversupplyThreshold;
                factor = 1f - ((excessUnits / 500f) * 0.1f);
            }
            return MathF.Clamp(factor, 0.1f, 10f);
        }
        public override float GetTradePenalty(ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand)
        {
            return 0f;
        }
         public override int GetPrice(EquipmentElement item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand)
        {
            float basePriceFactor = GetBasePriceFactor(item.Item.ItemCategory, inStoreValue, supply, demand, isSelling, item.ItemValue);
            float tradePenalty = GetTradePenalty(item.Item, clientParty, merchant, isSelling, inStoreValue, supply, demand);
            float priceFactor;
            if (!isSelling)
            {
                priceFactor = basePriceFactor * (1f + tradePenalty);
            } else
            {
                priceFactor = basePriceFactor * 1f / (1f + tradePenalty);
            }

           
            float finalPrice = item.ItemValue * priceFactor;
            int finalPriceFix = isSelling ? MathF.Floor(finalPrice) : MathF.Ceiling(finalPrice);
            if (!isSelling && merchant?.MobileParty != null && merchant.MobileParty.IsCaravan && clientParty.HasPerk(DefaultPerks.Trade.SilverTongue, checkSecondaryRole: true))
            {
                finalPriceFix = MathF.Ceiling((float)finalPriceFix * (1f - DefaultPerks.Trade.SilverTongue.SecondaryBonus));
            }
            
            return MathF.Max(finalPriceFix, 1);
        }
    }
    

    public class EquipmentTroopUpgradeModel : DefaultPartyTroopUpgradeModel
    {
        
        public override ExplainedNumber GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
        {
            PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
            int roundedResultNumber = partyWageModel.GetTroopRecruitmentCost(upgradeTarget, null, withoutItemCost: true).RoundedResultNumber;
            int roundedResultNumber2 = partyWageModel.GetTroopRecruitmentCost(characterObject, null, withoutItemCost: true).RoundedResultNumber;
            bool flag = characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Gangster;
            float upgradeCost = (float)(roundedResultNumber - roundedResultNumber2); // ((!flag) ? 2f : 3f);
            ExplainedNumber stat = new ExplainedNumber(upgradeCost);
            if (party.MobileParty.HasPerk(DefaultPerks.Steward.SoundReserves))
            {
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.SoundReserves, party.MobileParty, isPrimaryBonus: true, ref stat);
            }
            if (characterObject.IsRanged && party.MobileParty.HasPerk(DefaultPerks.Bow.RenownedArcher, checkSecondaryRole: true))
            {
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, party.MobileParty, isPrimaryBonus: false, ref stat);
            }
            if (characterObject.IsMounted && PartyBaseHelper.HasFeat(party, DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
            {
                stat.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture"));
            }
            if (flag && party.MobileParty.HasPerk(DefaultPerks.Steward.Contractors))
            {
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Contractors, party.MobileParty, isPrimaryBonus: true, ref stat);
            }
            return stat;
        }
    } 

    public class EquipmentPartyWageModel : DefaultPartyWageModel
    {
        public override int GetCharacterWage(CharacterObject character)
        {
            int num = character.Tier switch
            {
                0 => 1,
                1 => 2,
                2 => 4,
                3 => 8,
                4 => 16,
                5 => 32,
                6 => 64,
                _ => 128,
            };
            if (character.Occupation == Occupation.Mercenary)
            {
                num = (int)((float)num * 1.5f);
            }

            return num;
        }

        public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
        {
            ExplainedNumber vanillaCost = base.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
            ExplainedNumber normalPricing = new ExplainedNumber(20f * troop.Level);
            if (troop.Equipment != null)
            {
                float totalEquipmentValue = 0f;
                for (int i = 0; i < (int)Equipment.EquipmentSlotLength; i++)
                {
                    EquipmentElement equipmentElement = troop.Equipment[i];
                    if (equipmentElement.Item != null)
                    {
                        totalEquipmentValue += equipmentElement.Item.Value;
                    }
                }
                if (totalEquipmentValue > 0f)
                {
                    normalPricing.Add(totalEquipmentValue, new TextObject("{=!}Equipment Cost"));
                }
            }
            return normalPricing;
        }
    }
}