using System.Collections.Generic;
using UnityEngine;

public class Armor
{
    public ArmorDataSO data { get; private set; }
    public int currentLevel { get; private set; }
    public ArmorLevelStat currentStat { get; private set; }
    public ArmorGrade grade => data.grade;
    public ArmorType Type => data.armorType;

    public List<SpecialEffect> options { get; private set; } = new();

    public ArmorStat Stat { get; private set; }

    public Armor(ArmorDataSO data)
    {
        this.data = data;
        Init();
    }
    public Armor(ArmorDataSO data, ArmorGrade overrideGrade)
    {
        // 기존 SO를 복제해서 grade 덮어쓰기 (원본 보호)
        this.data = ScriptableObject.Instantiate(data);
        this.data.grade = overrideGrade;
        Init();
    }

    private void Init()
    {
        GenerateRandomEffects(); // 등급에 따른 효과 수만큼 효과 선택
        ApplyLevel(DetermineStartLevelByGrade());
        BuildStat();
    }

    private void GenerateRandomEffects()
    {
        options.Clear();

        int effectCount = grade switch
        {
            ArmorGrade.Common => 0,
            ArmorGrade.Rare => 1,
            ArmorGrade.Epic => 2,
            ArmorGrade.Legendary => 3,
            _ => 0
        };

        List<SpecialEffect> pool = data.possibleEffects;
        while (options.Count < effectCount && options.Count < pool.Count)
        {
            var pick = pool[Random.Range(0, pool.Count)];
            if (!options.Contains(pick))
                options.Add(pick);
        }
    }

    private int DetermineStartLevelByGrade()
    {
        return data.GetMinLevelForGrade(); // Common은 1, Rare는 3 등
    }

    public void ApplyLevel(int level)
    {
        currentLevel = level;
        currentStat = data.GetStatByLevel(level);
    }

    public float GetDefense() => currentStat.defense;

    private void BuildStat()
    {
        Stat = new ArmorStat();
        Stat.totalDefense = currentStat.defense;
    }

    public bool HasEffect(SpecialEffect effect)
    {
        return options.Contains(effect);
    }
}