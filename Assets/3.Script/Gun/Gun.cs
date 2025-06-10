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
    public abstract void Fire();
    public abstract void Reload();
}