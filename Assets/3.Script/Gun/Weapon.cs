using UnityEngine;
using System.Collections.Generic;

public class Weapon
{
    public WeaponDataSO data { get; private set; }
    public int currentLevel { get; private set; }
    public WeaponLevelStat currentStat { get; private set; }
    public WeaponGrade grade => data.grade;
    public WeaponType Type => data.weaponType;
    
    public List<WeaponSpecialEffect> options { get; private set; } = new();

    public int currentAmmo;
    
    public bool isReloading;
    
    // 증강관련
    public float ReloadDamageBonus = 0f;
    public float ReloadDamageTimer = 0f;

    public Weapon(WeaponDataSO data)
    {
        this.data = data;
        Init();
    }
    
    public Weapon(WeaponDataSO data, WeaponGrade overrideGrade)
    {
        // 기존 SO를 복제해서 grade 덮어쓰기 (원본 보호)
        this.data = ScriptableObject.Instantiate(data);
        this.data.grade = overrideGrade;
        Init();
    }

    public void ApplyLevel(int level)
    {
        currentLevel = level;
        currentStat = data.GetStatByLevel(level);

        // int bonusAmmo = GetBonusAmmoByGrade();
        // currentAmmo = data.maxAmmo + bonusAmmo;
    }    

    private void Init()
    {
        GenerateRandomEffects(); // 등급에 따른 효과 수만큼 효과 선택
        ApplyLevel(data.GetMinLevelForGrade());
        // BuildStat();
    }
    public void GenerateRandomEffects()
    {
        options.Clear();
        
        int effectCount = grade switch
        {
            WeaponGrade.Common => 0,
            WeaponGrade.Rare => 1,
            WeaponGrade.Epic => 2,
            WeaponGrade.Legendary => 3,
            _ => 0
        };

        List<WeaponSpecialEffect> pool = data.possibleEffects;
        while (options.Count < effectCount && options.Count < pool.Count)
        {
            var pick = pool[Random.Range(0, pool.Count)];
            if (!options.Contains(pick))
                options.Add(pick);
        }
    }
    private void SetStartLevelByGrade()
    {
        currentLevel = grade switch
        {
            WeaponGrade.Common => 1,
            WeaponGrade.Rare => 3,
            WeaponGrade.Epic => 5,
            WeaponGrade.Legendary => 7,
            _ => 1
        };
    }
    public void ApplyAmmo(int ammo)
    {
        currentAmmo = ammo;
    }
    private int GetBonusAmmoByGrade()
    {
        return grade switch
        {
            WeaponGrade.Rare => 1,
            WeaponGrade.Epic => 3,
            WeaponGrade.Legendary => 5,
            _ => 0
        };
    }

    #region trash

    // protected virtual void Update()
    // {
    //     // [ 버프 시간 감소 처리]
    //     if (ReloadDamageTimer > 0f)
    //     {
    //         ReloadDamageTimer -= Time.deltaTime;
    //         if (ReloadDamageTimer <= 0f)
    //         {
    //             ReloadDamageBonus = 0f;
    //             ReloadDamageTimer = 0f;
    //             Debug.Log("[버프 종료] 장전 후 공격력 증가가 끝났습니다.");
    //         }
    //     }
    // }
    // protected void InitGun()
    // {
    //     GenerateRandomEffects();      // 특수효과 랜덤 부여
    //     SetStartLevelByGrade();       // 레벨 초기화
    //
    //     ApplyLevel(currentLevel);
    //
    //     Debug.Log($"[{data.gunName}] 생성됨 - 등급: {grade}, Lv.{currentLevel}, 효과 {options.Count}개");
    //     foreach (var effect in options)
    //     {
    //         Debug.Log($"▶ 특수효과: {effect}");
    //     }
    // }
    // private void DetermineGrade()
    // {
    //     switch (options.Count)
    //     {
    //         case 0: grade = WeaponGrade.Common; break;
    //         case 1: grade = WeaponGrade.Rare; break;
    //         case 2: grade = WeaponGrade.Epic; break;
    //         default: grade = WeaponGrade.Legendary; break;
    //     }
    // }
    //
    // 증강 관련 코드임.
    // protected virtual void OnReloadComplete()
    // {
    //     var stat = GameData.Instance.augmentStat;
    //
    //     if (stat.reloadDamageBonus > 0f)
    //     {
    //         ReloadDamageBonus = stat.reloadDamageBonus;
    //         ReloadDamageTimer = stat.reloadBuffDuration;
    //
    //         Debug.Log($"[버프 적용] 장전 후 {ReloadDamageTimer}초간 공격력 +{ReloadDamageBonus}");
    //     }
    // }
    //
    
    //
    // public  void Fire();
    // public  void Reload();

    #endregion
}