using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int savedGunLevel;
    public int savedCurrentAmmo; 
    
    public AugmentStat augmentStat = new AugmentStat(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
          augmentStat.PrintAll();
        }
    }

    public void SaveGunState(WeaponController wc)
    {
        savedGunLevel = wc.weapon.currentLevel;
        savedCurrentAmmo = wc.weapon.currentAmmo;
    }

    /// GameData에 저장된 무기 정보를 현재 무기에 복원합니다.
    public void LoadGunState(WeaponController wc)
    {
        wc.weapon.ApplyLevel(savedGunLevel);
        wc.weapon.ApplyAmmo(savedCurrentAmmo);
    }

   // public void SavePlayerStats(int health, int shield)
   // {
   //     savedPlayerHealth = health;
   //     savedPlayerShield = shield;
   // }
   // public void LoadPlayerStats(out int health, out int shield)
   // {
   //     health = savedPlayerHealth;
   //     shield = savedPlayerShield;
   // }
    
    
    
    
    
    
    
    
    
    
}