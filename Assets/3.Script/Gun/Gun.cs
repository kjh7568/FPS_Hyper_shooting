using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("데이터")]
    public GunDataSO gunData;

    [Header("상태")]
    [SerializeField] protected int currentLevel;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected GunLevelStat currentStat;
    protected bool isReloading;
    
    public int CurrentLevel => currentLevel;
    public int CurrentAmmo => currentAmmo;

    public bool isOpenPanel = false;
    
    // 증강관련
    protected float ReloadDamageBonus = 0f;
    protected float ReloadDamageTimer = 0f;
    
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
        // 등급별 시작 레벨
        currentLevel = gunData.grade switch
        {
            GunGrade.Normal => 1,
            GunGrade.Rare => 3,
            GunGrade.Epic => 5,
            GunGrade.Legendary => 7,
            _ => 1
        };

        ApplyLevel(currentLevel);
    }
    public void ApplyAmmo(int ammo)
    {
        currentAmmo = ammo;
    }

    public void ApplyLevel(int level)
    {
        currentLevel = level;
        currentStat = gunData.GetStatByLevel(level);
        currentAmmo = gunData.maxAmmo;
        Debug.Log($"{gunData.gunName} (Lv.{currentLevel}) 적용됨");
    }

    public void LevelUp()
    {
        int maxLevel = gunData.GetMaxLevelForGrade();
        if (currentLevel < maxLevel)
        {
            ApplyLevel(currentLevel + 1);
            Debug.Log($"레벨업 → Lv.{currentLevel}");
        }
        else
        {
            Debug.Log($"[{gunData.gunName}] 최대 레벨({maxLevel})에 도달했습니다.");
        }
    }
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
        return currentStat.damage + ReloadDamageBonus;
    }
    public abstract void Fire();
    public abstract void Reload();
}