using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    private ZombieStat target;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text nameText;

    public void Init(ZombieStat target)
    {
        this.target = target;
        
        nameText.text = target.name;
        
        if (hpSlider != null)
        {
            hpSlider.maxValue = target.maxHealth;
        }
        
        hpSlider.gameObject.SetActive(true);
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

    public void OffHpBar()
    {
        hpSlider.gameObject.SetActive(false);
    }
}