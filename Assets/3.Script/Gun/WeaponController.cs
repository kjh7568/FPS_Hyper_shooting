using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    
    public bool isOpenPanel = false;
    
    public WeaponDataSO gunData; // 필요시 여기에서 weapon 생성

    private void Start()
    {
        // weapon이 null이면 여기서 생성할 수 있어야 함
        if (weapon == null && gunData != null)
        {
            weapon = new Weapon(gunData); // 생성자 필요
        }
    }
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
