using UnityEngine;

public class CorePanelManager : MonoBehaviour
{
    public static CorePanelManager instance;

    [SerializeField] private GameObject corePanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        corePanel.SetActive(false);
    }

    private void Update()
    {
        if (corePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        corePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FindObjectOfType<PlayerController>().isOpenPanel = true;
        WeaponManager.instance.currentWeapon.isOpenPanel = true;
    }

    public void ClosePanel()
    {
        corePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PlayerController>().isOpenPanel = false;
        WeaponManager.instance.currentWeapon.isOpenPanel = false;
    }
}
