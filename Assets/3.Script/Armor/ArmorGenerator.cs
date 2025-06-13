using System.Collections.Generic;
using UnityEngine;

public class ArmorGenerator : MonoBehaviour
{
    [SerializeField] private ArmorDataSO helmetSO;
    [SerializeField] private ArmorDataSO chestplateSO;
    [SerializeField] private ArmorDataSO glovesSO;
    [SerializeField] private ArmorDataSO bootsSO;

    private void Start()
    {
        List<Armor> generated = new List<Armor>();

        for (int i = 0; i < 3; i++) // 3개씩
        {
            ArmorGrade grade = GetRandomGrade();

            Armor helmet      = new Armor(helmetSO, grade);
            Armor chestplate  = new Armor(chestplateSO, grade);
            Armor gloves      = new Armor(glovesSO, grade);
            Armor boots       = new Armor(bootsSO, grade);

            generated.Add(helmet);
            generated.Add(chestplate);
            generated.Add(gloves);
            generated.Add(boots);

            PrintArmor($"Helmet {i + 1}", helmet);
            PrintArmor($"Chestplate {i + 1}", chestplate);
            PrintArmor($"Gloves {i + 1}", gloves);
            PrintArmor($"Boots {i + 1}", boots);
        }
    }

    private ArmorGrade GetRandomGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 40) return ArmorGrade.Common;
        else if (roll < 70) return ArmorGrade.Rare;
        else if (roll < 90) return ArmorGrade.Epic;
        else return ArmorGrade.Legendary;
    }

    private void PrintArmor(string label, Armor armor)
    {
        string effectList = armor.effects.Count > 0
            ? string.Join(", ", armor.effects)
            : "없음";

        Debug.Log($"[{label}] {armor.grade} {armor.Type} | Lv.{armor.currentLevel} | 방어력: {armor.GetDefense()} | 효과: {effectList}");
    }
}