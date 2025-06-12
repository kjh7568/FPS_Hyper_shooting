public class Armor
{
    public ArmorDataSO data { get; private set; }
    public int currentLevel { get; private set; }
    public ArmorLevelStat currentStat { get; private set; }

    public Armor(ArmorDataSO data)
    {
        this.data = data;
        Init();
    }

    private void Init()
    {
        currentLevel = data.grade switch
        {
            ArmorGrade.Common => 1,
            ArmorGrade.Rare => 3,
            ArmorGrade.Epic => 5,
            ArmorGrade.Legendary => 7,
            _ => 1
        };
        ApplyLevel(currentLevel);
    }

    public void ApplyLevel(int level)
    {
        currentLevel = level;
        currentStat = data.GetStatByLevel(level);
    }

    public void LevelUp()
    {
        int maxLevel = data.GetMaxLevelForGrade();
        if (currentLevel < maxLevel)
        {
            ApplyLevel(currentLevel + 1);
        }
    }

    public float GetDefense() => currentStat.defense;

    public ArmorType Type => data.armorType;
}