using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoreButtonUI : MonoBehaviour
{
    [SerializeField] private Image iconImage; 
    [SerializeField] private Button button;

    public CoreDataSO coreData;

    private void Start()
    {
        iconImage.sprite = coreData.coreIcon; // 이게 실제 이미지 UI에 들어가는 부분
        button.onClick.AddListener(OnClickShowInfo);
    }

    private void OnClickShowInfo()
    {
        CoreInfoPanel.Instance.DisplayCore(coreData);
        CorePurchasePanel.Instance.SetSelectedCore(coreData);
    }
}