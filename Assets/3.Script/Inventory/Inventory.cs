using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<ArmorType, Armor> equippedArmors = new();

    public EquipmentStat EquipmentStat = new();

    public void EquipArmor(Armor armor)
    {
        ArmorType type = armor.Type;

        if (equippedArmors.ContainsKey(type))
        {
            RemoveEquipmentOption(equippedArmors[type]);
        }

        ApplyEquipmentOption(armor);

        equippedArmors[type] = armor;

        EquipmentStat.PrintOption();
    }
    
    private void ApplyEquipmentOption(Armor parts)
    {
        EquipmentStat.totalDefense += parts.currentStat.defense;

        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    EquipmentStat.dashCooldownReduction += 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    EquipmentStat.multiplierDefense += 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    EquipmentStat.increaseHealth += 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    EquipmentStat.multiplierHealth += 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    EquipmentStat.reloadSpeedReduction += 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    EquipmentStat.multiplierAttack += 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    EquipmentStat.multiplierMovementSpeed += 0.1f;
                    break;
            }
        }
    }

    private void RemoveEquipmentOption(Armor parts)
    {
        EquipmentStat.totalDefense -= parts.currentStat.defense;

        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    EquipmentStat.dashCooldownReduction -= 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    EquipmentStat.multiplierDefense -= 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    EquipmentStat.increaseHealth -= 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    EquipmentStat.multiplierHealth -= 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    EquipmentStat.reloadSpeedReduction -= 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    EquipmentStat.multiplierAttack -= 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    EquipmentStat.multiplierMovementSpeed -= 0.1f;
                    break;
            }
        }
    }
     // **단일 옵션만 누적 적용**
    public void ApplyEquipmentOption(SpecialEffect option)
    {
        switch (option)
        {
            case SpecialEffect.DashCooldownReduction:
                EquipmentStat.dashCooldownReduction += 0.1f;
                break;
            case SpecialEffect.MultiplierDefense:
                EquipmentStat.multiplierDefense += 0.1f;
                break;
            case SpecialEffect.IncreaseHealth:
                EquipmentStat.increaseHealth += 20;
                break;
            case SpecialEffect.MultiplierHealth:
                EquipmentStat.multiplierHealth += 0.1f;
                break;
            case SpecialEffect.ReloadSpeedReduction:
                EquipmentStat.reloadSpeedReduction += 0.1f;
                break;
            case SpecialEffect.MultiplierAttackDamage:
                EquipmentStat.multiplierAttack += 0.05f;
                break;
            case SpecialEffect.MultiplierMovementSpeed:
                EquipmentStat.multiplierMovementSpeed += 0.1f;
                break;
        }
    }

    // **단일 옵션만 누적 제거**
    public void RemoveEquipmentOption(SpecialEffect option)
    {
        switch (option)
        {
            case SpecialEffect.DashCooldownReduction:
                EquipmentStat.dashCooldownReduction -= 0.1f;
                break;
            case SpecialEffect.MultiplierDefense:
                EquipmentStat.multiplierDefense -= 0.1f;
                break;
            case SpecialEffect.IncreaseHealth:
                EquipmentStat.increaseHealth -= 20;
                break;
            case SpecialEffect.MultiplierHealth:
                EquipmentStat.multiplierHealth -= 0.1f;
                break;
            case SpecialEffect.ReloadSpeedReduction:
                EquipmentStat.reloadSpeedReduction -= 0.1f;
                break;
            case SpecialEffect.MultiplierAttackDamage:
                EquipmentStat.multiplierAttack -= 0.05f;
                break;
            case SpecialEffect.MultiplierMovementSpeed:
                EquipmentStat.multiplierMovementSpeed -= 0.1f;
                break;
        }
    }
    
}