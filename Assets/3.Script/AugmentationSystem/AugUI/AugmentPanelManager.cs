using UnityEngine;

public class AugmentPanelManager : MonoBehaviour
{
    public static AugmentPanelManager Instance;

    [SerializeField] private GameObject augmentPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OpenPanel()
    {
        augmentPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        augmentPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}