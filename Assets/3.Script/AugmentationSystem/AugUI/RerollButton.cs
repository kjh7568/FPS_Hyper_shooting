using UnityEngine;
using UnityEngine.UI;

public class RerollButton : MonoBehaviour
{
    private Button rerollButton;

    private void Awake()
    {
        rerollButton = GetComponent<Button>();
        rerollButton.onClick.AddListener(OnClickReroll);
    }

    private void OnEnable()
    {
        rerollButton.interactable = !AugmentPanelManager.Instance.HasRerolled;
    }

    private void OnClickReroll()
    {
        if (AugmentPanelManager.Instance.HasRerolled)
        {
            Debug.Log("이미 리롤을 사용했습니다.");
            rerollButton.interactable = false;
            return;
        }

        Debug.Log("증강 리롤 실행!");
        AugmentPanelManager.Instance.RerollAugments();
        rerollButton.interactable = false;
    }
}