using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CorePurchasePanel : MonoBehaviour
{
    public static CorePurchasePanel Instance;

    [System.Serializable]
    public class PurchaseSlot
    {
        public Button button;
        public TMP_Text costText;
    }

    [SerializeField] private List<PurchaseSlot> slots;

    private CoreDataSO selectedCore;
    private int currentLevel;
    // **코어별로 “구매한 최고 레벨” 을 저장**
    private Dictionary<CoreID, int> purchasedLevels = new Dictionary<CoreID, int>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedCore(CoreDataSO core)
    {
        selectedCore = core;
        // 이미 구매한 레벨이 있으면 꺼내오고, 없으면 0
        purchasedLevels.TryGetValue(core.coreID, out currentLevel);
        UpdateUI();
    }

    private void UpdateUI()
    {
        int maxLevel = selectedCore.levelStats.Count;
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < maxLevel)
            {
                var lvData = selectedCore.levelStats[i];
                slots[i].costText.text = $"{lvData.price} Core";

                // 먼저 이전에 추가된 리스너 제거
                slots[i].button.onClick.RemoveAllListeners();
                int levelIndex = i;
                slots[i].button.onClick.AddListener(() => Buy(levelIndex));

                // **순차 제한 + 최대레벨 도달 시 비활성화 처리**
                bool canBuy = (currentLevel == i) && (currentLevel < maxLevel);
                slots[i].button.interactable = canBuy;
            }
            else
            {
                slots[i].button.gameObject.SetActive(false);
                slots[i].costText.text = "";
            }
        }
    }

    private void Buy(int levelIndex)
    {
        var data = selectedCore.levelStats[levelIndex];
        Debug.Log($"[{selectedCore.coreNameKor}] {data.level}레벨 구매! 가격: {data.price} Core");

        // 실제 골드/코어 차감 로직 추가할 자리

        CoreApplier.Instance.ApplyCore(selectedCore, data.level);

        // **구매한 최고 레벨 업데이트 & 저장**
        currentLevel = levelIndex + 1;
        purchasedLevels[selectedCore.coreID] = currentLevel;

        UpdateUI();
        CoreInfoPanel.Instance.DisplayCore(selectedCore);
    }
}
