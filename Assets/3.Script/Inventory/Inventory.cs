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
    }
    
    private void ApplyEquipmentOption(Armor parts)
    {
        EquipmentStat.totalDefense += parts.currentStat.defense;

        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    EquipmentStat.increaseDashCooldown += 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    EquipmentStat.increaseDefense += 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    EquipmentStat.plusHp += 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    EquipmentStat.increaseHealth += 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    EquipmentStat.increaseReloadSpeed += 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    EquipmentStat.increaseAttack += 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    EquipmentStat.increaseMovementSpeed += 0.1f;
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
                    EquipmentStat.increaseDashCooldown -= 0.1f;
                    break;
                case SpecialEffect.MultiplierDefense:
                    EquipmentStat.increaseDefense -= 0.1f;
                    break;
                case SpecialEffect.IncreaseHealth:
                    EquipmentStat.plusHp -= 20;
                    break;
                case SpecialEffect.MultiplierHealth:
                    EquipmentStat.increaseHealth -= 0.1f;
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    EquipmentStat.increaseReloadSpeed -= 0.1f;
                    break;
                case SpecialEffect.MultiplierAttackDamage:
                    EquipmentStat.increaseAttack -= 0.05f;
                    break;
                case SpecialEffect.MultiplierMovementSpeed:
                    EquipmentStat.increaseMovementSpeed -= 0.1f;
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
                EquipmentStat.increaseDashCooldown += 0.1f;
                break;
            case SpecialEffect.MultiplierDefense:
                EquipmentStat.increaseDefense += 0.1f;
                break;
            case SpecialEffect.IncreaseHealth:
                EquipmentStat.plusHp += 20;
                break;
            case SpecialEffect.MultiplierHealth:
                EquipmentStat.increaseHealth += 0.1f;
                break;
            case SpecialEffect.ReloadSpeedReduction:
                EquipmentStat.increaseReloadSpeed += 0.1f;
                break;
            case SpecialEffect.MultiplierAttackDamage:
                EquipmentStat.increaseAttack += 0.05f;
                break;
            case SpecialEffect.MultiplierMovementSpeed:
                EquipmentStat.increaseMovementSpeed += 0.1f;
                break;
        }
    }

    // **단일 옵션만 누적 제거**
    public void RemoveEquipmentOption(SpecialEffect option)
    {
        switch (option)
        {
            case SpecialEffect.DashCooldownReduction:
                EquipmentStat.increaseDashCooldown -= 0.1f;
                break;
            case SpecialEffect.MultiplierDefense:
                EquipmentStat.increaseDefense -= 0.1f;
                break;
            case SpecialEffect.IncreaseHealth:
                EquipmentStat.plusHp -= 20;
                break;
            case SpecialEffect.MultiplierHealth:
                EquipmentStat.increaseHealth -= 0.1f;
                break;
            case SpecialEffect.ReloadSpeedReduction:
                EquipmentStat.increaseReloadSpeed -= 0.1f;
                break;
            case SpecialEffect.MultiplierAttackDamage:
                EquipmentStat.increaseAttack -= 0.05f;
                break;
            case SpecialEffect.MultiplierMovementSpeed:
                EquipmentStat.increaseMovementSpeed -= 0.1f;
                break;
        }
    }
    
}