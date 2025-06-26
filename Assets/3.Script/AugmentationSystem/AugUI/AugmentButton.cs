using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AugmentButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image selectedOverlayImage;
    [SerializeField] private TextMeshProUGUI nameText; // ← 새로 추가된 텍스트 필드

    private AugmentData augmentData;

    public void Initialize(AugmentData data)
    {
        augmentData = data;

        // 아이콘 처리
        if (iconImage != null)
        {
            Sprite icon = augmentData.LoadIcon();
            iconImage.sprite = icon != null ? icon : GetDefaultIcon();
        }

        // 오버레이 초기화
        if (selectedOverlayImage != null)
        {
            selectedOverlayImage.gameObject.SetActive(false);
        }

        // 텍스트 표시 (예: LEGEND - MoveSpeedUp)
        if (nameText != null)
        {
            string grade = data.Grade.ToString().ToUpper();
            string type = data.Type.ToString();
            nameText.text = $"{grade}";
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
        return Resources.Load<Sprite>("Icons/AlreadySelected");
    }
}