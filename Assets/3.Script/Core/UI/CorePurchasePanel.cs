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
    private int currentLevel = 0; // 현재 구매된 최고 레벨

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedCore(CoreDataSO core)
    {
        selectedCore = core;
        currentLevel = 0; // 혹은 PlayerData에서 불러오기

        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < selectedCore.levelStats.Count)
            {
                var lvData = selectedCore.levelStats[i];
                slots[i].costText.text = $"{lvData.price}Core";

                int levelIndex = i; // 캡처 방지용
                slots[i].button.onClick.RemoveAllListeners();
                slots[i].button.onClick.AddListener(() => Buy(levelIndex));

                // 순차 제한: 이전 레벨을 사야만 버튼 활성
                slots[i].button.interactable = (i == currentLevel);
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
        Debug.Log($"[{selectedCore?.coreNameKor}] {data?.level}레벨 구매! 가격: {data?.price}G");
       // 비용처리 추가필요
        CoreApplier.Instance.ApplyCore(selectedCore, data.level);

        currentLevel++;
        UpdateUI();
        CoreInfoPanel.Instance.DisplayCore(selectedCore);
    }

}