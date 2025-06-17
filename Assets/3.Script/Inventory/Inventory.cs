using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<ArmorType, Armor> equippedArmors = new();

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
        Debug.Log($"[방어구 총합 방어력] {GetTotalDefense()}");
    }

    private void ApplyEquipmentOption(Armor parts)
    {
        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    Debug.Log("대쉬 쿨감 옵션 추가");
                    break;
                case SpecialEffect.DefenseBoostPercent:
                    Debug.Log("방증 옵션 추가");
                    break;
                case SpecialEffect.HealthBoostPercent:
                    Debug.Log("체증 옵션 추가");
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    Debug.Log("장전 쿨감 옵션 추가");
                    break;
                case SpecialEffect.AttackBoostPercent:
                    Debug.Log("공증 옵션 추가");
                    break;
            }
        }
    }

    private void RemoveEquipmentOption(Armor parts)
    {
        foreach (var option in parts.options)
        {
            switch (option)
            {
                case SpecialEffect.DashCooldownReduction:
                    Debug.Log("대쉬 쿨감 옵션 제거");
                    break;
                case SpecialEffect.DefenseBoostPercent:
                    Debug.Log("방증 옵션 제거");
                    break;
                case SpecialEffect.HealthBoostPercent:
                    Debug.Log("체증 옵션 제거");
                    break;
                case SpecialEffect.ReloadSpeedReduction:
                    Debug.Log("장전 쿨감 옵션 제거");
                    break;
                case SpecialEffect.AttackBoostPercent:
                    Debug.Log("공증 옵션 제거");
                    break;
            }
        }
    }
        // DashCooldownReduction,   // 대쉬 쿨타임 5% 감소
        // DefenseBoostPercent,     // 방어력 10% 증가
        // HealthBoostPercent,      // 체력 10% 증가
        // ReloadSpeedReduction,    // 재장전 시간 5% 감소
        // AttackBoostPercent       // 공격력 5% 증가
}