using UnityEngine;
using System.Collections.Generic;

public class Weapon
{
    public WeaponDataSO data { get; private set; }
    public int currentLevel { get; private set; }
    public GunLevelStat currentStat { get; private set; }

    public ArmorGrade grade => data.grade;
    public ArmorType Type => data.armorType;
    
    public int currentAmmo;
    protected bool isReloading;
    
    public int CurrentLevel => currentLevel;
    public int CurrentAmmo => currentAmmo;

    public bool isOpenPanel = false;
    
    // 증강관련
    protected float ReloadDamageBonus = 0f;
    protected float ReloadDamageTimer = 0f;
    
    // 특수효과 및 등급
    [SerializeField] private WeaponGrade grade;
    [SerializeField] private List<GunSpecialEffect> specialEffects = new();

    protected virtual void Start()
    {
        InitGun();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            LevelUp();
        }
        // [ 버프 시간 감소 처리]
        if (ReloadDamageTimer > 0f)
        {
            ReloadDamageTimer -= Time.deltaTime;
            if (ReloadDamageTimer <= 0f)
            {
                ReloadDamageBonus = 0f;
                ReloadDamageTimer = 0f;
                Debug.Log("[버프 종료] 장전 후 공격력 증가가 끝났습니다.");
            }
        }
    }

    protected void InitGun()
    {
        GenerateRandomEffects();      // 특수효과 랜덤 부여
        DetermineGrade();             // 등급 결정
        SetStartLevelByGrade();       // 레벨 초기화

        ApplyLevel(currentLevel);

        Debug.Log($"[{data.gunName}] 생성됨 - 등급: {grade}, Lv.{currentLevel}, 효과 {specialEffects.Count}개");
        foreach (var effect in specialEffects)
        {
            Debug.Log($"▶ 특수효과: {effect}");
        }
    }
    private void GenerateRandomEffects()
    {
        specialEffects.Clear();
        List<GunSpecialEffect> pool = new((GunSpecialEffect[])System.Enum.GetValues(typeof(GunSpecialEffect)));

        int effectCount = Random.Range(0, 4); // 0~3개
        while (specialEffects.Count < effectCount && pool.Count > 0)
        {
            int idx = Random.Range(0, pool.Count);
            specialEffects.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
    }
    private void DetermineGrade()
    {
        switch (specialEffects.Count)
        {
            case 0: grade = WeaponGrade.Common; break;
            case 1: grade = WeaponGrade.Rare; break;
            case 2: grade = WeaponGrade.Epic; break;
            default: grade = WeaponGrade.Legendary; break;
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

    public void ApplyLevel(int level)
    {
        currentLevel = level;
        currentStat = data.GetStatByLevel(level);

        int bonusAmmo = GetBonusAmmoByGrade();
        currentAmmo = data.maxAmmo + bonusAmmo;
        
        Debug.Log($"{data.gunName} (Lv.{currentLevel}) 적용됨");
    }

    public void LevelUp()
    {
        int maxLevel = data.GetMaxLevelForGrade();
        if (currentLevel < maxLevel)
        {
            ApplyLevel(currentLevel + 1);
            Debug.Log($"레벨업 → Lv.{currentLevel}");
        }
        else
        {
            Debug.Log($"[{data.gunName}] 최대 레벨({maxLevel})에 도달했습니다.");
        }
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
    // 증강 관련 코드임.
    protected virtual void OnReloadComplete()
    {
        var stat = GameData.Instance.augmentStat;

        if (stat.reloadDamageBonus > 0f)
        {
            ReloadDamageBonus = stat.reloadDamageBonus;
            ReloadDamageTimer = stat.reloadBuffDuration;

            Debug.Log($"[버프 적용] 장전 후 {ReloadDamageTimer}초간 공격력 +{ReloadDamageBonus}");
        }
    }
    protected virtual float GetFinalDamage()
    {
        return (currentStat.damage + ReloadDamageBonus) * Player.localPlayer.inventory.armorStat.multiplierAttack;
    }
    public abstract void Fire();
    public abstract void Reload();
}