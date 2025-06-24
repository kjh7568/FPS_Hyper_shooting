using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCoreData", menuName = "Core/CoreData")]
public class CoreDataSO : ScriptableObject
{
    public CoreID coreID;              // 내부 식별용 enum
    public string coreNameKor;         // UI 출력용 한글 이름
    public Sprite coreIcon;            // 코어 아이콘 이미지
    [TextArea]
    public string description;         // 일반 설명
    public CoreType type;              // 타입 (Attack / Defense / Utility)
    public int infoIndex;              // UI 출력 위치

    public List<CoreLevelData> levelStats = new List<CoreLevelData>();

    private void Reset()
    {
        levelStats = new List<CoreLevelData>();
        for (int i = 1; i <= 5; i++)
        {
            CoreLevelData levelData = new CoreLevelData
            {
                level = i,
                value = i * 5f,
                price = i * 5,
                detailDescription = $"레벨 {i} 효과 설명입니다."
            };
            levelStats.Add(levelData);
        }
    }
}

[System.Serializable]
public class CoreLevelData
{
    public int level;
    public float value;
    public int price;
    [TextArea]
    public string detailDescription;
}

public enum CoreType
{
    Attack,
    Defense,
    Utility
}

public enum CoreID
{
    AttUp,
    PrimaryAttUp,
    SecondaryAttUp,
    KnifeAttUp,
    GrenadeAttUp,
    DefUp,
    HpRegenUp,
    MaxHpUp,
    MovementSpeedUp,
    CoinDropChanceUp,
    ItemDropChanceUp,
    CoinGetRangeUp,
    GrenadeCoolReduce,
    GrenadeRangeUp
}