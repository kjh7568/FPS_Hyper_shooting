using UnityEngine;
using UnityEngine.UI;

public class AugmentButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image selectedOverlayImage; // ← 오버레이 이미지 슬롯

    private AugmentData augmentData;

    public void Initialize(AugmentData data)
    {
        augmentData = data;

        if (iconImage != null)
        {
            Sprite icon = augmentData.LoadIcon();
            iconImage.sprite = icon != null ? icon : GetDefaultIcon();
        }

        if (selectedOverlayImage != null)
        {
            selectedOverlayImage.gameObject.SetActive(false); // 초기 비활성화
        }
    }

    public void ShowSelectedOverlay()
    {
        if (selectedOverlayImage != null)
        {
            selectedOverlayImage.sprite = Resources.Load<Sprite>("Icons/AlreadySelected");
            selectedOverlayImage.gameObject.SetActive(true);
        }
    }

    private Sprite GetDefaultIcon()
    {
        return Resources.Load<Sprite>("Icons/DefaultIcon");
    }
}