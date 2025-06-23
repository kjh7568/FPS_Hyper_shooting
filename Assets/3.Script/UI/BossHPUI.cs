using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    private ZombieStat target;

    [SerializeField] private Slider hpSlider; // 🎯 Slider 사용
    [SerializeField] private TMP_Text nameText; // 🎯 Slider 사용

    public void Init(ZombieStat target)
    {
        this.target = target;
        
        nameText.text = target.name;
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = target.maxHealth;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
    
        // 체력 반영
        if (hpSlider != null)
        {
            hpSlider.value = target.health;
        }
    }
}