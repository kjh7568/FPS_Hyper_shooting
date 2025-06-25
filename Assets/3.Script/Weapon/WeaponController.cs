using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class WeaponController : MonoBehaviour
{
    public Weapon weapon;
    
    public bool isOpenPanel = false;
    
    public WeaponDataSO weaponData; // 필요시 여기에서 weapon 생성

    public GameObject muzzleFlash;
    
    private WaitForSeconds delay = new  WaitForSeconds(0.05f);
    
    protected UIManager uiManager;
    
    private void Start()
    {
        // weapon이 null이면 여기서 생성할 수 있어야 함
        if (weapon == null && weaponData != null)
        {
            weapon = new Weapon(weaponData); // 생성자 필요
        }

        uiManager = FindObjectOfType<UIManager>();
    }
    
    protected WeaponGrade GetRandomGrade()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50) return WeaponGrade.Common;
        else if (roll < 80) return WeaponGrade.Rare;
        else if (roll < 95) return WeaponGrade.Epic;
        else return WeaponGrade.Legendary;
    }
    
    protected float GetFinalDamage()
    {
        float baseDamage = weapon.currentStat.damage;
        float attackMultiplier = CalculateAttackMultiplier(weapon.Type);

        return baseDamage * attackMultiplier * Player.localPlayer.coreStat.coreDamage;
    }

    private float CalculateAttackMultiplier(WeaponType weaponType)
    {
        float multiplier = Player.localPlayer.inventory.EquipmentStat.multiplierAttack;

        switch (weaponType)
        {
            case WeaponType.Akm:
            case WeaponType.M4:
            case WeaponType.Sniper:
            case WeaponType.Shotgun:
            case WeaponType.Ump:
                multiplier += Player.localPlayer.coreStat.primaryDamage;
                break;
            case WeaponType.Pistol:
                multiplier += Player.localPlayer.coreStat.secondaryDamage;
                break;
            case WeaponType.Knife:
                multiplier += Player.localPlayer.coreStat.meleeDamage;
                break;
            case WeaponType.Grenade:
                multiplier += Player.localPlayer.coreStat.grenadeDamage;
                break;
        }

        return multiplier;
    }

    
    public  void Init(Weapon parameter)
    {
        weapon = parameter;
        weapon.currentAmmo = weapon.currentStat.magazine;
    }

    protected IEnumerator PlayMuzzleFlash()
    {
        muzzleFlash.transform.localRotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
        muzzleFlash.SetActive(true);

        yield return delay;
        
        muzzleFlash.SetActive(false);
    }
    
    public abstract void Fire();
    public abstract void Reload();
    
}
