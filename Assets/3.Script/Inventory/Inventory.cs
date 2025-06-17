using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<ArmorType, Armor> equippedArmors = new();

    public ArmorStat armorStat = new();

    public void EquipArmor(Armor armor)
    {
        ArmorType type = armor.Type;

        if (equippedArmors.ContainsKey(type))
        {
            RemoveEquipmentOption(equippedArmors[type]);
        }

        ApplyEquipmentOption(armor);

        equippedArmors[type] = armor;
    }

    public float GetTotalDefense()
    {
        float total = 0f;
        foreach (var armor in equippedArmors.Values)
        {
            total += armor.GetDefense();
        }

        return total;
    }

    public void DebugPrintTotalDefense()
    {
        Debug.Log($"[방어구 총합 방어력] {armorStat.totalDefense * armorStat.multiplierDefense}");
    }

    private void ApplyEquipmentOption(Armor parts)
    {
        armorStat.totalDefense += parts.currentStat.defense;

        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    armorStat.dashCooldownReduction += 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    armorStat.multiplierDefense += 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    armorStat.increaseHealth += 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    armorStat.multiplierHealth += 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    armorStat.reloadSpeedReduction += 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    armorStat.multiplierAttack += 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    armorStat.multiplierMovementSpeed += 0.1f;
                    break;
            }
        }
    }

    private void RemoveEquipmentOption(Armor parts)
    {
        armorStat.totalDefense -= parts.currentStat.defense;

        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    armorStat.dashCooldownReduction -= 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    armorStat.multiplierDefense -= 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    armorStat.increaseHealth -= 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    armorStat.multiplierHealth -= 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    armorStat.reloadSpeedReduction -= 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    armorStat.multiplierAttack -= 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    armorStat.multiplierMovementSpeed -= 0.1f;
                    break;
            }
        }
    }
}