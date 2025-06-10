using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int savedGunLevel;
    public int savedCurrentAmmo; 
   // 잠시 보류
   // public float moveSpeed;
   // public int savedPlayerHealth; 
   // public int savedPlayerShield;
    

    
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

    public void SaveGunState(Gun gun)
    {
        savedGunLevel = gun.CurrentLevel;
        savedCurrentAmmo = gun.CurrentAmmo;
    }

    /// GameData에 저장된 무기 정보를 현재 무기에 복원합니다.
    public void LoadGunState(Gun gun)
    {
        gun.ApplyLevel(savedGunLevel);
        gun.ApplyAmmo(savedCurrentAmmo);
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