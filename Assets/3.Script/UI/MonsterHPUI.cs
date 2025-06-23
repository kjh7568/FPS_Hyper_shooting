using UnityEngine;
using UnityEngine.UI;

public class MonsterHPUI : MonoBehaviour
{
    private NormalZombie target;
    private Camera cam;

    [SerializeField] private RectTransform followTarget;
    [SerializeField] private Slider hpSlider; // 🎯 Slider 사용

    public void Init(NormalZombie target, Camera cam)
    {
        this.target = target;
        this.cam = cam;

        // 🔧 자동 할당
        if (followTarget == null)
            followTarget = GetComponent<RectTransform>();

        if (hpSlider != null)
        {
            hpSlider.maxValue = target.zombieStat.maxHealth;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
    
        // 위치 갱신 (스크린 좌표 아님 → 월드 좌표로 직접 지정)
        transform.position = target.transform.position + Vector3.up * 2.0f;
    
        // 체력 반영
        if (hpSlider != null)
        {
            hpSlider.value = target.zombieStat.health;
        }

        if (target.zombieStat.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}