using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<ArmorType, Armor> ArmorSlots = new();

    public Inventory()
    {
        // 4개 슬롯 미리 초기화
        foreach (ArmorType type in System.Enum.GetValues(typeof(ArmorType)))
        {
            ArmorSlots[type] = null;
        }
    }

    /// 방어구 장착 (해당 부위에 장착됨)
    public void EquipArmor(Armor armor)
    {
        ArmorType slot = armor.Type;
        ArmorSlots[slot] = armor;

        Debug.Log($"[장착] {slot} 슬롯에 {armor.data.armorName} (Lv.{armor.currentLevel}) 장착됨");
    }

    /// 해당 부위에 장착된 방어구 반환
    public Armor GetEquippedArmor(ArmorType type)
    {
        return ArmorSlots.TryGetValue(type, out var armor) ? armor : null;
    }
}