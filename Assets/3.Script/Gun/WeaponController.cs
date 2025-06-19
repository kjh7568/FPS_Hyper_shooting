using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    
    public bool isOpenPanel = false;

    protected WeaponGrade GetRandomGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return WeaponGrade.Common;
        else if (roll < 80) return WeaponGrade.Rare;
        else if (roll < 95) return WeaponGrade.Epic;
        else return WeaponGrade.Legendary;
    }
    
    protected virtual float GetFinalDamage()
    {
        return (weapon.currentStat.damage/* + ReloadDamageBonus*/) * Player.localPlayer.inventory.EquipmentStat.multiplierAttack;
    }
    
    public  void Init(Weapon parameter)
    {
        weapon = parameter;
        weapon.currentAmmo = weapon.currentStat.magazine;
    }
    
    public abstract void Fire();
    public abstract void Reload();
}
