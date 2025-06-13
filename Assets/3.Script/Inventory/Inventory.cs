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
            equippedArmors[type] = armor;
        }
        else
        {
            equippedArmors.Add(type, armor);
        }
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
}