using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCoreData", menuName = "Core/CoreData")]
public class CoreDataSO : ScriptableObject
{
    public string coreName;
    public Sprite coreIcon;                   // 코어 아이콘 이미지
    [TextArea]
    public string description;
    public CoreType type;
    public int infoIndex;

    public List<CoreLevelData> levelStats = new List<CoreLevelData>();

    private void Reset()
    {
        levelStats = new List<CoreLevelData>();
        for (int i = 1; i <= 5; i++)
        {
            CoreLevelData levelData = new CoreLevelData
            {
                level = i,
                value = i * 5f, // 예시: +1, +2, ...
                price = i * 5,  // 5, 10, 15, 20, 25
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