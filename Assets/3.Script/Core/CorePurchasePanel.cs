using UnityEngine;
using UnityEngine.UI;

public class CorePurchasePanel : MonoBehaviour
{
    public static CorePurchasePanel Instance;

    [SerializeField] private Button buyButton;
    private CoreDataSO selectedCore;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSelectedCore(CoreDataSO data)
    {
        selectedCore = data;
    }

    private void Start()
    {
        buyButton.onClick.AddListener(BuyCore);
    }

    private void BuyCore()
    {
        if (selectedCore == null) return;

        var lv1 = selectedCore.levelStats.Find(x => x.level == 1);
        if (lv1 != null)
        {
            Debug.Log($"구매한 코어: {selectedCore.coreName} | 가격: {lv1.price}");
            // 추후 PlayerData.Instance.UseGold(lv1.price); 등으로 연동
        }
    }
}