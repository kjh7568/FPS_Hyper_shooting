using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    
    public Armor dropedArmor;
    public Weapon dropedWeapon;
    
    public bool isWeapon;
    public bool isPreviousItem;
    
    [SerializeField] private ArmorDataSO helmetSO;
    [SerializeField] private ArmorDataSO chestplateSO;
    [SerializeField] private ArmorDataSO glovesSO;  
    [SerializeField] private ArmorDataSO bootsSO;
    
    [SerializeField] private WeaponDataSO[] weapons;

    void Start()
    {
        if (isPreviousItem) return;
        
        if (Random.Range(0, 2) == 0)
        {
            dropedWeapon = GetRandomWeapon();
            isWeapon = true;
        }
        else
        {
            dropedArmor = GetRandomArmor();
            isWeapon = false;
        }
    }

    #region 무기

    private Weapon GetRandomWeapon()
    {
        int roll = Random.Range(0, weapons.Length);

        return new Weapon(weapons[roll], GetRandomWeaponGrade());
    }
    
    private WeaponGrade GetRandomWeaponGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return WeaponGrade.Common;
        else if (roll < 80) return WeaponGrade.Rare;
        else if (roll < 95) return WeaponGrade.Epic;
        else return WeaponGrade.Legendary;
    }

    #endregion
    
    #region 방어구

    private Armor GetRandomArmor()
    {
        int roll = Random.Range(0, 4);

        if (roll < 1) return new Armor(helmetSO, GetRandomArmorGrade());
        else if (roll < 2) return new Armor(chestplateSO, GetRandomArmorGrade());
        else if (roll < 3) return new Armor(glovesSO, GetRandomArmorGrade());
        else return new Armor(bootsSO, GetRandomArmorGrade());
    }
    
    private ArmorGrade GetRandomArmorGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return ArmorGrade.Common;
        else if (roll < 80) return ArmorGrade.Rare;
        else if (roll < 95) return ArmorGrade.Epic;
        else return ArmorGrade.Legendary;
    }

    #endregion
}
