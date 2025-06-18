using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Armor dropedItem;
    
    [SerializeField] private ArmorDataSO helmetSO;
    [SerializeField] private ArmorDataSO chestplateSO;
    [SerializeField] private ArmorDataSO glovesSO;
    [SerializeField] private ArmorDataSO bootsSO;
    
    void Start()
    {
        dropedItem = GetRandomArmor();
    }

    private Armor GetRandomArmor()
    {
        int roll = Random.Range(0, 4);

        if (roll < 1) return new Armor(helmetSO, GetRandomGrade());
        else if (roll < 2) return new Armor(chestplateSO, GetRandomGrade());
        else if (roll < 3) return new Armor(glovesSO, GetRandomGrade());
        else return new Armor(bootsSO, GetRandomGrade());
    }
    
    private ArmorGrade GetRandomGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return ArmorGrade.Common;
        else if (roll < 80) return ArmorGrade.Rare;
        else if (roll < 95) return ArmorGrade.Epic;
        else return ArmorGrade.Legendary;
    }
}
